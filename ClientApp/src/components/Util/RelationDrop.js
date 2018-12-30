import React, { Component } from 'react';
import axios from 'axios';

export default class RelationDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            RelationList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/GetRelation')
            .then(response => {
                this.setState({ RelationList: response.data, loading: false });
            });
        

    }
    
    render() {

        const renderRelationlist = (RelationList) => {
            
            return (
                <select name={this.props.RelationName} value={this.props.RelationId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {RelationList.map(S =>
                        <option key={S.relationId} value={S.relationId}>{S.relationName}</option>
                    )}
                </select>
            );
        }

        let Relationlist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderRelationlist(this.state.RelationList);

        return (

            //<div className="col-md-4 col-sm-4 col-xs-12 col-lg-4">
            //<label>{this.props.RelationName}</label>
            <div>
                {Relationlist}
            </div>
                
            //</div>

        );
    }
}
