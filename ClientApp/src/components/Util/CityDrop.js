import React, { Component } from 'react';
import axios from 'axios';

export default class CityDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            CityList: [],
            loading: true,
            Message: 'wait....!'
        };
        
        this.fillCity = this.fillCity.bind(this);

        this.fillCity(this.props.StateId);
    }

    componentWillReceiveProps(nextProps) {
        if (nextProps.StateId !== this.props.StateId) {
            //Perform some operation
            this.fillCity(nextProps.StateId);
        }
    }

    fillCity(id) {

        axios.get(`api/Masters/GetCityOfState/${id}`)
            .then(response => {
                this.setState({ CityList: response.data, loading: false });
            });

    }

   

    render() {

        const renderCitylist = (CityList) => {
        return (
            <select name="CityId" value={this.props.CityId} onChange={this.props.func} className="full-width">
                <option value="0">Select</option>
                {CityList.map(C =>
                    <option key={C.cityId} value={C.cityId}>{C.cityName}</option>
                )}
            </select>
        );
    }


        let Citylist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderCitylist(this.state.CityList);

        return (

            //<div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
            //    <label>City</label>
            <div>
                {Citylist}
            </div>
                
            //</div>

        );
    }
}
