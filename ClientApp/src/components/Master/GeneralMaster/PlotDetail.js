import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import ProjectDrop from '../../Util/ProjectDrop';
import SectorDrop from '../../Util/SectorDrop';
import BlockDrop from '../../Util/BlockDrop';
import SegmentDrop from '../../Util/SegmentDrop';
import PlotTypeDrop from '../../Util/PlotTypeDrop';
import RateDrop from '../../Util/RateDrop';

export default class PlotDetail extends Component {

    constructor(props) {
        super(props);
        this.state = {
            PlotDetailList: [],
            loading: true,
            FormGroup: {
                PlotDetailId: 0,
                ProjectId: 0,
                SectorId: 0,
                BlockId: 0,
                SegmentId: 0,
                PlotTypeId: 0,
                Rate: 0,
                Area: '',
                PlotNo: '',
                FromPlot: 1,
                ToPlot: 1,
            },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillPlotDetail = this.fillPlotDetail.bind(this);
        this.Confirm = this.Confirm.bind(this);
        this.FillForm(0);

    }
    
    FillForm = (ID) => {
        axios.get(`api/Masters/GetOnePlotDetail/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["PlotDetailId"] = response.data[0].PlotDetailId;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["SectorId"] = response.data[0].SectorId;
                UserInput["BlockId"] = response.data[0].BlockId;
                UserInput["SegmentId"] = response.data[0].SegmentId;
                UserInput["PlotTypeId"] = response.data[0].PlotTypeId;
                UserInput["Rate"] = response.data[0].Rate;
                UserInput["Area"] = response.data[0].Area;
                UserInput["PlotNo"] = response.data[0].PlotNo;
                UserInput["FromPlot"] = 1;
                UserInput["ToPlot"] = 1;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillPlotDetail();
            });
    }


    fillPlotDetail = () => {
        axios.get('api/Masters/FillPlotDetail')
            .then(response => {
                console.log(response.data);
                this.setState({ PlotDetailList: response.data, loading: false });
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
        
    }

    handleSubmit = (event) => {
        event.preventDefault();

        const Plot = this.state.FormGroup

        axios.post(`api/Masters/vrifyPlotDetail`, Plot)
            .then(res => {
                this.setState({
                    Message: res.data[0].Message
                });

                if (res.data[0].MessageType === 1) {

                    axios.post(`api/Masters/SavePlotDetail`, Plot)
                        .then(res => {
                            this.setState({
                                Message: res.data[0].Message
                            });
                            this.FillForm(0);
                        })
                }
                else {
                    this.Confirm(res.data[0].Message);
                }
            })

        $(document).ready(function () {
            $(".message").show();
        });
    }


    Confirm = (message) => {

        const Plot = this.state.FormGroup;

        var body = document.getElementsByTagName('body')[0];
        var overlay = document.createElement('div');
        overlay.className = 'myConfirm';
        var box = document.createElement('div');
        var boxchild = document.createElement('div');
        var p = document.createElement('p');
        p.appendChild(document.createTextNode("Plot No " + message + " Already Exist, Do you want to Update"));
        boxchild.appendChild(p);
        var yesButton = document.createElement('button');
        var noButton = document.createElement('button');
        yesButton.appendChild(document.createTextNode('Yes'));
        yesButton.addEventListener('click', () => {

            axios.post(`api/Masters/SavePlotDetail`, Plot)                .then(res => {                    this.setState({
                        Message: res.data[0].Message
                    });                    this.FillForm(0);                })

            $(document).ready(function () {
                $(".message").show();
            });

            body.removeChild(overlay);
        }, false);

        noButton.appendChild(document.createTextNode('No'));
        noButton.addEventListener('click', () => {

            body.removeChild(overlay);

        }, false);
        boxchild.appendChild(yesButton);
        boxchild.appendChild(noButton);
        box.appendChild(boxchild);
        overlay.appendChild(box)
        body.appendChild(overlay);

    }
    

    Delete(ID) {
        var body = document.getElementsByTagName('body')[0];
        var overlay = document.createElement('div');
        overlay.setAttribute("id", "myConfirm");
        var box = document.createElement('div');
        var boxchild = document.createElement('div');
        var p = document.createElement('p');
        p.appendChild(document.createTextNode("Are you sure"));
        boxchild.appendChild(p);
        var yesButton = document.createElement('button');
        var noButton = document.createElement('button');
        yesButton.appendChild(document.createTextNode('Yes'));
        yesButton.addEventListener('click', () => {

            axios.get(`api/Masters/DeletePlotDetail/${ID}`)                .then(res => {                    this.fillPlotDetail();                    this.setState({
                        Message: res.data[0].Message
                    });                })

            $(document).ready(function () {
                $(".message").show();
            });

            body.removeChild(overlay);
        }, false);

        noButton.appendChild(document.createTextNode('No'));
        noButton.addEventListener('click', () => {

            body.removeChild(overlay);

        }, false);
        boxchild.appendChild(yesButton);
        boxchild.appendChild(noButton);
        box.appendChild(boxchild);
        overlay.appendChild(box)
        body.appendChild(overlay);
    }
 
    
    
    render() {

        const renderPlotTable = (PlotDetailList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>ProjectName</th>
                        <th>SectorName</th>
                        <th>BlockName</th>
                        <th>SegmentName</th>
                        <th>PlottypeName</th>
                        <th>Rate</th>
                        <th>Area</th>
                        <th>PlotNo</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    {PlotDetailList.map((forecast, i) =>

                        <tr key={forecast.PlotDetailId}>
                            <td>{i + 1}</td>
                            <td>{forecast.ProjectName}</td>
                            <td>{forecast.SectorName}</td>
                            <td>{forecast.BlockName}</td>
                            <td>{forecast.SegmentName}</td>
                            <td>{forecast.PlottypeName}</td>
                            <td>{forecast.Rate}</td>
                            <td>{forecast.Area}</td>
                            <td>{forecast.PlotNo}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.PlotDetailId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.PlotDetailId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

        let PlotDetailList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderPlotTable(this.state.PlotDetailList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Plot Detail</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          <div className="col-md-12 col-sm-12 col-xs-12">

                                <div className="col-md-4 col-sm-12 col-xs-12 required">
                                    <label>Project</label>
                                    <ProjectDrop ProjectId={this.state.FormGroup.ProjectId} func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Sector</label>
                                      <SectorDrop ProjectId={this.state.FormGroup.ProjectId} SectorId={this.state.FormGroup.SectorId} func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Block</label>
                                      <BlockDrop SectorId={this.state.FormGroup.SectorId} BlockId={this.state.FormGroup.BlockId} func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Segment</label>
                                      <SegmentDrop SegmentId={this.state.FormGroup.SegmentId} func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Plot Type</label>
                                      <PlotTypeDrop PlotTypeId={this.state.FormGroup.PlotTypeId} func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>Rate</label>
                                      <RateDrop Rate={this.state.FormGroup.Rate} func={this.handleInputChange} />             
                                </div>
                              
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Area</label>
                                  <input type="text" className="full-width" name="Area" value={this.state.FormGroup.Area} onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Plot No</label>
                                  <input type="text" className="full-width" name="PlotNo" value={this.state.FormGroup.PlotNo} onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                      <label>No. of Plot</label>

                                      <div className="col-md-6 col-sm-6 col-xs-6 pt0">
                                        <input type="text" className="full-width" name="FromPlot" value={this.state.FormGroup.FromPlot} onChange={this.handleInputChange} />
                                      </div>

                                      <div className="col-md6 col-sm-6 col-xs-6 pt0">
                                        <input type="text" className="full-width" name="ToPlot" value={this.state.FormGroup.ToPlot} onChange={this.handleInputChange} />
                                      </div>
                                  
                                </div>

                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                  </div>
                                </div>

                            </div>
                                  
                        </div>

                      {PlotDetailList}

                  </form>
                </div>
            </section>
          
    );
  }
}
