import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message';
import StateDrop from '../Util/StateDrop';
import CityDrop from '../Util/CityDrop';

export class ManageCustomer extends Component {

    constructor(props) {
        super(props);
        this.state = {
            CustomerList: [],
            ImagePath: "assets/Upload/Employee/blankprofile.jpg",
            FormGroup: {
                CustomerId: 0,
                CustomerCode: '',
                Title:0,
                CustomerName: '',
                isFather: 0,
                FatherHusband:'',
                DOB:'',
                ContactNo: '',
                EmailId: '',
                DOJ: '',
                MaritalStatus: 0,
                StateId: 0,
                CityId: 0,
                Address: '',
                Photo:''
                
            },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    
        this.getCustomerByUrl();

    }

    getCustomerByUrl() {

        axios.get(`api/Masters/GetCustomerId/${this.props.match.params.id}`)
            .then(response => {

                this.FillForm(response.data[0].CustomerId);
            
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

                pic = reader.result;

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
        axios.get(`api/Masters/GetOneCustomer/${ID}`)
            .then(response => {

                const UserInput = this.state.FormGroup;
                
                UserInput['CustomerId'] = response.data[0].CustomerId;
                UserInput['CustomerCode'] = response.data[0].CustomerCode;
                UserInput['Title'] = response.data[0].Title;
                UserInput['CustomerName'] = response.data[0].CustomerName;
                UserInput['isFather'] = response.data[0].isFather;
                UserInput['FatherHusband'] = response.data[0].FatherHusband;
                UserInput['DOB'] = this.GetFormattedDate(response.data[0].DOB);
                UserInput['ContactNo'] = response.data[0].ContactNo;
                UserInput['EmailId'] = response.data[0].EmailId;
                UserInput['DOJ'] = this.GetFormattedDate(response.data[0].DOJ);
                UserInput['MaritalStatus'] = response.data[0].MaritalStatus;
                UserInput['StateId'] = response.data[0].StateId;
                UserInput['CityId'] = response.data[0].CityId;
                UserInput['Address'] = response.data[0].Address;
                
                this.setState({
                    FormGroup: UserInput,
                    ImagePath: "assets/Upload/Employee/" + response.data[0].CustomerId+".jpg"
                });

            });
    }


    Search(event) {
        axios.get(`api/Masters/GetCustomerId/${event.target.value}`)
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
        
        const target = event.target;
        const value = target.value;
        const name = target.name;

        const UserInput = this.state.FormGroup;

        UserInput[name] = value;

        this.setState({
            FormGroup: UserInput
        });
        
    }

    ChangePic(event) {
        event.target.src = "assets/Upload/Employee/blankprofile.jpg";
    }

    handleSubmit(event) {
        event.preventDefault();
        
        const Customer = this.state.FormGroup

        axios.post(`api/Masters/SaveCustomer`, Customer)
            .then(res => {

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
    
    render() {
        
      return (

          <section className="grid_12">
              <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="ManageUser" method="post" action="#" style={{paddingBottom: "70px"}}>
              
                      <h1>Manage Customer</h1>

                      <span className="relative Search_input">
                          <input type="text" name="SearchCode" onBlur={this.Search.bind(this)} className="full-width" />
                              <a className="search_btn">Search</a>
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
                                                  <label>Customer Code</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.CustomerCode} onChange={this.handleInputChange} name="CustomerCode"  />
                                              </div>

                                              <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label>Customer Name</label>

                                                    <select value={this.state.FormGroup.Title} name="Title" onChange={this.handleInputChange} className="col-md-2 col-sm-2 col-xs-2 pt0 pr0 pl0">
                                                      <option value="0">Select Title</option>
                                                      <option value="1">Mr</option>
                                                      <option value="2">MrsTitle</option>
                                                  </select>

                                                  <div className="col-md-10 col-sm-10 col-xs-10 pt0 pr0 pl0">
                                              <input type="text" className="full-width" value={this.state.FormGroup.CustomerName} onChange={this.handleInputChange} name="CustomerName" />
                                                  </div>
                                              </div>

                                              <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label >Father/Husband Name</label>
                                          <input type="text" className="full-width" value={this.state.FormGroup.FatherHusband} onChange={this.handleInputChange} name="FatherHusband" />
                                              </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                  <label>DOB</label>
                                                  <input type="date" className="full-width" value={this.state.FormGroup.DOB} onChange={this.handleInputChange} name="DOB" />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                    <label>Contact</label>
                                                    <input type="text" className="full-width" value={this.state.FormGroup.ContactNo} onChange={this.handleInputChange} name="ContactNo" />
                                                </div>
                                      
                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3 required">
                                                      <label>EmailId</label>
                                                      <input type="text" className="full-width" value={this.state.FormGroup.EmailId} onChange={this.handleInputChange} name="EmailId" required />
                                                </div>

                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label>DOJ</label>
                                                    <input type="date" className="full-width" value={this.state.FormGroup.DOJ} onChange={this.handleInputChange} name="DOJ" />
                                                </div>
                                      
                                                <div className="col-md-3 col-sm-6 col-xs-12 col-lg-3">
                                                    <label >Marital Status</label>
                                                    <select className="full-width" value={this.state.FormGroup.MaritalStatus} onChange={this.handleInputChange} name="MaritalStatus">
                                                        <option value="0">Select</option>
                                                        <option value="1">Married</option>
                                                        <option value="2">Unmarried</option>
                                                    </select>
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
                                                    <label>Address</label>
                                                    <input type="text" className="full-width" onChange={this.handleInputChange} value={this.state.FormGroup.Address} name="Address" />
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
