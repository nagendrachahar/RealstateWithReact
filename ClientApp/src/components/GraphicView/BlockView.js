import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';

export class BlockView extends Component {

    constructor(props) {
        super(props);
        this.state = {
            BlockList: [],
            loading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.fillBlock = this.fillBlock.bind(this);
        //this.Redirect = this.Redirect.bind(this);

        this.fillBlock();
    }
    
    fillBlock = () => {
        axios.get(`api/Masters/FillBlockBySector/${this.props.match.params.ID}`)
        .then(response => {
            console.log(response.data);
            this.setState({ BlockList: response.data, loading: false });
        });
    }

    Redirect = (ID) => {
        console.log(ID);
        this.props.history.push(`/GraphicViewPlot/${ID}`);
    }
    
    render(){

        const renderBlockTable = (BlockList) => {
            return (
            
                    <div className="col-md-12 col-sm-12 col-xs-12">
                        {BlockList.map((forecast, i) =>
                            <div key={forecast.BlockId} onClick={this.Redirect.bind(this, forecast.BlockId)} className="col-md-3 col-sm-3 col-xs-12 col-lg-3 p5">
                                <div className="bgRed">
                                    <p>{forecast.BlockName}</p>
                                </div>
                            </div>
                        )}
                    </div>
            );
        }

        let BlockList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderBlockTable(this.state.BlockList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Block View</h1>
                        <div className="columns">

                          
                          {BlockList}
                           
                                  
                        </div>
                      
                  </form>
                </div>
            </section>
          
    );
  }
}
