import React, { Component } from 'react';
import axios from 'axios';

export default class CombinationDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            CombinationList: [],
            loading: true,
            Message: 'wait....!'
        };

      axios.get('api/Masters/GetCombination')
            .then(response => {
              this.setState({ CombinationList: response.data, loading: false });
            });
        

    }
    
    render() {

      const renderCombinationlist = (CombinationList) => {
            return (
                <select name={this.props.CombinationName} value={this.props.CombinationId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {CombinationList.map(S =>
                        <option key={S.seriesCombinationId} value={S.seriesCombinationId}>{S.seriesCombinationName}</option>
                    )}
                </select>
            );
      }

      let CombinationList = this.state.loading
            ? <p><em>Loading...</em></p>
        : renderCombinationlist(this.state.CombinationList);

        return (
          
            <div>
                {CombinationList}
            </div>
                
        );
    }
}
