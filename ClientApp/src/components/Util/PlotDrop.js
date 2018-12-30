import React, { Component } from 'react';
//import PropTypes from 'prop-types';
import axios from 'axios';

class PlotDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            PlotList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        this.fillPlot = this.fillPlot.bind(this);

      this.fillPlot(this.props.BlockId, this.props.isBooked, this.props.isUpdate);
    }

    componentWillReceiveProps(nextProps) {
      if (nextProps.BlockId !== this.props.BlockId || nextProps.isUpdate !== this.props.isUpdate) {
            //Perform some operation
            this.fillPlot(nextProps.BlockId, nextProps.isBooked, nextProps.isUpdate);
        }
    }

  fillPlot = (BlockId, isBooked, isUpdate) => {
    debugger;
    if (isUpdate === "0") {
      if (BlockId !== "0" && isBooked === "ALL") {

        axios.get(`api/Masters/FillPlotByBlock/${BlockId}`)
          .then(response => {
            this.setState({ PlotList: response.data, loading: false });
          });
      }
      else if (BlockId !== "0" && isBooked !== "ALL") {

        axios.get(`api/Masters/FillActivePlotByBlock/${BlockId}/${isBooked}`)
          .then(response => {
            this.setState({ PlotList: response.data, loading: false });
          });
      }
      else {
        //return all plot List
        axios.get(`api/Masters/FillPlotDetail`)
          .then(response => {
            this.setState({ PlotList: response.data, loading: false });
          });
      }
    }
    else {
      axios.get(`api/Masters/FillActivePlotAndSelf/${BlockId}/${isBooked}/${isUpdate}`)
        .then(response => {
          this.setState({ PlotList: response.data, loading: false });
        });
    }
    
  }
    
    render() {

        const renderPlotlist = (PlotList) => {
            return (
                <select name="PlotId" value={this.props.PlotId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {PlotList.map(C =>
                        <option key={C.PlotDetailId} value={C.PlotDetailId}>{C.PlotNo}</option>
                    )}
                </select>
            );
        }

        let PlotList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderPlotlist(this.state.PlotList);

        return (
            <div>
                {PlotList}
            </div>
        );
    }
}

//PlotDrop.propTypes = {
//  isBooked: PropTypes.string,
//  BlockId: PropTypes.string
//};

PlotDrop.defaultProps = {
  BlockId: "0",
  isBooked: "ALL",
  isUpdate: "0"
}

export default PlotDrop;
