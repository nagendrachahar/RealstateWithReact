import React, { Component } from 'react';
import axios from 'axios';

export default class DepartmentDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            DepartmentList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillDepartment')
            .then(response => {
                this.setState({ DepartmentList: response.data, loading: false });
            });
        

    }
    
    
    
    render() {

        const renderDepartmentlist = (DepartmentList) => {
        return (
            <select name="DepartmentId" value={this.props.DepartmentId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {DepartmentList.map(D =>
                    <option key={D.DepartmentId} value={D.DepartmentId}>{D.DepartmentName}</option>
                )}
            </select>
        );
    }

        
        let Departmentlist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderDepartmentlist(this.state.DepartmentList);
        
      return (
              <div>
                    {Departmentlist}
              </div>
              
    );
  }
}
