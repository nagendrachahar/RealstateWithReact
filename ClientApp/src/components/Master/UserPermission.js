import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import UserDrop from '../Util/UserDrop.js';

export class UserPermission extends Component {

    constructor(props) {
        super(props);
        this.state = {
            MenuList: [],
            loading: true,
            FormGroup: { UserId: 0 },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

     
        this.handleInputChange = this.handleInputChange.bind(this);
        this.fillMenu = this.fillMenu.bind(this);
        this.fillMenu();

    }
    


    fillMenu() {

        axios.get('api/Masters/FillMenuTable/' + this.state.FormGroup.UserId)
            .then(response => {
                console.log(response.data);
                this.setState({ MenuList: response.data, loading: false });
            });
    }

    handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });

        this.fillMenu();
    }

    SaveFlag(MenuId) {

        const UserId = this.state.FormGroup.UserId
        if (UserId > 0) {
            axios.get('api/Masters/Save_SavePermission/' + MenuId + '/' + UserId)
                .then(res => {
                    this.setState({
                        Message: res.data[0].Message
                    });
                    this.fillMenu();
                })
        }
        else {
            this.setState({
                Message: "Please Select User First"
            });
        }
        
        $(document).ready(function () {
            $(".message").show();
        });
    }

    UpdateFlag(MenuId) {

        const UserId = this.state.FormGroup.UserId
        if (UserId > 0) {
            axios.get('api/Masters/Save_UpdatePermission/' + MenuId + '/' + UserId)
                .then(res => {
                    this.setState({
                        Message: res.data[0].Message
                    });
                    this.fillMenu();
                })
        }
        else {
            this.setState({
                Message: "Please Select User First"
            });
        }

        $(document).ready(function () {
            $(".message").show();
        });
    }

    DeleteFlag(MenuId) {

        const UserId = this.state.FormGroup.UserId
        if (UserId > 0) {
            axios.get('api/Masters/Save_DeletePermission/' + MenuId + '/' + UserId)
                .then(res => {
                    this.setState({
                        Message: res.data[0].Message
                    });
                    this.fillMenu();
                })
        }
        else {
            this.setState({
                Message: "Please Select User First"
            });
        }

        $(document).ready(function () {
            $(".message").show();
        });
    }

    ViewFlag(MenuId) {

        const UserId = this.state.FormGroup.UserId
        if (UserId > 0) {
            axios.get('api/Masters/Save_ViewPermission/' + MenuId + '/' + UserId)
                .then(res => {
                    this.setState({
                        Message: res.data[0].Message
                    });
                    this.fillMenu();
                })
        }
        else {
            this.setState({
                Message: "Please Select User First"
            });
        }

        $(document).ready(function () {
            $(".message").show();
        });
    }
    
    render() {

        const renderMenuTable = (MenuList) => {
            return (
                <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                    <thead>
                        <tr>
                            <th style={{ textAlign: "center" }}>Sr. No.</th>
                            <th>Menu Name</th>
                            <th>Save</th>
                            <th>Update</th>
                            <th>Delete</th>
                            <th>View</th>
                        </tr>
                    </thead>
                    <tbody>
                        {MenuList.map((forecast, i) =>

                            <tr key={forecast.MenuId}>
                                <td>{i + 1}</td>
                                <td>{forecast.MenuName}</td>
                                <td><input type="checkbox" onChange={this.SaveFlag.bind(this, forecast.MenuId)} checked={1 === forecast.Save} /></td>
                                <td><input type="checkbox" onChange={this.UpdateFlag.bind(this, forecast.MenuId)} checked={1 === forecast.Update} /></td>
                                <td><input type="checkbox" onChange={this.DeleteFlag.bind(this, forecast.MenuId)} checked={1 === forecast.Delete} /></td>
                                <td><input type="checkbox" onChange={this.ViewFlag.bind(this, forecast.MenuId)} checked={1 === forecast.View} /></td>
                            </tr>
                        )}
                    </tbody>
                </table>

            );
        }
        

        let MenuList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderMenuTable(this.state.MenuList);

  
      return (

          <section className="grid_12">
              <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" method="post" action="#" style={{paddingBottom: "70px"}}>
              
                      <h1>Manage User</h1>

                      <Message message={this.state.Message} />

                      <div className="columns">
                          <div id="tab-locales">
                              <div id="tab-en">
                                  <div className="col-md-12 col-sm-12 col-xs-12">
                                      
                                      <div className="col-md-3 col-sm-3 col-xs-12">
                                          <label>User</label>
                                          <UserDrop UserId={this.state.FormGroup.UserId} func={this.handleInputChange} />
                                      </div>
                                      

                                  </div>
                              </div>
                          </div>
                      </div>
                      {MenuList}
                  </form>
              </div>
          </section>
          
    );
  }
}
