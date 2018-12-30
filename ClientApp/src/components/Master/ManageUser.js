import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import BranchDrop from '../Util/BranchDrop.js';

export class ManageUser extends Component {

    constructor(props) {
        super(props);
        this.state = {
            BranchList: [],
            Branchloading: true,
            UserList: [],
            Userloading: true,
            DeleteAlertLoding: false,
            FormGroup: { UserId: 0, UserCode: '', UserName: '', BranchId: 1, Password: '' },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

        axios.get('api/Masters/GetBranch')
            .then(response => {
                this.setState({ BranchList: response.data, Branchloading: false });
            });

        this.fillUsers = this.fillUsers.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        //ManageUser.renderBranchlist = ManageUser.renderBranchlist.bind(this);
        ManageUser.renderUserTable = ManageUser.renderUserTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        //this.FillForm = this.FillForm.bind(this);
        this.FillForm(0);
        this.fillUsers();

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneUser/${ID}`)
            .then(response => {

                const UserInput = this.state.FormGroup;
                

                UserInput["UserId"] = response.data[0].UserId;
                UserInput["UserCode"] = response.data[0].UserCode;
                UserInput["UserName"] = response.data[0].UserName;
                UserInput["BranchId"] = response.data[0].BranchId;
                UserInput["Password"] = response.data[0].Password;

                this.setState({
                    FormGroup: UserInput
                });

            });
    }


    fillUsers() {
        axios.get('api/Masters/FillUsers')
            .then(response => {
                this.setState({ UserList: response.data, Userloading: false });
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
        
    }

    handleSubmit(event) {
        event.preventDefault();

        const User = this.state.FormGroup

        axios.post(`api/Masters/SaveUser`, User)
            .then(res => {
                this.fillUsers();
                this.FillForm(0);
                this.setState({
                    Message: res.data[0].Message
                });
            })
        $(document).ready(function () {
            $(".message").show();
        });
    }

    Delete(ID) {
        var body = document.getElementsByTagName('body')[0];
        var overlay = document.createElement('div');
        overlay.setAttribute("id", "myConfirm");
        var box = document.createElement('div');
        var boxchild = document.createElement('div');
        var p = document.createElement('p');
        p.appendChild(document.createTextNode("Are you sure"));
        boxchild.appendChild(p);
        var yesButton = document.createElement('button');
        var noButton = document.createElement('button');
        yesButton.appendChild(document.createTextNode('Yes'));
        yesButton.addEventListener('click', () => {

            axios.get(`api/Masters/DeleteUser/${ID}`)
                .then(res => {
                    this.fillUsers();
                    this.setState({
                        Message: res.data[0].Message
                    });
                })

            $(document).ready(function () {
                $(".message").show();
            });

            body.removeChild(overlay);
        }, false);

        noButton.appendChild(document.createTextNode('No'));
        noButton.addEventListener('click', () => {

            body.removeChild(overlay);

        }, false);
        boxchild.appendChild(yesButton);
        boxchild.appendChild(noButton);
        box.appendChild(boxchild);
        overlay.appendChild(box)
        body.appendChild(overlay);
    }
    
 
    static renderUserTable(UserList) {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th scope="col" style={{ textAlign: "center" }}>Sr. No.</th>
                        <th scope="col">UserCode</th>
                        <th scope="col">UserName</th>
                        <th scope="col">Password</th>
                        <th scope="col">BranchName</th>
                        <th scope="col" style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {UserList.map((forecast, i) =>
                        
                        <tr key={forecast.UserId}>
                            <td>{i+1}</td>
                            <td>{forecast.UserCode}</td>
                            <td>{forecast.UserName}</td>
                            <td>{forecast.Password}</td>
                            <td>{forecast.BranchName}</td>
                            <td className="table-actions" style={{textAlign:"center"}}>
                                <a onClick={this.FillForm.bind(this, forecast.UserId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.UserId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
           
        );
    }
    
    render() {
        

        let UserList = this.state.Userloading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : ManageUser.renderUserTable(this.state.UserList);

  
      return (

          <section className="grid_12">
              <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="ManageUser" method="post" action="#" style={{paddingBottom: "70px"}}>
              
                      <h1>Manage User</h1>

                      <Message message={this.state.Message} />

                      <div className="columns">
                          <div id="tab-locales">
                              <div id="tab-en">
                                  <div className="col-md-12 col-sm-12 col-xs-12">

                                        <div className="col-md-3 col-sm-3 col-xs-12 required">
                                          <label>User Code</label>
                                          <input type="text" name="UserCode" value={this.state.FormGroup.UserCode} onChange={this.handleInputChange} className="full-width" />
                                        </div>

                                        <div className="col-md-3 col-sm-3 col-xs-12">
                                          <label>User Name</label>
                                          <input type="text" name="UserName" value={this.state.FormGroup.UserName} onChange={this.handleInputChange} className="full-width" />
                                      </div>

                                      <div className="col-md-3 col-sm-3 col-xs-12">
                                          <label>Branch</label>
                                          <BranchDrop BranchId={this.state.FormGroup.BranchId} func={this.handleInputChange} />
                                      </div>

                                        

                                        <div className="col-md-3 col-sm-3 col-xs-12">
                                          <label>Password</label>
                                          <input type="password" name="Password" value={this.state.FormGroup.Password} onChange={this.handleInputChange} className="full-width" required />
                                        </div>

                                        <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                            <div className="align-center" style={{marginTop:"20px"}}>
                                                <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="Save" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                                <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="Reset" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                            </div>
                                        </div>

                                  </div>
                              </div>
                          </div>
                      </div>
                      {UserList}
                  </form>
              </div>
          </section>
          
    );
  }
}
