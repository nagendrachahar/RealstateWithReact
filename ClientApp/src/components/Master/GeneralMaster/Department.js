import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';

export default class DepartmentMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            DepartmentList: [],
            loading: true,
            FormGroup: { DepartmentId: 0, DepartmentName: ''},
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //DepartmentMaster.renderDepartmentTable = DepartmentMaster.renderDepartmentTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillDepartment = this.fillDepartment.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneDepartment/${ID}`)
            .then(response => {

                const UserInput = this.state.FormGroup;
                
                UserInput["DepartmentId"] = response.data[0].DepartmentId;
                UserInput["DepartmentName"] = response.data[0].DepartmentName;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillDepartment();

            });
    }


    fillDepartment() {
        axios.get('api/Masters/FillDepartment')
            .then(response => {
                this.setState({ DepartmentList: response.data, loading: false });
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

        const Department = this.state.FormGroup

        axios.post(`api/Masters/SaveDepartment`, Department)
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

            axios.get(`api/Masters/DeleteDepartment/${ID}`)
                .then(res => {
                    this.fillDepartment();
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

        const renderDepartmentTable = (DepartmentList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Dapartment Name</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}>
                            <img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" />
                        </td>
                        <td>
                            <input type="text" name="DepartmentName" value={this.state.FormGroup.DepartmentName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {DepartmentList.map((forecast, i) =>

                        <tr key={forecast.DepartmentId}>
                            <td>{i + 1}</td>
                            <td>{forecast.DepartmentName}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.DepartmentId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.DepartmentId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

        let DepartmentList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderDepartmentTable(this.state.DepartmentList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Department</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">

                          {DepartmentList}
                                        
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
