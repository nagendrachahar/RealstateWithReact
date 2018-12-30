import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../Message.js';
import ProjectDrop from '../Util/ProjectDrop.js';
import BranchDrop from '../Util/BranchDrop.js';
import AccountLedgerDrop from '../Util/AccountLedgerDrop.js';

export class AccountTransaction extends Component {

    constructor(props) {
        super(props);
        this.state = {
            AccountTransactionList: [],
            loading: true,
            FormGroup: {
                AccountTransactionId: 0,
                VoucherNo: '',
                VoucherDate: '',
                BranchId: 0,
                ProjectId: 0,
                FromLedger: 0,
                ToLedger: 0,
                Remark: '',
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
        this.fillAccountTransaction = this.fillAccountTransaction.bind(this);
        this.FillForm(0);
        //this.Payment();
    }

    GetFormattedDate = (D) => {
        return D.slice(0, 10);
    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneAccountTransaction/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["AccountTransactionId"] = response.data[0].AccountTransactionId;
                UserInput["VoucherNo"] = response.data[0].VoucherNo;
                UserInput["VoucherDate"] = this.GetFormattedDate(response.data[0].VoucherDate);
                UserInput["BranchId"] = response.data[0].BranchId;
                UserInput["ProjectId"] = response.data[0].ProjectId;
                UserInput["FromLedger"] = response.data[0].FromLedger;
                UserInput["ToLedger"] = response.data[0].ToLedger;
                UserInput["Remark"] = response.data[0].Remark;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillAccountTransaction();
            });
    }


    fillAccountTransaction() {
        axios.get('api/Masters/FillAccountTransaction')
            .then(response => {
                console.log(response.data);
                this.setState({ AccountTransactionList: response.data, loading: false });
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

        const Account = this.state.FormGroup

        axios.post(`api/Masters/SaveAccountTransaction`, Account)
            .then(res => {
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

            axios.get(`api/Masters/DeleteAccountTransaction/${ID}`)
                .then(res => {
                    this.fillAccountTransaction();
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

 
    
    render() {

        const renderAccountTable = (AccountTransactionList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>VoucherNo</th>
                        <th>VoucherDate</th>
                        <th>ProjectName</th>
                        <th>FromLedger</th>
                        <th>ToLedger</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    {AccountTransactionList.map((forecast, i) =>

                        <tr key={forecast.AccountTransactionId}>
                            <td>{i + 1}</td>
                            <td>{forecast.VoucherNo}</td>
                            <td>{forecast.VoucherDate}</td>
                            <td>{forecast.ProjectName}</td>
                            <td>{forecast.FromLedger}</td>
                            <td>{forecast.ToLedger}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.AccountTransactionId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.AccountTransactionId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}

                </tbody>
            </table>
        );
    }

        let AccountTransactionList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderAccountTable(this.state.AccountTransactionList);
        
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} method="post" action="#">
                      <h1>Account Transaction</h1>

                        <Message message={this.state.Message} />

                        <div className="columns">

                          <div className="col-md-12 col-sm-12 col-xs-12">
                              
                              
                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Voucher No</label>
                                  <input type="text" className="full-width" name="VoucherNo" value={this.state.FormGroup.VoucherNo} onChange={this.handleInputChange} />
                                </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Voucher Date</label>
                                  <input type="date" className="full-width" name="VoucherDate" value={this.state.FormGroup.VoucherDate} onChange={this.handleInputChange} />
                              </div>

                                <div className="col-md-4 col-sm-12 col-xs-12 required">
                                  <label>Branch</label>
                                  <BranchDrop
                                      BranchId={this.state.FormGroup.BranchId}
                                      func={this.handleInputChange} />
                                </div>

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label >Project</label>
                                  <ProjectDrop
                                      ProjectId={this.state.FormGroup.ProjectId}
                                      func={this.handleInputChange} />
                                </div>
                              

                                <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>From Ledger</label>
                                  <AccountLedgerDrop
                                      LedgerName="FromLedger"
                                      LedgerId={this.state.FormGroup.FromLedger}
                                      func={this.handleInputChange} />
                                </div>

                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>To Ledger</label>
                                  <AccountLedgerDrop
                                      LedgerName="ToLedger"
                                      LedgerId={this.state.FormGroup.ToLedger}
                                      func={this.handleInputChange} />
                              </div>


                              <div className="col-md-4 col-sm-12 col-xs-12">
                                  <label>Remark</label>
                                  <input type="text" className="full-width" name="Remark" value={this.state.FormGroup.Remark} onChange={this.handleInputChange} />
                              </div>
                              
                                <div className="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                                  <div className="align-center" style={{marginTop:"20px"}}>
                                      <button type="submit" className="form_button" style={{ color: "white" }}><img src="assets/images/icons/fugue/tick-circle.png" alt="btn" width="16" height="16" />Save</button>&nbsp; &nbsp; &nbsp; &nbsp;
                                      <button type="button" className="red form_button"><img src="assets/images/icons/fugue/arrow-circle.png" alt="btn" width="16" height="16" /><span style={{ color: "white" }}> Reset</span></button>
                                    </div>
                                </div>

                            </div>
                                  
                        </div>

                      {AccountTransactionList}

                  </form>
                </div>
            </section>
          
    );
  }
}
