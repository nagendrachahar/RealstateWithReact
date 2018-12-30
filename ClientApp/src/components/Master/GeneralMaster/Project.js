import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';

export default class ProjectMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            ProjectList: [],
            loading: true,
            FormGroup: { ProjectId: 0, ProjectName: '', Address: '', Area: ''},
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //ProjectMaster.renderProjectTable = ProjectMaster.renderProjectTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillProject = this.fillProject.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneProject/${ID}`)
            .then(response => {
                
                const UserInput = this.state.FormGroup;
                
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["ProjectName"] = response.data[0].ProjectName;
                UserInput["Address"] = response.data[0].ProjectAddress;
                UserInput["Area"] = response.data[0].Area;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillProject();

            });
    }


    fillProject() {
        axios.get('api/Masters/FillProject')
            .then(response => {
                this.setState({ ProjectList: response.data, loading: false });
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

        const Project = this.state.FormGroup

        axios.post(`api/Masters/SaveProject`, Project)
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

            axios.get(`api/Masters/DeleteProject/${ID}`)
                .then(res => {
                    this.fillProject();
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

        const renderProjectTable = (ProjectList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Project Name</th>
                        <th>Address</th>
                        <th>Area</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}>
                            <img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" />
                        </td>
                        <td>
                            <input type="text" name="ProjectName" value={this.state.FormGroup.ProjectName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                            <input type="text" name="Address" value={this.state.FormGroup.Address} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                            <input type="text" name="Area" value={this.state.FormGroup.Area} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {ProjectList.map((forecast, i) =>

                        <tr key={forecast.ProjectId}>
                            <td>{i + 1}</td>
                            <td>{forecast.ProjectName}</td>
                            <td>{forecast.ProjectAddress}</td>
                            <td>{forecast.Area}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.ProjectId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.ProjectId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}

                </tbody>
            </table>

        );
    }

        let ProjectList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderProjectTable(this.state.ProjectList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Project</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">

                          {ProjectList}
                                        
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
