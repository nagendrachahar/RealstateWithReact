import React, { Component } from 'react';
import axios from 'axios';

export default class CustomerDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            CustomerList: [],
            loading: true,
            Message: 'wait....!'
        };
    }

    componentDidMount() {
        axios.get('api/Masters/FillCustomer')
            .then(response => {
                this.setState({ CustomerList: response.data, loading: false });
            });
    }

    render(){

        const renderCustomerlist = (CustomerList) => {
            return (
                <select name="CustomerId" value={this.props.CustomerId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {CustomerList.map(S =>
                        <option key={S.CustomerId} value={S.CustomerId}>{S.CustomerName} - {S.CustomerCode}</option>
                    )}
                </select>
            );
        }

        let CustomerList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderCustomerlist(this.state.CustomerList);

        return (
            <div>
                {CustomerList}
            </div>
                
        );
    }
}
