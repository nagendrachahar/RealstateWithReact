import React, { Component } from 'react';
import axios from 'axios';

export default class PurposeDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            PurposeList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillVisitorPurpose')
            .then(response => {
                this.setState({ PurposeList: response.data, loading: false });
            });
    }
    
    render() {

        const renderPurposelist = (PurposeList) => {
        return (
            <select name="PurposeId" value={this.props.PurposeId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {PurposeList.map(P =>
                    <option key={P.VisitorPurposeId} value={P.VisitorPurposeId}>{P.VisitorPurposeName}</option>
                )}
            </select>
        );
    }
        
        let PurposeList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderPurposelist(this.state.PurposeList);
        
      return (
          
          <div>
              {PurposeList}
          </div>
          
          
    );
  }
}
