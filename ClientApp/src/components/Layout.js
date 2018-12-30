import React, { Component } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import './../assets/css/mini3537.css';
import './../assets/css/bootstrap.css';
import './../assets/css/Custom.css';

export class Layout extends Component {
    displayName = Layout.name

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, UserName: '', FinancialYearName: '', CompanyName: '', BranchName: '' };


        axios.get('api/Masters/GetSession')
            .then(response => {
                if (response.data[0].IsUser === 1) {

                    this.setState({
                        UserName: response.data[0].UserName,
                        FinancialYearName: response.data[0].FinancialYearName,
                        CompanyName: response.data[0].CompanyName,
                        BranchName: response.data[0].BranchName,

                    });

                    this.fillMenu();
                }
                else {
                    window.location.href = "Login";
                }
                
            });

        this.fillMenu = this.fillMenu.bind(this);
        
    }

    fillMenu() {
        axios.get('api/Masters/FillMenu')
            .then(response => {
                this.setState({ forecasts: response.data, loading: false });
            });

    }

    static getChild(id, MenuList) {
        return MenuList.filter(M => M.ParentID === id);
    }

    static renderForecastsTable(forecasts, childMenu) {
        return (
           <ul>
                {forecasts.map(M =>
                    <li className="with-menu" key={M.MenuId}>
                        <a title="My settings">{ M.MenuName }</a>

                        <div className="menu">
                            <img src="assets/images/menu-open-arrow.png" alt="Menu" width="16" height="16" />
                            <ul>
                                {Layout.getChild(M.MenuId, childMenu).map(C =>
                                    <li className="icon_down" key={C.MenuId}><Link to={C.Url}>{C.MenuName}</Link></li>
                                )}
                            </ul>
                        </div>
                    </li>
                     
                 )}
            </ul>
        );
    }
    
    render() {
        const navLi = {
            background: 'none'
        };
        const navH = {
            color: 'white',
            fontFamily: 'Imprint MT Shadow',
            fontSize: '50px'
        };
        const subnavA = {
            height: '10px',
            marginTop: '3px',
            marginRight: '0px',
            marginBottom: '0px',
            marginLeft: '4px'
        };
        const subnavForm = {
            height: '22px',
            marginTop: '3px',
            marginRight: '0px',
            marginBottom: '0px',
            marginLeft: '4px',
            color: 'black'
        };

        var parentMenu = this.state.forecasts.filter(M => M.ParentID === 0);

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Layout.renderForecastsTable(parentMenu, this.state.forecasts);

       
        return (

            <div>
                <header>
                    <div className="container_12">
                        <p id="skin-name"><small>Financial<br />Year</small> <strong>{this.state.FinancialYearName}</strong></p>
                        <div className="server-info">Logged As : <strong> {this.state.UserName} </strong></div>
                        <div className="server-info">Branch : <strong> {this.state.BranchName} </strong></div>
                        <div className="server-info">Balance : <strong>1000 Cr.</strong></div>
                    </div>
                </header>

                <nav id="main-nav">
                    <ul className="container_12">
                        <li className="current" style={navLi}>
                            <h1 style={navH}>{this.state.CompanyName}</h1>
                            {contents}
                        </li>
                    </ul>
                </nav>

                <div id="sub-nav">
                    <div className="container_12 main_search" >
                        <a title="Help" className="nav-button" style={subnavA}>
                        <b styles={{ height: '25px' }}>FIND</b></a>
                        <form id="search-form" name="search-form" method="post" action="">
                            <input type="text" name="s" id="s" value="" title="Search admin..." style={subnavForm} autoComplete="off" />
                        </form>
                    </div>
                </div>

                <div id="status-bar">
                    <div className="container_12">

                        <ul id="status-infos">
                            <li>
                                <a href="/Change Password" className="button blue" title="Change Password"><span className="small"><i className="fa fa-shutdown"></i>Change Password</span></a>
                            </li>
                            <li>
                                <a href="Login/Logout" className="button red" title="Logout"><span className="small"><i className="fa fa-shutdown"></i>LOGOUT</span></a>
                            </li>
                        </ul>
                        <ul id="breadcrumb">
                            <li><a title="Home">Home</a></li>
                            <li><a>Dashboard</a></li>
                        </ul>

                    </div>
                </div>

                <div id="header-shadow"></div>

                <article className="container_12">

                    {this.props.children}

                    <div className="clear"></div>

                </article>


                <footer>

                    <div className="float-left">
                        <a className="button">&copy; 2018</a>
                        <a href="nbfc.indiansoftking.com" className="button">&reg; JJMV</a>
                    </div>

                    <div className="float-right">
                        <a href="#top" className="button"><img src={require('../assets/images/icons/fugue/navigation-090.png')} alt="navigation" width="18" height="18"/> Page top</a>
                    </div>
                </footer>

            </div>
        );
    }
}
