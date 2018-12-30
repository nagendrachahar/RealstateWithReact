import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';

export default class EMIPlan extends Component {

    constructor(props) {
        super(props);
        this.state = {
            PlanList: [],
            loading: true,
            FormGroup: { PlanId: 0, PlanCode: '', PlanValue: ''},
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //EMIPlan.renderEMIPlanTable = EMIPlan.renderEMIPlanTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillPlan = this.fillPlan.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneEMIPlan/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["PlanId"] = response.data[0].PlanId;
                UserInput["PlanCode"] = response.data[0].PlanCode;
                UserInput["PlanValue"] = response.data[0].PlanValue;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillPlan();

            });
    }
    
    fillPlan() {
        axios.get('api/Masters/FillEMIPlan')
            .then(response => {
                this.setState({ PlanList: response.data, loading: false });
            });
    }

    handleInputChange(event) {
        debugger;
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

        const EMIPlan = this.state.FormGroup

        axios.post(`api/Masters/SaveEMIPlan`, EMIPlan)
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

            axios.get(`api/Masters/DeleteEMIPlan/${ID}`)
                .then(res => {
                    this.fillPlan();
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

        const renderEMIPlanTable = (PlanList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Plan Code</th>
                        <th>Plan Value</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}>
                            <img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" />
                        </td>
                        <td>
                            <input type="text" name="PlanCode" value={this.state.FormGroup.PlanCode} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                            <input type="text" name="PlanValue" value={this.state.FormGroup.PlanValue} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {PlanList.map((forecast, i) =>

                        <tr key={forecast.PlanId}>
                            <td>{i + 1}</td>
                            <td>{forecast.PlanCode}</td>
                            <td>{forecast.PlanValue}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.PlanId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.PlanId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

        let PlanList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderEMIPlanTable(this.state.PlanList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                        <h1>EMI Plan</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          {PlanList}
                                        
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
