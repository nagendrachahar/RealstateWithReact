import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import BranchDrop from '../Util/BranchDrop.js';
import DepartmentDrop from '../Util/DepartmentDrop.js';
import DesignationDrop from '../Util/DesignationDrop.js';
import StateDrop from '../Util/StateDrop.js';
import CityDrop from '../Util/CityDrop.js';
import RelationDrop from '../Util/RelationDrop.js';

export class ManageEmployee extends Component {

    constructor(props) {
        super(props);
        this.state = {
            Branchloading: true,
            UserList: [],
            Userloading: true,
            DeleteAlertLoding: false,
            ImagePath: "assets/Upload/Employee/blankprofile.jpg",
            FormGroup: {
                SearchCode:'',
                EmployeeId: 0,
                EmployeeCode:'',
                TitleId:0,
                EmployeeName: '',
                FatherHusbandName:'',
                MotherName:'',
                DOJ: '',
                BranchId: 0,
                DepartmentId: 0,
                DesignationId: 0,
                WorkLocation: '',
                ReportingPersonCode: '',
                ReportingPerson: '',
                DOB: '',
                ContactNo: '',
                EmailId: '',
                MaritalStatus: '',
                Qualification: '',
                Address: '',
                StateId: 0,
                CityId: 0,
                Pincode: '',
                Photo: '',
                PersonName1: '',
                ContactNo1: '',
                RelationId1: 0,
                PersonName2: '',
                ContactNo2: '',
                RelationId2: 0
            },
            ReportingName: '',
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });


        this.fillUsers = this.fillUsers.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        //ManageEmployee.renderUserTable = ManageEmployee.renderUserTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.getReporting = this.getReporting.bind(this);
        //this.FillForm = this.FillForm.bind(this);
        //this.FillForm(0);
        this.fillUsers();
        this.getEmployeeByUrl();

    }

    getEmployeeByUrl() {

        axios.get(`api/Masters/GetEmployeeId/${this.props.match.params.id}`)
            .then(response => {
                
                this.FillForm(response.data[0].EmployeeId);
                
            });
    }

    onFileChange(event) {
        const reader = new FileReader();
        var pic;
        if (event.target.files && event.target.files.length) {
            const [file] = event.target.files;
            reader.readAsDataURL(file);

            reader.onload = () => {
                const UserInput = this.state.FormGroup;
                UserInput["Photo"] = reader.result
                
                this.setState({
                    FormGroup: UserInput
                });

                pic = reader.result
                $(document).ready(function () {
                    $("#ProfileImg").attr('src', pic);
                });
                
            };
        }
    }

    GetFormattedDate = (D) => {
      return D.slice(0, 10);
    }


    FillForm(ID) {
        axios.get(`api/Masters/GetOneEmployee/${ID}`)
            .then(response => {

                const UserInput = this.state.FormGroup;
                
                
                UserInput['EmployeeId'] = response.data[0].EmployeeId;
                UserInput['EmployeeCode'] = response.data[0].EmployeeCode;
                UserInput['TitleId'] = response.data[0].TitleId;
                UserInput['EmployeeName'] = response.data[0].EmployeeName;
                UserInput['FatherHusbandName'] = response.data[0].FatherHusbandName;
                UserInput['MotherName'] = response.data[0].MotherName;
                UserInput['DOJ'] = this.GetFormattedDate(response.data[0].DOJ);
                UserInput['BranchId'] = response.data[0].BranchId;
                UserInput['DepartmentId'] = response.data[0].DepartmentId;
                UserInput['DesignationId'] = response.data[0].DesignationId;
                UserInput['WorkLocation'] = response.data[0].WorkLocation;
                UserInput['ReportingPerson'] = response.data[0].ReportingPerson;
                UserInput['ReportingPersonCode'] = response.data[0].ReportingPersonCode;
                UserInput['DOB'] = this.GetFormattedDate(response.data[0].DOJ);
                UserInput['ContactNo'] = response.data[0].ContactNo;
                UserInput['EmailId'] = response.data[0].EmailId;
                UserInput['MaritalStatus'] = response.data[0].MaritalStatus;
                UserInput['Qualification'] = response.data[0].Qualification;
                UserInput['Address'] = response.data[0].Address;
                UserInput['StateId'] = response.data[0].StateId;
                UserInput['CityId'] = response.data[0].CityId;
                UserInput['Pincode'] = response.data[0].Pincode;
                UserInput['PersonName1'] = response.data[0].PersonName1;
                UserInput['ContactNo1'] = response.data[0].ContactNo1;
                UserInput['RelationId1'] = response.data[0].RelationId1;
                UserInput['PersonName2'] = response.data[0].PersonName2;
                UserInput['ContactNo2'] = response.data[0].ContactNo2;
                UserInput['RelationId2'] = response.data[0].RelationId2;

                
                this.setState({
                    FormGroup: UserInput,
                    ImagePath: "assets/Upload/Employee/"+response.data[0].EmployeeCode+".jpg"
                });

            });
    }

    fillUsers() {
        axios.get('api/Masters/FillUsers')
            .then(response => {
                this.setState({ UserList: response.data, Userloading: false });
            });
    }
    

    Search(event) {
        axios.get(`api/Masters/GetEmployeeId/${event.target.value}`)
            .then(response => {
                
                if (response.data[0].EmployeeId !== 0) {
                    this.FillForm(response.data[0].EmployeeId);
                }
                else {
                    this.setState({ Message: "Wrong Code"});

                    $(document).ready(function () {
                        $(".message").show();
                    });
                }
            });
    }

    handleInputChange(event) {

        debugger;

        const target = event.target;
        const value = target.value;
        const name = target.name;

        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });
        
    }

    getReporting(event) {

        const UserInput = this.state.FormGroup;

        axios.get(`api/Masters/GetEmployeeId/${event.target.value}`)
            .then(response => {
                

                UserInput['ReportingPerson'] = response.data[0].EmployeeId;

                this.setState({
                    FormGroup: UserInput,
                    ReportingName: response.data[0].EmployeeName
                });

                if (response.data[0].EmployeeId === 0) {
                    
                    this.setState({ Message: "Wrong Code" });

                    $(document).ready(function () {
                        $(".message").show();
                    });
                }
             
            });
        
    }

    ChangePic(event) {
        event.target.src = "assets/Upload/Employee/blankprofile.jpg";
    }

    handleSubmit(event) {
        event.preventDefault();

        debugger;

        const Employee = this.state.FormGroup

        axios.post(`api/Masters/SaveEmployee`, Employee)
            .then(res => {
                this.fillUsers();

                this.setState({
                    Message: res.data[0].Message
                });
                

                if (res.data[0].MessageType === 1) {
                    this.FillForm(0);
                }

                
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

            axios.get(`api/Masters/DeleteUser/${ID}`)                .then(res => {                    this.fillUsers();                    this.setState({
                        Message: res.data[0].Message
                    });                })

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
    
    
    render() {
        
      return (

          <section className="grid_12">
              <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="ManageUser" method="post" action="#" style={{paddingBottom: "70px"}}>
              
                      <h1>Manage Employee</h1>

                      <span className="relative Search_input">
                              <input type="text" name="SearchCode" className="full-width" />
                              <a className="search_btn" onBlur={this.Search.bind(this)}>Search</a>
                      </span>

                      <Message message={this.state.Message} />

                      <div className="columns">
                          <div id="tab-locales"> 
                              <div id="tab-en">
                                  <div className="col-md-1 col-sm-12 col-xs-12 col-lg-1" style={{paddingLeft: "0px", paddingRight: "0px"}}>
                                      <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">

                                          <div className="profile_img">
                                              <label htmlFor="ProfilePic">
                                                  <img id="ProfileImg" onError={this.ChangePic} alt="Prof" src={this.state.ImagePath} style={{ width: "100px", height: "110px" }} />
                                              </label>
                                          </div>

                                          <div className="profile_img" style={{ display: "none" }}>
                                              <input id="ProfilePic" onChange={this.onFileChange.bind(this)} type="file" />
                                          </div>

                                      </div>
                                  </div>

                                  <div className="col-md-11 col-sm-12 col-xs-12 col-lg-11" style={{paddingLeft:"40px"}}>

                                              <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label >Employee Code</label>
                                                  <input type="text" className="full-width" value={this.state.FormGroup.EmployeeCode} onChange={this.handleInputChange} name="EmployeeCode"  />
                                              </div>

                                              <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label >Employee Name</label>

                                                  <select value={this.state.FormGroup.TitleId} name="TitleId" onChange={this.handleInputChange} className="col-md-2 col-sm-2 col-xs-2 pt0 pr0 pl0">
                                                      <option value="0">Select Title</option>
                                                      <option value="1">Mr</option>
                                                      <option value="2">MrsTitle</option>
                                                  </select>

                                                  <div className="col-md-10 col-sm-10 col-xs-10 pt0 pr0 pl0">
                                                    <input type="text" className="full-width" value={this.state.FormGroup.EmployeeName} onChange={this.handleInputChange} name="EmployeeName" />
                                                  </div>
                                              </div>

                                              <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label >Father/Husband Name</label>
                                                  <input type="text" className="full-width" value={this.state.FormGroup.FatherHusbandName} onChange={this.handleInputChange} name="FatherHusbandName" />
                                              </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                    <label >Mother Name</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.MotherName} onChange={this.handleInputChange} name="MotherName" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                    <label >DOJ</label>
                                                    <input type="date" className="full-width" value={this.state.FormGroup.DOJ} onChange={this.handleInputChange} name="DOJ" />
                                                </div>

                                                  <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                      <label>Branch</label>
                                                      <BranchDrop BranchId={this.state.FormGroup.BranchId} func={this.handleInputChange} />
                                                  </div>

                                                  <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                      <label>Department</label>
                                                      <DepartmentDrop DepartmentId={this.state.FormGroup.DepartmentId} func={this.handleInputChange} />
                                                  </div>

                                                  <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                      <label>Designation</label>
                                                      <DesignationDrop DepartmentType={this.state.FormGroup.DepartmentId} DesignationId={this.state.FormGroup.DesignationId} func={this.handleInputChange} />
                                                  </div>
                      
                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                    <label >Work Location</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.WorkLocation} onChange={this.handleInputChange} name="WorkLocation" required />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                      <label style={{ float: "left" }}>Reporting Person</label>
                                                      <label style={{ float: "right" }}>{this.state.ReportingName}</label>
                                                      <input type="text" className="full-width" value={this.state.FormGroup.ReportingPersonCode} onBlur={this.getReporting} onChange={this.handleInputChange} name="ReportingPersonCode" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Date of Birth</label>
                                                    <input type="date" className="full-width" value={this.state.FormGroup.DOB} onChange={this.handleInputChange} name="DOB" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Contact No.</label>
                                                    <input type="number" className="full-width" value={this.state.FormGroup.ContactNo} onChange={this.handleInputChange} name="ContactNo" maxLength="10" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Email ID</label>
                                                    <input type="email" className="full-width" value={this.state.FormGroup.EmailId} onChange={this.handleInputChange} name="EmailId" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Marital Status</label>
                                                    <select className="full-width" value={this.state.FormGroup.MaritalStatus} onChange={this.handleInputChange} name="MaritalStatus">
                                                        <option value="0">Select</option>
                                                        <option value="1">married</option>
                                                        <option value="2">Unmarried</option>
                                                    </select>
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Qualification</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.Qualification} onChange={this.handleInputChange} name="Qualification" maxLength="10" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Address</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.Address} onChange={this.handleInputChange} name="Address" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label>State</label>
                                                    <StateDrop StateId={this.state.FormGroup.StateId} func={this.handleInputChange} />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label>City</label>
                                                    <CityDrop CityId={this.state.FormGroup.CityId} StateId={this.state.FormGroup.StateId} func={this.handleInputChange} />

                                                </div>
                                      

                                      
                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Pin Code</label>
                                                    <input type="text" className="full-width" maxLength="6" onChange={this.handleInputChange} value={this.state.FormGroup.Pincode} name="Pincode" />
                                                </div>

                                                <h4 style={{display:"block", clear:"both", paddingTop:"15px"}}>Emergency Person</h4>

                                                <div className="col-md-4 col-sm-6 col-xs-12 col-lg-4">
                                                    <label >PersonName1</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.PersonName1} onChange={this.handleInputChange} name="PersonName1" />
                                                </div>

                                                <div className="col-md-4 col-sm-6 col-xs-12 col-lg-4">
                                                    <label >ContactNo1</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.ContactNo1} onChange={this.handleInputChange} name="ContactNo1" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-4">
                                                    <label>Relation</label>
                                                    <RelationDrop RelationName="RelationId1" RelationId={this.state.FormGroup.RelationId1} func={this.handleInputChange} />

                                                </div>

                                      
                                      <br style={{ clear: "both" }} />

                                                <div className="col-md-4 col-sm-6 col-xs-12 col-lg-4">
                                                    <label >PersonName2</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.PersonName2} onChange={this.handleInputChange} name="PersonName2" />
                                                </div>

                                                <div className="col-md-4 col-sm-6 col-xs-12 col-lg-4">
                                                    <label >ContactNo2</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.ContactNo2} onChange={this.handleInputChange} name="ContactNo2" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-4">
                                                    <label>Relation</label>
                                                    <RelationDrop RelationName="RelationId2" RelationId={this.state.FormGroup.RelationId2} func={this.handleInputChange} />

                                                </div>
                                      
                                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                                    <div className="align-center" style={{marginTop:"20px"}}>
                                                        <button type="submit" className="form_button" style={{ color: "white" }}><img alt="btn" src="assets/images/icons/fugue/tick-circle.png" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                                        <button type="button" className="red form_button"><img alt="btn" src="assets/images/icons/fugue/arrow-circle.png" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                                    </div>
                                                </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                      {
                          //display Table Here
                      }
                  </form>
              </div>
          </section>
          
    );
  }
}
