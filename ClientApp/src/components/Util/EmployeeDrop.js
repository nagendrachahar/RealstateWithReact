import React, { Component } from 'react';
import axios from 'axios';

export default class EmployeeDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            EmployeeList: [],
            loading: true,
            Message: 'wait....!'
        };
        
    }

    componentDidMount() {
        axios.get('api/Masters/FillEmployee')
            .then(response => {
                this.setState({ EmployeeList: response.data, loading: false });
            });
    }

    render(){

        const renderEmployeelist = (EmployeeList) => {
            return (
                <select name="EmployeeId" value={this.props.EmployeeId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {EmployeeList.map(S =>
                        <option key={S.EmployeeId} value={S.EmployeeId}>{S.EmployeeName} - {S.EmployeeCode}</option>
                    )}
                </select>
            );
        }

        let EmployeeList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderEmployeelist(this.state.EmployeeList);

        return (
            <div>
                {EmployeeList}
            </div>
                
        );
    }
}
