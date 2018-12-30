import React, { Component } from 'react';
import $ from 'jquery';
import Series from './Pages/Series.js';

export class Configuration extends Component {

    constructor(props) {
        super(props);
        this.state = {
            SeriesLoading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

        this.hideAll = this.hideAll.bind(this);
    }

    hideAll() {
        this.setState({
            SeriesLoading: false
        });

    }
    
    
    ShowAll(show) {

        this.hideAll();

        if (show === "Series") {
            this.setState({
                SeriesLoading: true
            });
        }

    }
    
    
    render() {
        
  
      return (

          <div>

                <div className="grid_3">

                    <section className="with-margin">
                        <div className="block-border">
                            <form className="block-content form" id="simple-list-form" method="post" action="#">
                                <h1>Configuration</h1>

                                <p className="input-type-text">
                                  <input type="text" name="simple-search" id="simple-search" value="" style={{ width: "90%" }} title="Filter results" />
                                  <img src="assets/images/icons/fugue/magnifier.png" alt="Search" width="16" height="16" className="float-right" />
                                </p>

                                <ul className="simple-list with-icon" style={{ maxHeight: "350px", overflow:"auto" }}>
                                        
                                      <li><a onClick={this.ShowAll.bind(this, "Series")}>Series</a></li>

                                </ul>
                            </form>
                        </div>
                    </section>
                </div>

                <section className="grid_9">
                    <div className="childrout">
                        {this.state.SeriesLoading ? <Series /> : null}
                    </div>
                </section>

          </div>
          
    );
  }
}
