import React, { Component } from 'react';
import axios from 'axios';

export default class StateDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            StateList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/FillState')
            .then(response => {
                this.setState({ StateList: response.data, loading: false });
            });
    }

    render(){

        const renderStatelist = (StateList) => {
            return (
                <select name="StateId" value={this.props.StateId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {StateList.map(S =>
                        <option key={S.stateId} value={S.stateId}>{S.stateName}</option>
                    )}
                </select>
            );
        }

        let Statelist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderStatelist(this.state.StateList);

        return (
            <div>
                {Statelist}
            </div>
                
        );
    }
}
