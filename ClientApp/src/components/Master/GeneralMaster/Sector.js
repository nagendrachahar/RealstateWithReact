import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import ProjectDrop from '../../Util/ProjectDrop.js';

export default class SectorMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            SectorList: [],
            loading: true,
            FormGroup: { SectorId: 0, ProjectId: 0, SectorName: ''},
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //SectorMaster.renderSectorTable = SectorMaster.renderSectorTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillSector = this.fillSector.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneSector/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["SectorId"] = response.data[0].SectorId;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["SectorName"] = response.data[0].SectorName;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillSector();
            });
    }


    fillSector() {
        axios.get('api/Masters/FillSector')
            .then(response => {
                this.setState({ SectorList: response.data, loading: false });
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

        const Sector = this.state.FormGroup

        axios.post(`api/Masters/SaveSector`, Sector)
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

            axios.get(`api/Masters/DeleteSector/${ID}`)
                .then(res => {
                    this.fillSector();
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

       const renderSectorTable = (SectorList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Project Name</th>
                        <th>Sector Name</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}><img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" /></td>
                        <td>
                            <ProjectDrop ProjectId={this.state.FormGroup.ProjectId} func={this.handleInputChange} />
                        </td>
                        <td>
                            <input type="text" name="SectorName" value={this.state.FormGroup.SectorName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {SectorList.map((forecast, i) =>

                        <tr key={forecast.SectorId}>
                            <td>{i + 1}</td>
                            <td>{forecast.ProjectName}</td>
                            <td>{forecast.SectorName}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.SectorId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.SectorId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

        let SectorList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderSectorTable(this.state.SectorList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Sector</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">
                            {SectorList}
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
