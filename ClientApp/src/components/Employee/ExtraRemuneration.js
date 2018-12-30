import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import DepartmentDrop from '../Util/DepartmentDrop.js';
import DesignationDrop from '../Util/DesignationDrop.js';

export class ExtraRemuneration extends Component {

    constructor(props) {
        super(props);
        this.state = {
            RemunerationList: [],
            loading: true,
            FormGroup: {
                RemunerationId: 0,
                DepartmentId: 0,
                DesignationId: 0,
                Value: 0
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
        this.fillRemuneration = this.fillRemuneration.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneExtraRemuneration/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["RemunerationId"] = response.data[0].RemunerationId;
                UserInput["DepartmentId"] = response.data[0].DepartmentId;
                UserInput["DesignationId"] = response.data[0].DesignationId;
                UserInput["Value"] = response.data[0].Value;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillRemuneration();
            });
    }


    fillRemuneration() {
        axios.get('api/Masters/FillExtraRemuneration')
            .then(response => {
                console.log(response.data);
                this.setState({ RemunerationList: response.data, loading: false });
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

        const ExtraRemuneration = this.state.FormGroup

        axios.post(`api/Masters/SaveExtraRemuneration`, ExtraRemuneration)
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

            axios.get(`api/Masters/DeleteExtraRemuneration/${ID}`)
                .then(res => {
                    this.fillRemuneration();
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

        const renderRemunerationTable = (RemunerationList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th scope="col">Department</th>
                        <th scope="col">Designation</th>
                        <th scope="col">Value</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    {RemunerationList.map((forecast, i) =>

                        <tr key={forecast.RemunerationId}>
                            <td>{i + 1}</td>
                            <td>{forecast.DepartmentName}</td>
                            <td>{forecast.DesignationName}</td>
                            <td>{forecast.Value}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.RemunerationId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.RemunerationId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}

                </tbody>
            </table>
        );
    }

        let RemunerationList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderRemunerationTable(this.state.RemunerationList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="Cityform" method="post" action="#">
                      <h1>Remuneration Structure</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          <div className="col-md-12 col-sm-12 col-xs-12">

                                <div className="col-md-4 col-sm-12 col-xs-12 required">
                                  <label>Department</label>
                                  <DepartmentDrop
                                      DepartmentId={this.state.FormGroup.DepartmentId}
                                      func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label >Designation</label>
                                    <DesignationDrop
                                      DepartmentType={this.state.FormGroup.DepartmentId}
                                      DesignationId={this.state.FormGroup.DesignationId}
                                      func={this.handleInputChange} />
                                </div>
                              

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Value</label>
                                  <input type="text" className="full-width" name="Value" value={this.state.FormGroup.RemunerationPer} onChange={this.handleInputChange} />
                                </div>
                              
                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                    </div>
                                </div>

                            </div>
                                  
                        </div>

                      {RemunerationList}

                  </form>
                </div>
            </section>
          
    );
  }
}
