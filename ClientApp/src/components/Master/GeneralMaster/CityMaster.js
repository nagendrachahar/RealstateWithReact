import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import StateDrop from '../../Util/StateDrop.js';

export default class CityMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            StateList: [],
            loading: true,
            FormGroup: { CityId: 0, CityName: '', StateId: 0 },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //CityMaster.renderCityTable = CityMaster.renderCityTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillCity = this.fillCity.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneCity/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["CityId"] = response.data[0].CityId;
                UserInput["CityName"] = response.data[0].CityName;
                UserInput["StateId"] = response.data[0].StateId;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillCity();
            });
    }


    fillCity() {
        axios.get('api/Masters/GetCity')
            .then(response => {
                this.setState({ CityList: response.data, loading: false });
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

        const City = this.state.FormGroup

        axios.post(`api/Masters/SaveCity`, City)
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

            axios.get(`api/Masters/DeleteCity/${ID}`)
                .then(res => {
                    this.fillCity();
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

        const renderCityTable = (CityList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>State</th>
                        <th>City</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}><img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" /></td>
                        <td>
                            <StateDrop StateId={this.state.FormGroup.StateId} func={this.handleInputChange} />
                        </td>
                        <td><input type="text" name="CityName" value={this.state.FormGroup.CityName} onChange={this.handleInputChange} className="full-width" /></td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {CityList.map((forecast, i) =>

                        <tr key={forecast.cityId}>
                            <td>{i + 1}</td>
                            <td>{forecast.stateName}</td>
                            <td>{forecast.cityName}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.cityId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.cityId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

        let CityList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderCityTable(this.state.CityList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="Cityform" method="post" action="#">
                      <h1>City</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">
                            {CityList}
                                  
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
