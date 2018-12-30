import React, { Component } from 'react';
import axios from 'axios';

export default class AccountLedgerDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            LedgerList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/FillAccountLedger')
            .then(response => {
                this.setState({ LedgerList: response.data, loading: false });
            });
        

    }
    
    render() {

        const renderLedgerlist = (LedgerList) => {
            
            return (
                <select name={this.props.LedgerName} value={this.props.LedgerId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {LedgerList.map(S =>
                        <option key={S.LedgerId} value={S.LedgerId}>{S.LedgerName}</option>
                    )}
                </select>
            );
        }

        let LedgerList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderLedgerlist(this.state.LedgerList);

        return (
            
            <div>
                {LedgerList}
            </div>
                

        );
    }
}
