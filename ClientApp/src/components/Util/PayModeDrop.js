import React, { Component } from 'react';
import axios from 'axios';

export default class PayModeDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            PayModeList: [],
            loading: true,
            Message: 'wait....!'
        };
    }

    componentDidMount() {
        axios.get('api/Masters/FillPayMode')
            .then(response => {
                this.setState({ PayModeList: response.data, loading: false });
            });
    }

    render(){

        const renderPaymodelist = (PayModeList) => {
            return (
                <select name="PayMode" value={this.props.PayMode} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {PayModeList.map(S =>
                        <option key={S.PayModeId} value={S.PayModeId}>{S.PayModeName}</option>
                    )}
                </select>
            );
        }

        let PayModeList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderPaymodelist(this.state.PayModeList);

        return (
            <div>
                {PayModeList}
            </div>
                
        );
    }
}
