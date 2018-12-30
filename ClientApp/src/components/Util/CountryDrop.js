import React, { Component } from 'react';
import axios from 'axios';

export default class CountryDrop extends Component {

    constructor(props) {

        super(props);
        this.state = {
            CountryList: [],
            loading: true,
            Message: 'wait....!'
        };

        axios.get('api/Masters/FillCountry')
            .then(response => {
                this.setState({ CountryList: response.data, loading: false });
            });
        
    }

    render() {

        const renderCountrylist = (CountryList) => {
            return (
                <select name="CountryId" value={this.props.CountryId} onChange={this.props.func} className="full-width">
                    <option value="0">Select</option>
                    {CountryList.map(S =>
                        <option key={S.countryId} value={S.countryId}>{S.countryName}</option>
                    )}
                </select>
            );
        }

        let Countrylist = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderCountrylist(this.state.CountryList);

        return (
            
            <div>
                {Countrylist}
            </div>
                

        );
    }
}
