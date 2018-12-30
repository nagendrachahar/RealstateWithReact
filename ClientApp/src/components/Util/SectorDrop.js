import React, { Component } from 'react';
import axios from 'axios';

export default class SectorDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            SectorList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        this.fillSector = this.fillSector.bind(this);

        this.fillSector(this.props.ProjectId);
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.ProjectId !== this.props.ProjectId) {
            //Perform some operation
            this.fillSector(nextProps.ProjectId);
        }
    }

    fillSector(id) {

        if (id !== "0") {

            axios.get(`api/Masters/FillSectorByProject/${id}`)
                .then(response => {
                    this.setState({ SectorList: response.data, loading: false });
                });
        }
        else {

            axios.get(`api/Masters/FillSector`)
                .then(response => {
                    this.setState({ SectorList: response.data, loading: false });
                });
        }
       
    }
    
    render() {

        const renderSectorlist = (SectorList) => {
        return (
            <select name="SectorId" value={this.props.SectorId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {SectorList.map(C =>
                    <option key={C.SectorId} value={C.SectorId}>{C.SectorName}</option>
                )}
            </select>
        );
    }


        let SectorList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderSectorlist(this.state.SectorList);

        return (

            <div>
                {SectorList}
            </div>
                

        );
    }
}
