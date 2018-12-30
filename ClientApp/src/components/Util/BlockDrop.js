import React, { Component } from 'react';
import axios from 'axios';

export default class BlockDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            BlockList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        this.fillBlock = this.fillBlock.bind(this);

        this.fillBlock(this.props.ProjectId);
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.SectorId !== this.props.SectorId) {
            //Perform some operation
            this.fillBlock(nextProps.SectorId);
        }
    }

    fillBlock = (id) => {

        if (id !== "0") {

            axios.get(`api/Masters/FillBlockBySector/${id}`)
                .then(response => {
                    this.setState({ BlockList: response.data, loading: false });
                });
        }
        else {

            axios.get(`api/Masters/FillBlock`)
                .then(response => {
                    this.setState({ BlockList: response.data, loading: false });
                });
        }
       
    }
    
    render() {

        const renderBlocklist = (BlockList) => {
        return (
            <select name="BlockId" value={this.props.BlockId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {BlockList.map(C =>
                    <option key={C.BlockId} value={C.BlockId}>{C.BlockName}</option>
                )}
            </select>
        );
    }


        let BlockList = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderBlocklist(this.state.BlockList);

        return (

            <div>
                {BlockList}
            </div>
                

        );
    }
}
