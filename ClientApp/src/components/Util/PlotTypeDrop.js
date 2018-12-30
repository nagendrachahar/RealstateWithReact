import React, { Component } from 'react';
import axios from 'axios';

export default class PlotTypeDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            PlotTypeList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillPlotType')
            .then(response => {
                this.setState({ PlotTypeList: response.data, loading: false });
            });
    }
    
    render() {

        const renderPlotTypelist = (PlotTypeList) => {
        return (
            <select name="PlotTypeId" value={this.props.PlotTypeId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {PlotTypeList.map(P =>
                    <option key={P.PlottypeId} value={P.PlottypeId}>{P.PlottypeName}</option>
                )}
            </select>
        );
    }
        
        let PlotTypeList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderPlotTypelist(this.state.PlotTypeList);
        
      return (

          <div>
              {PlotTypeList}
          </div>
              
          
    );
  }
}
