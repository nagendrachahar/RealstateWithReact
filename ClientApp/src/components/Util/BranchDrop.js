import React, { Component } from 'react';
import axios from 'axios';

export default class BranchDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            BranchList: [],
            Branchloading: true,
            Message: 'wait....!'
        };
        
        axios.get('api/Masters/GetBranch')
            .then(response => {
                this.setState({ BranchList: response.data, Branchloading: false });
            });
    }
    
    render() {

        const renderBranchlist = (BranchList) => {
        return (
            <select name="BranchId" value={this.props.BranchId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {BranchList.map(B =>
                    <option key={B.BranchID} value={B.BranchID}>{B.BranchName}</option>
                )}
            </select>
        );
    }
        
        let Branchlist = this.state.Branchloading
            ? <p><em>Loading...</em></p>
            : renderBranchlist(this.state.BranchList);
        
      return (

          <div>
              {Branchlist}
          </div>
            
          
    );
  }
}
