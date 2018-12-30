import React, { Component } from 'react';
import axios from 'axios';

export default class EMIPlanDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            PlanList: [],
            loading: true,
            Message: 'wait....!'
        };
    }

    componentDidMount() {
        axios.get('api/Masters/FillEMIPlan')
            .then(response => {
                this.setState({ PlanList: response.data, loading: false });
            });
    }

    render(){

        const renderPlanlist = (PlanList) => {
            return (
                <select name="EMIPlanId" value={this.props.EMIPlanId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {PlanList.map(S =>
                        <option key={S.PlanId} value={S.PlanId}>{S.PlanCode}</option>
                    )}
                </select>
            );
        }

        let PlanList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderPlanlist(this.state.PlanList);

        return (
            <div>
                {PlanList}
            </div>
                
        );
    }
}
