import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import CombinationDrop from '../../Util/CombinationDrop.js';

export default class Series extends Component {

    constructor(props) {
        super(props);
        this.state = {
            SeriesList: [],
            CombinationList: [],
            loading: true,
            FormGroup: {
              SeriesId: 0,
              SeriesName: '',
              TableName: '',
              ColumnName: '',
              Prefix: '',
              NoOfDigit: '',
              Combination1: 0,
              Combination2: 0,
              Combination3: 0
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
        this.fillSeries = this.fillSeries.bind(this);
      
        this.FillForm(0);

    }
    
    FillForm(ID) {
      axios.get(`api/Masters/GetOneSeries/${ID}`)
            .then(response => {
                const UserInput = this.state.FormGroup;
                
                UserInput["SeriesId"] = response.data[0].SeriesId;
                UserInput["SeriesName"] = response.data[0].SeriesName;
                UserInput["TableName"] = response.data[0].TableName;
                UserInput["ColumnName"] = response.data[0].ColumnName;
                UserInput["Prefix"] = response.data[0].Prefix;
                UserInput["NoOfDigit"] = response.data[0].NoOfDigit;
                UserInput["Combination1"] = response.data[0].Combination1;
                UserInput["Combination2"] = response.data[0].Combination2;
                UserInput["Combination3"] = response.data[0].Combination3;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillSeries();
            });
    }
  
  fillSeries = () => {
    axios.get('api/Masters/FillSeries')
      .then(response => {
        this.setState({ SeriesList: response.data, loading: false });
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

        const Series = this.state.FormGroup

      axios.post(`api/Masters/SaveSeries`, Series)
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
  
    
    render() {

      const renderSeriesTable = (SeriesList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Series Name</th>
                        <th>Table</th>
                        <th>Column</th>
                        <th>Prefix</th>
                        <th>No Of Digit</th>
                        <th>Combination</th>
                        <th>Combination</th>
                        <th>Combination</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}>
                          <img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" />
                        </td>
                        <td>
                          <input type="text" name="SeriesName" value={this.state.FormGroup.SeriesName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                          <input type="text" name="TableName" value={this.state.FormGroup.TableName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                          <input type="text" name="ColumnName" value={this.state.FormGroup.ColumnName} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                          <input type="text" name="Prefix" value={this.state.FormGroup.Prefix} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                          <input type="text" name="NoOfDigit" value={this.state.FormGroup.NoOfDigit} onChange={this.handleInputChange} className="full-width" />
                        </td>
                        <td>
                            <CombinationDrop
                              CombinationName="Combination1"
                              CombinationId={this.state.FormGroup.Combination1}
                              func={this.handleInputChange} />
                        </td>

                        <td>
                          <CombinationDrop
                            CombinationName="Combination2"
                            CombinationId={this.state.FormGroup.Combination2}
                            func={this.handleInputChange} />
                        </td>
                        <td>
                          <CombinationDrop
                            CombinationName="Combination3"
                            CombinationId={this.state.FormGroup.Combination3}
                            func={this.handleInputChange} />
                        </td>
                        
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {SeriesList.map((forecast, i) =>

                <tr key={forecast.SeriesId}>
                            <td>{i + 1}</td>
                            <td>{forecast.SeriesName}</td>
                            <td>{forecast.TableName}</td>
                            <td>{forecast.ColumnName}</td>
                            <td>{forecast.Prefix}</td>
                            <td>{forecast.NoOfDigit}</td>
                            <td>{forecast.Combination1Name}</td>
                            <td>{forecast.Combination2Name}</td>
                            <td>{forecast.Combination3Name}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.SeriesId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }

      let SeriesList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
        : renderSeriesTable(this.state.SeriesList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Series</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">
                            {SeriesList}
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
