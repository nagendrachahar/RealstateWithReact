import React, { Component } from 'react';
import axios from 'axios';

export default class DesignationDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            DesignationList: [],
            loading: true,
            Message: 'wait....!'
        };

        this.fillDesignation = this.fillDesignation.bind(this);
        this.fillDesignation(this.props.DepartmentType);
        
    }


    componentWillReceiveProps(nextProps) {
        if (nextProps.DepartmentType !== this.props.DepartmentType) {
          this.fillDesignation(nextProps.DepartmentType);
        }
    }


    fillDesignation = (type) => {

      if (type === "1") {

          axios.get('api/Masters/FillDesignation')
            .then(response => {
              this.setState({ DesignationList: response.data, loading: false });
            });

        }
        else {
          axios.get('api/Masters/FillFARank')
            .then(response => {
              this.setState({ DesignationList: response.data, loading: false });
            });
        }

    }
    
    render() {

      const renderDesignationlist = (DesignationList) => {

        if (this.props.DepartmentType === "1") {
          return (
            <select name="DesignationId" value={this.props.DesignationId} onChange={this.props.func} className="full-width">
              <option value="0">Select</option>
              {DesignationList.map(D =>
                <option key={D.DesignationId} value={D.DesignationId}>{D.DesignationName}</option>
              )}
            </select>
          );
        }
        else {
            return (
              <select name="DesignationId" value={this.props.DesignationId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {DesignationList.map(D =>
                  <option key={D.RankID} value={D.RankID}>{D.RankName}</option>
                )}
              </select>
            );
        }
          
    }
        
        let Designationlist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderDesignationlist(this.state.DesignationList);
        
      return (
          
          <div>
              {Designationlist}
          </div>
              
    );
  }
}

DesignationDrop.defaultProps = {
  DepartmentType: "1",
  DesignationId: "0"
}
