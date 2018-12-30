import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import ProjectDrop from '../Util/ProjectDrop.js';
import PurposeDrop from '../Util/PurposeDrop.js';

export class VisitorEntry extends Component {

    constructor(props) {
        super(props);
        this.state = {
            VisitorList: [],
            loading: true,
            FormGroup: {
                VisitorId: 0,
                ProjectId: 0,
                VisitorName: '',
                ContactNo: '',
                EmailId: '',
                PurposeId: 0,
                VisitDate: ''
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
        this.fillVisitorEntry = this.fillVisitorEntry.bind(this);
        this.FillForm(0);

    }

    GetFormattedDate = (D) => {
        return D.slice(0, 10);
    }
    
    FillForm = (ID) => {
        axios.get(`api/Masters/GetOneVisitorEntry/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["VisitorId"] = response.data[0].VisitorId;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["VisitorName"] = response.data[0].VisitorName;
                UserInput["ContactNo"] = response.data[0].ContactNo;
                UserInput["EmailId"] = response.data[0].EmailId;
                UserInput["PurposeId"] = response.data[0].PurposeId;
                UserInput["VisitDate"] = this.GetFormattedDate(response.data[0].VisitDate);

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillVisitorEntry();
            });
    }


    fillVisitorEntry = () => {
        axios.get('api/Masters/FillVisitorEntry')
            .then(response => {
                console.log(response.data);
                this.setState({ VisitorList: response.data, loading: false });
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

        const VisitorEntry = this.state.FormGroup

        axios.post(`api/Masters/SaveVisitorEntry`, VisitorEntry)
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

    Delete = (ID) => {
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

            axios.delete(`api/Masters/DeleteVisitorEntry/${ID}`)
                .then(res => {
                    this.fillVisitorEntry();
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
 
    
    
    render(){

        const renderVisitorTable = (VisitorList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>VisitorName</th>
                        <th>ContactNo</th>
                        <th>EmailId</th>
                        <th>Project</th>
                        <th>Purpose</th>
                        <th>VisitDate</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    {VisitorList.map((forecast, i) =>

                        <tr key={forecast.VisitorId}>
                            <td>{i + 1}</td>
                            <td>{forecast.VisitorName}</td>
                            <td>{forecast.ContactNo}</td>
                            <td>{forecast.EmailId}</td>
                            <td>{forecast.ProjectName}</td>
                            <td>{forecast.VisitorPurposeName}</td>
                            <td>{forecast.VisitDate}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.VisitorId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.VisitorId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}

                </tbody>
            </table>
        );
    }

        let VisitorList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderVisitorTable(this.state.VisitorList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Visitor Entry</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          <div className="col-md-12 col-sm-12 col-xs-12">


                              <div className="col-md-4 col-sm-12 col-xs-12 required">
                                  <label>Project</label>
                                  <ProjectDrop
                                      ProjectId={this.state.FormGroup.ProjectId}
                                      func={this.handleInputChange} />
                              </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Visitor Name</label>
                                  <input type="text" className="full-width" name="VisitorName" value={this.state.FormGroup.VisitorName} onChange={this.handleInputChange} />
                              </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Contact</label>
                                  <input type="text" className="full-width" name="ContactNo" value={this.state.FormGroup.ContactNo} onChange={this.handleInputChange} />
                              </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Email</label>
                                  <input type="text" className="full-width" name="EmailId" value={this.state.FormGroup.EmailId} onChange={this.handleInputChange} />
                              </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Visitor Purpose</label>
                                  <PurposeDrop
                                      PurposeId={this.state.FormGroup.PurposeId}
                                      func={this.handleInputChange} />
                              </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Visit Date</label>
                                  <input type="date" className="full-width" name="VisitDate" value={this.state.FormGroup.VisitDate} onChange={this.handleInputChange} />
                              </div>
                              
                              
                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                    </div>
                                </div>

                            </div>
                                  
                        </div>

                      {VisitorList}

                  </form>
                </div>
            </section>
          
    );
  }
}
