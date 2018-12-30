import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';

export class PlotView extends Component {

    constructor(props) {
        super(props);
        this.state = {
            PlotList: [],
            loading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.fillPlot = this.fillPlot.bind(this);
        //this.Redirect = this.Redirect.bind(this);

        this.fillPlot();
    }
    
    fillPlot = () => {
        axios.get(`api/Masters/FillPlotByBlock/${this.props.match.params.ID}`)
            .then(response => {
                console.log(response.data);
                this.setState({ PlotList: response.data, loading: false });
            });
    }
    
    render(){

        const renderPlotTable = (PlotList) => {
            return (
            
                    <div className="col-md-12 col-sm-12 col-xs-12">
                        {PlotList.map((forecast, i) =>
                        <div key={forecast.PlotDetailId} className="col-md-3 col-sm-3 col-xs-12 col-lg-3 p5">
                                <div className="bgRed">
                                <p>{forecast.PlotNo}</p>
                                </div>
                            </div>
                        )}
                    </div>
            );
        }

        let PlotList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderPlotTable(this.state.PlotList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Plot View</h1>
                        <div className="columns">

                          
                          {PlotList}
                           
                                  
                        </div>
                      
                  </form>
                </div>
            </section>
          
    );
  }
}
