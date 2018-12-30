import React, { Component } from 'react';
import axios from 'axios';

export default class SegmentDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            SegmentList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillSegment')
            .then(response => {
                this.setState({ SegmentList: response.data, loading: false });
            });
    }
    
    render() {

        const renderSegmentlist = (SegmentList) => {
        return (
            <select name="SegmentId" value={this.props.SegmentId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {SegmentList.map(P =>
                    <option key={P.SegmentId} value={P.SegmentId}>{P.SegmentName}</option>
                )}
            </select>
        );
    }
        
        let SegmentList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderSegmentlist(this.state.SegmentList);
        
      return (
          

          <div>
              {SegmentList}
          </div>
              
    );
  }
}
