import React, { Component } from 'react';
import axios from 'axios';

export default class ProjectDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            ProjectList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/FillProject')
            .then(response => {
                this.setState({ ProjectList: response.data, loading: false });
            });
    }
    
    render() {

        const renderProjectlist = (ProjectList) => {
        return (
            <select name="ProjectId" value={this.props.ProjectId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {ProjectList.map(P =>
                    <option key={P.ProjectId} value={P.ProjectId}>{P.ProjectName}</option>
                )}
            </select>
        );
    }
        
        let ProjectList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderProjectlist(this.state.ProjectList);
        
      return (
          
          <div>
              {ProjectList}
          </div>
          
          
    );
  }
}
