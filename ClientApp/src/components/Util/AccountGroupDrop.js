import React, { Component } from 'react';
import axios from 'axios';

export default class AccountGroupDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            AccountGroupList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/FillAccountGroup')
            .then(response => {
                this.setState({ AccountGroupList: response.data, loading: false });
            });
        

    }
    
    render() {

        const renderGrouplist = (AccountGroupList) => {
            
            return (
                <select name="GroupId" value={this.props.GroupId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {AccountGroupList.map(S =>
                        <option key={S.GroupId} value={S.GroupId}>{S.GroupName}</option>
                    )}
                </select>
            );
        }

        let AccountGroupList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderGrouplist(this.state.AccountGroupList);

        return (

            //<div className="col-md-4 col-sm-4 col-xs-12 col-lg-4">
            //<label>{this.props.RelationName}</label>
            <div>
                {AccountGroupList}
            </div>
                
            //</div>

        );
    }
}
