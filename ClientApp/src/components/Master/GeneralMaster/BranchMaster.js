import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import StateDrop from '../../Util/StateDrop.js';
import CityDrop from '../../Util/CityDrop.js';

export default class BrancMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            BranchList: [],
            loading: true,
            FormGroup: {
                BranchId: 0,
                BranchCode: '',
                BranchName: '',
                RegistrationDate: '',
                InchargePerson: '',
                Address: '',
                CityId: 0,
                StateId: 0,
                Pincode: '',
                ContactNo: ''
            },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //this.renderBranchTable = this.renderBranchTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillBranch = this.fillBranch.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneBranch/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["BranchId"] = response.data[0].BranchID;
                UserInput["BranchCode"] = response.data[0].BranchCode;
                UserInput["BranchName"] = response.data[0].BranchName;
                UserInput["RegistrationDate"] = response.data[0].RDate;
                UserInput["InchargePerson"] = response.data[0].InchargePerson;
                UserInput["Address"] = response.data[0].Address;
                UserInput["StateId"] = response.data[0].StateID;
                UserInput["CityId"] = response.data[0].CityID;
                UserInput["Pincode"] = response.data[0].PinCode;
                UserInput["ContactNo"] = response.data[0].ContactNo;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillBranch();
            });
    }


    fillBranch() {
        axios.get('api/Masters/GetBranch')
            .then(response => {
                console.log(response.data);
                this.setState({ BranchList: response.data, loading: false });
            });
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
        
        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });
        
    }

    handleSubmit(event) {
        event.preventDefault();

        const Branch = this.state.FormGroup

        axios.post(`api/Masters/SaveBranch`, Branch)
            .then(res => {
                this.FillForm(0);
                this.setState({
                    Message: res.data[0].Message
                });
            })
        $(document).ready(function () {
            $(".message").show();
        });
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

            axios.get(`api/Masters/DeleteBranch/${ID}`)
                .then(res => {
                    this.fillBranch();
                    this.setState({
                        Message: res.data[0].Message
                    });
                })

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

        const renderBranchTable = (BranchList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Branch Code</th>
                        <th>Branch Name</th>
                        <th>Open Date</th>
                        <th>Incharge Person</th>
                        <th>Address</th>
                        <th>City</th>
                        <th>State</th>
                        <th>Pincode</th>
                        <th>Contact No.</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    {BranchList.map((forecast, i) =>

                        <tr key={forecast.BranchID}>
                            <td>{i + 1}</td>
                            <td>{forecast.BranchCode}</td>
                            <td>{forecast.BranchName}</td>
                            <td>{forecast.ReDate}</td>
                            <td>{forecast.InchargePerson}</td>
                            <td>{forecast.Address}</td>
                            <td>{forecast.CityName}</td>
                            <td>{forecast.StateName}</td>
                            <td>{forecast.PinCode}</td>
                            <td>{forecast.ContactNo}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.BranchID)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.BranchID)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

        let BranchList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderBranchTable(this.state.BranchList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="Cityform" method="post" action="#">
                      <h1>Branch</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                            <div className="col-md-12 col-sm-12 col-xs-12">
                                <div className="col-md-4 col-sm-12 col-xs-12 required">
                                    <label >Branch Code</label>
                                  <input type="text" className="full-width" name="BranchCode" value={this.state.FormGroup.BranchCode} onChange={this.handleInputChange} required />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label >Branch Name</label>
                                  <input type="text" className="full-width" name="BranchName" value={this.state.FormGroup.BranchName} onChange={this.handleInputChange} required />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>Open Date</label>
                                  <input type="date" className="full-width" name="RegistrationDate" value={this.state.FormGroup.RegistrationDate} onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label >Incharge Person</label>
                                  <input type="text" className="full-width" name="InchargePerson" value={this.state.FormGroup.InchargePerson} onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12" >
                                    <label >Address</label>
                                  <input type="text" className="full-width" name="Address" value={this.state.FormGroup.Address} onChange={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>State</label>
                                    <StateDrop StateId={this.state.FormGroup.StateId} func={this.handleInputChange} />             
                                </div>
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>City</label>
                                    <CityDrop CityId={this.state.FormGroup.CityId} StateId={this.state.FormGroup.StateId} func={this.handleInputChange} />
     
                                </div>
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>Pin Code</label>
                                  <input type="text" className="full-width" maxLength="6" name="Pincode" value={this.state.FormGroup.Pincode} onChange={this.handleInputChange} />
                                </div>
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                    <label>Contact No.</label>
                                  <input type="text" className="full-width" name="ContactNo" value={this.state.FormGroup.ContactNo} onChange={this.handleInputChange} />
                                </div>
                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                    </div>
                                </div>
                            </div>
                                  
                        </div>

                        {BranchList}

                  </form>
                </div>
            </section>
          
    );
  }
}
