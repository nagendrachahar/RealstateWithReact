import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import ProjectDrop from '../../Util/ProjectDrop';
import SectorDrop from '../../Util/SectorDrop';
import BlockDrop from '../../Util/BlockDrop';
import SegmentDrop from '../../Util/SegmentDrop';
import PlotTypeDrop from '../../Util/PlotTypeDrop';
import PlotDrop from '../../Util/PlotDrop';

export default class RateMaster extends Component {

    constructor(props) {
        super(props); 
        this.state = {
            RateList: [],
            loading: true,
            FormGroup: {
                RateId: 0,
                ProjectId: 0,
                SectorId: 0,
                BlockId: 0,
                SegmentId: 0,
                PlotTypeId: 0,
                PlotId: 0,
                Rate: '',
                IdCollection: ''
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
        this.fillRateDetail = this.fillRateDetail.bind(this);
        this.Confirm = this.Confirm.bind(this);
        this.FillForm(0);

    }
    
    FillForm = (ID) => {
        axios.get(`api/Masters/GetOneRateMaster/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["RateId"] = response.data[0].RateId;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["SectorId"] = response.data[0].SectorId;
                UserInput["BlockId"] = response.data[0].BlockId;
                UserInput["SegmentId"] = response.data[0].SegmentId;
                UserInput["PlotTypeId"] = response.data[0].PlotTypeId;
                UserInput["PlotId"] = response.data[0].PlotId;
                UserInput["Rate"] = response.data[0].Rate;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillRateDetail();
            });
    }

    fillRateDetail = () => {
        axios.get('api/Masters/FillRateDetail')
            .then(response => {
                console.log(response.data);
                this.setState({ RateList: response.data, loading: false });
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

        const Rate = this.state.FormGroup

        axios.post(`api/Masters/vrifyRateDetail`, Rate)
            .then(res => {
                this.setState({
                    Message: res.data[0].Message
                });

                if (res.data[0].MessageType === 1) {

                    axios.post(`api/Masters/SaveRateDetail`, Rate)
                        .then(res => {
                            this.setState({
                                Message: res.data[0].Message
                            });
                            this.FillForm(0);
                        })
                }
                else {
                    const UserInput = this.state.FormGroup;

                    UserInput['IdCollection'] = res.data[0].AllPlotId;

                    this.setState({
                        FormGroup: UserInput
                    });

                    this.Confirm(res.data[0].Message, res.data[0].NewPlotId);
                }
            })

        $(document).ready(function () {
            $(".message").show();
        });
    }


    Confirm = (message, NewPlotId) => {

        const Rate = this.state.FormGroup;

        var body = document.getElementsByTagName('body')[0];
        var overlay = document.createElement('div');
        overlay.className = 'myConfirm';
        var box = document.createElement('div');
        var boxchild = document.createElement('div');
        var p = document.createElement('p');
        p.appendChild(document.createTextNode("Rate for Plot No. " + message + " Already defined, Do you want to Update"));
        boxchild.appendChild(p);
        var yesButton = document.createElement('button');
        var noButton = document.createElement('button');
        yesButton.appendChild(document.createTextNode('Yes'));
        yesButton.addEventListener('click', () => {

            axios.post(`api/Masters/SaveRateDetail`, Rate)                .then(res => {                    this.setState({
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
    
    
    render() {

        const renderRateTable = (RateList) => {
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

                    {RateList.map((forecast, i) =>

                        <tr key={forecast.RateId}>
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
                                <a onClick={this.FillForm.bind(this, forecast.RateId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

        let RateList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderRateTable(this.state.RateList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Rate Master</h1>

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
                                      <label>Plot</label>
                                      <PlotDrop BlockId={this.state.FormGroup.BlockId} PlotId={this.state.FormGroup.PlotId} func={this.handleInputChange} />             
                                </div>
                              
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Rate</label>
                                  <input type="text" className="full-width" name="Rate" value={this.state.FormGroup.Rate} onChange={this.handleInputChange} />
                                </div>
                              

                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                  </div>
                                </div>

                            </div>
                                  
                        </div>

                      {RateList}

                  </form>
                </div>
            </section>
          
    );
  }
}
