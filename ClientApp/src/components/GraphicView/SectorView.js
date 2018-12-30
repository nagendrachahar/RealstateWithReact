import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';

export class SectorView extends Component {

    constructor(props) {
        super(props);
        this.state = {
            SectorList: [],
            loading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.fillSector = this.fillSector.bind(this);
        //this.Redirect = this.Redirect.bind(this);

        this.fillSector();
    }
    
    fillSector = () => {
        axios.get(`api/Masters/FillSectorByProject/${this.props.match.params.ID}`)
            .then(response => {
                console.log(response.data);
                this.setState({ SectorList: response.data, loading: false });
            });
    }

    Redirect = (ID) => {
        console.log(ID);
        this.props.history.push(`/GraphicViewBlock/${ID}`);
    }
    
    
    
    render(){

        const renderSectorTable = (SectorList) => {
        return (
            
                <div className="col-md-12 col-sm-12 col-xs-12">
                {SectorList.map((forecast, i) =>

                    <div key={forecast.SectorId} onClick={this.Redirect.bind(this, forecast.SectorId)} className="col-md-3 col-sm-3 col-xs-12 col-lg-3 p5">
                        <div className="bgRed">
                            <p>{forecast.SectorName}</p>
                        </div>
                    </div>
                    )}
                </div>
        );
    }

        let SectorList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderSectorTable(this.state.SectorList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Sector View</h1>
                      

                        <div className="columns">

                          
                          {SectorList}
                           
                                  
                        </div>
                      
                  </form>
                </div>
            </section>
          
    );
  }
}
