import React, { Component } from 'react';
import axios from 'axios';

export default class UserDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            UserList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/FillUsers')
            .then(response => {
                this.setState({ UserList: response.data, loading: false });
            });
    }

    render(){

        const renderUserlist = (UserList) => {
            return (
                <select name="UserId" value={this.props.UserId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {UserList.map(S =>
                        <option key={S.UserId} value={S.UserId}>{S.UserName} - {S.UserCode}</option>
                    )}
                </select>
            );
        }

        let UserList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderUserlist(this.state.UserList);

        return (
            <div>
                {UserList}
            </div>
                
        );
    }
}
