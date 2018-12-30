import React, { Component } from 'react';
import axios from 'axios';

export default class RateDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            RateList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillRate')
        .then(response => {
            this.setState({ RateList: response.data, loading: false });
        });
        
    }
    
    render() {

        const renderRatelist = (RateList) => {
        return (
            <select name="Rate" value={this.props.Rate} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {RateList.map(D =>
                    <option key={D.Rate} value={D.Rate}>{D.Rate}</option>
                )}
            </select>
        );
    }
        
    let RateList = this.state.loading
        ? <p><em>Loading...</em></p>
        : renderRatelist(this.state.RateList);
        
          return (
                  <div>
                    {RateList}
                  </div>
          );
  }
}
