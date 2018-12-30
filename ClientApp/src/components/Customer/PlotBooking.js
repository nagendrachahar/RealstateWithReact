import React, { Component } from 'react';
//import * as jsPDF from 'jspdf';
//import html2canvas from 'html2canvas';
//import ReactDOMServer from "react-dom/server";
import axios from 'axios';
import { connect } from 'react-redux';
import { changeState } from '../../store/action/action';
import $ from 'jquery';
import Message from '../Message';
import CustomerDrop from '../Util/CustomerDrop';
import EmployeeDrop from '../Util/EmployeeDrop';
import ProjectDrop from '../Util/ProjectDrop';
import SectorDrop from '../Util/SectorDrop';
import BlockDrop from '../Util/BlockDrop';
import PlotDrop from '../Util/PlotDrop';
import EMIPlanDrop from '../Util/EMIPlanDrop';
import PayModeDrop from '../Util/PayModeDrop';

class PlotBooking extends Component {

    constructor(props) {
        super(props);
        this.state = {
            BookingList: [],
            loading: true,
            FormGroup: {
                BookingId: 0,
                CustomerId: 0,
                EmployeeId: 0,
                BookingCode: '',
                ProjectId: 0,
                SectorId: 0,
                BlockId: 0,
                PlotId: 0,
                ActualPlotAmt: 0,
                PayableAmt: 0,
                BookingAmt: 0,
                BookingDate: '',
                PayMode: 0,
                Remark: '',
                EMIPlanId: 0,
                EMIAmt: 0,
                FirstEMIDate: '',
                isUpdate: '0'
            },
            EMIValue: 0,
            EMIDuration: 0,
            Message: 'wait....!'
          
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        //this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        //this.fillBooking = this.fillBooking.bind(this);
        //this.applyEmi = this.applyEmi.bind(this);
        //this.applyBookingAmt = this.applyBookingAmt.bind(this);
        //this.printPDF = this.printPDF.bind(this);
        this.FillForm(0);
    }

    _changeState = () => {
      this.props.changeStateToReducer(this.state.userName);
    }

    _changeUserInput = (e) => {

      this.setState({
        userName: e.target.value

      });

  }

    GetFormattedDate = (D = new Date().getFullYear() + "-" + (new Date().getMonth()+1) + "-" + new Date().getDate()) => {
        
        if (D === "" || D === null) {
            D = new Date().getFullYear() + "-" + (new Date().getMonth()+1) + "-" + new Date().getDate();
        }
        return D.slice(0, 10);
    }
    
    FillForm = (ID) => {
        axios.get(`api/Masters/GetOnePlotBooking/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;

                UserInput["CustomerId"] = response.data[0].CustomerId;
                UserInput["EmployeeId"] = response.data[0].EmployeeId;
                UserInput["BookingId"] = response.data[0].BookingId;
                UserInput["BookingCode"] = response.data[0].BookingCode;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["SectorId"] = response.data[0].SectorId;
                UserInput["BlockId"] = response.data[0].BlockId;
                UserInput["PlotId"] = response.data[0].PlotId;
                UserInput["ActualPlotAmt"] = response.data[0].ActualPlotAmt;
                UserInput["PayableAmt"] = response.data[0].PayableAmt;
                UserInput["BookingAmt"] = response.data[0].BookingAmt;
                UserInput["BookingDate"] = this.GetFormattedDate(response.data[0].BookingDate);
                UserInput["PayMode"] = response.data[0].PayMode;
                UserInput["EMIPlanId"] = response.data[0].EMIPlanId;
                UserInput["EMIAmt"] = response.data[0].EMIAmount;
                UserInput["FirstEMIDate"] = this.GetFormattedDate(response.data[0].FirstEMIDate);
                UserInput["Remark"] = response.data[0].Remark;
                UserInput["isUpdate"] = response.data[0].PlotId;

                this.setState({
                    FormGroup: UserInput,
                    EMIValue: response.data[0].PlanValue,
                    EMIDuration: response.data[0].Duration
                });
                
                this.fillBooking();
            });
    }

    fillBooking = () => {
            axios.get('api/Masters/FillPlotBooking')
            .then(response => {
                this.setState({ BookingList: response.data, loading: false });
            });
    }

    handleInputChange = (event) => {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        
        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });

        if (name === "PlotId") {
            this.getActualAmt(value);
        }

        if (name === "EMIPlanId") {
            this.applyEmi(value);
        } 

        if (name === "BookingAmt") {
            this.applyBookingAmt(value);
        }
        
    }

    getActualAmt = (PlotId) => {

      axios.get(`api/Masters/getActualPlotAmt/${PlotId}`)
        .then(response => {
            console.log(response.data);
            const UserInput = this.state.FormGroup;

            UserInput["ActualPlotAmt"] = response.data[0].ActualAmt;
            UserInput["EMIPlanId"] = "0";
            UserInput["PayableAmt"] = response.data[0].ActualAmt;

            this.setState({
              FormGroup: UserInput
            });

        });
    }

    applyEmi = (EmiId) => {

    axios.get(`api/Masters/getEmiValue/${EmiId}`)
          .then(response => {
              //console.log(response.data);
              const UserInput = this.state.FormGroup;
              var EMIPer = this.state.EMIValue;
              var Duration = this.state.Duration;
              var PayableAmt = this.state.FormGroup.ActualPlotAmt - this.state.FormGroup.BookingAmt;

              if (response.data.length > 0) {
                  Duration = response.data[0].Duration;
                  EMIPer = response.data[0].PlanValue;
                  const payAmt = (((PayableAmt * response.data[0].PlanValue) / 100) + PayableAmt);
                  UserInput["PayableAmt"] = (Number(payAmt) + Number(this.state.FormGroup.BookingAmt)).toFixed(2);
                  UserInput["EMIAmt"] = (Number(payAmt) / Number(Duration)).toFixed(2);
              }
              else {
                  UserInput["PayableAmt"] = this.state.FormGroup.ActualPlotAmt;
                  UserInput["EMIAmt"] = 0;
                  EMIPer = 0;
                  Duration = 0;
              }
        
              this.setState({
                   FormGroup: UserInput,
                   EMIValue: EMIPer,
                   EMIDuration: Duration
              });
        
          });
          
    }

    applyBookingAmt = (BookingAmt) => {
        
        var BookAmt = 0;
        if (this.state.FormGroup.BookingAmt === "") {
            BookAmt = 0;
        }
        else {
            BookAmt = this.state.FormGroup.BookingAmt;
        }

        if (this.state.EMIValue > 0) {
            const UserInput = this.state.FormGroup;
            var PayableAmt = this.state.FormGroup.ActualPlotAmt - BookAmt;

            const PayAmt = (((PayableAmt * this.state.EMIValue) / 100) + PayableAmt);
            UserInput["PayableAmt"] = (Number(PayAmt) + Number(BookAmt)).toFixed(2);
            UserInput["EMIAmt"] = (Number(PayAmt) / Number(this.state.EMIDuration)).toFixed(2);
            this.setState({
                FormGroup: UserInput
            });
        }
    }

    handleSubmit = (event) => {
      event.preventDefault();
      
      const Booking = this.state.FormGroup;

      axios.post(`api/Masters/SavePlotBookingDetail`, Booking)
          .then(res => {
              console.log(res.data);
            this.setState({
              Message: res.data[0].Message
            });

            if (res.data[0].MessageType === "1"){
                this.FillForm(0);
                window.open(res.data[0].FilePath);
            }
        });

        $(document).ready(function () {
            $(".message").show();
        });
        
    }

    //printPDF = () => {
    //    const input = document.getElementById('divToPrint');
    //    html2canvas(input)
    //        .then((canvas) => {
    //            const imgData = canvas.toDataURL('image/png');
    //            const pdf = new jsPDF('landscape', 'mm', 'a4');
    //            pdf.addImage(imgData, 'JPEG', 0, 0);
    //            // pdf.output('dataurlnewwindow');
    //            pdf.save("download.pdf");
    //        });
    //}
    
  
    render() {
      const renderBookingTable = (BookingList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>BookingCode</th>
                        <th>ActualPlotAmt</th>
                        <th>PayableAmt</th>
                        <th>BookingAmt</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                {BookingList.map((forecast, i) => 
                  <tr key={forecast.BookingId}>
                    <td>{i + 1}</td>
                    <td>{forecast.BookingCode}</td>
                    <td>{forecast.ActualPlotAmt}</td>
                    <td>{forecast.PayableAmt}</td>
                    <td>{forecast.BookingAmt}</td>
                    <td className="table-actions" style={{ textAlign: "center" }}>
                        <a onClick={this.FillForm.bind(this, forecast.BookingId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                    </td>
                  </tr>
                )}
                </tbody>
            </table>
        )
      }

      let BookingList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
          : renderBookingTable(this.state.BookingList);

  
      return (
          <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>PLot Booking</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          <div className="col-md-12 col-sm-12 col-xs-12">

                              <div style={{ width: "100%", borderBottom: "1px solid #999999", paddingBottom:"15px" }}>
                                  <div className="col-md-4 col-sm-12 col-xs-12 required">
                                      <label>Customer</label>
                                      <CustomerDrop
                                        CustomerId={this.state.FormGroup.CustomerId}
                                        func={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12 required">
                                      <label>Employee</label>
                                      <EmployeeDrop
                                        EmployeeId={this.state.FormGroup.EmployeeId}
                                        func={this.handleInputChange} />
                                  </div>

                                  <i className="clearfix"></i>
                      
                              </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>Booking Code</label>
                                    <input
                                      type="text"
                                      className="full-width"
                                      name="BookingCode"
                                      value={this.state.FormGroup.BookingCode}
                                      onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12 required">
                                    <label>Project</label>
                                    <ProjectDrop
                                      ProjectId={this.state.FormGroup.ProjectId}
                                      func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>Sector</label>
                                    <SectorDrop
                                      ProjectId={this.state.FormGroup.ProjectId}
                                      SectorId={this.state.FormGroup.SectorId}
                                      func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Block</label>
                                      <BlockDrop
                                        SectorId={this.state.FormGroup.SectorId}
                                        BlockId={this.state.FormGroup.BlockId}
                                        func={this.handleInputChange} />
                                </div>
                              
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Plot</label>
                                      <PlotDrop
                                        isBooked='0'
                                        BlockId={this.state.FormGroup.BlockId}
                                        PlotId={this.state.FormGroup.PlotId}
                                        func={this.handleInputChange}
                                        isUpdate={this.state.FormGroup.isUpdate}/>

                                </div>
                              
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Actual Amount</label>
                                      <input
                                        type="text"
                                        className="full-width"
                                        name="ActualPlotAmt"
                                        value={this.state.FormGroup.ActualPlotAmt}
                                        onChange={this.handleInputChange}
                                        readOnly="true" />
                                </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12 required">
                                      <label>EMI Plan</label>
                                      <EMIPlanDrop
                                        EMIPlanId={this.state.FormGroup.EMIPlanId}
                                        func={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Payable Amount</label>
                                      <input
                                        type="text"
                                        className="full-width"
                                        name="PayableAmt"
                                        value={this.state.FormGroup.PayableAmt}
                                        onChange={this.handleInputChange}
                                        readOnly="true"/>
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Booking Amount</label>
                                      <input
                                          type="text"
                                          className="full-width"
                                          name="BookingAmt"
                                          value={this.state.FormGroup.BookingAmt}
                                          onChange={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>EMI</label>
                                      <input
                                          type="text"
                                          className="full-width"
                                          name="EMIAmt"
                                          value={this.state.FormGroup.EMIAmt}
                                          onChange={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Booking Date</label>
                                      <input
                                          type="date"
                                          className="full-width"
                                          name="BookingDate"
                                          value={this.state.FormGroup.BookingDate}
                                          onChange={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>First EMI Date</label>
                                      <input
                                          type="date"
                                          className="full-width"
                                          name="FirstEMIDate"
                                          value={this.state.FormGroup.FirstEMIDate}
                                          onChange={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12 required">
                                      <label>Pay Mode</label>
                                      <PayModeDrop
                                        PayMode={this.state.FormGroup.PayMode}
                                        func={this.handleInputChange} />
                                  </div>

                                  <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Remark</label>
                                      <input
                                          type="text"
                                          className="full-width"
                                          name="Remark"
                                          value={this.state.FormGroup.Remark}
                                          onChange={this.handleInputChange} />
                                  </div>

                                    <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                        <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                            <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                        </div>
                                    </div>

                            </div>
                                  
                        </div>

                    {BookingList}

                  </form>
                </div>
            </section>
          
    );
  }
}

function mapStateToProps(state) {
  return ({
    userName: state.rootReducer.userName
  });

}

function mapDispatchToProps(dispatch) {
  return ({
    changeStateToReducer: (updatedName) => {
      dispatch(changeState(updatedName));
    }
  });
}

export default connect(mapStateToProps, mapDispatchToProps)(PlotBooking);
