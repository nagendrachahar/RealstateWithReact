import React, { Component } from 'react';
import axios from 'axios';
import $ from 'jquery';
import Message from '../../Message.js';
import AccountGroupDrop from '../../Util/AccountGroupDrop.js';

export default class AccountLedger extends Component {

    constructor(props) {
        super(props);
        this.state = {
            LedgerList: [],
            loading: true,
            FormGroup: { LedgerId: 0, GroupId: 0, LedgerName: '' },
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });
        
        this.handleInputChange = this.handleInputChange.bind(this);
        //AccountLedger.renderLedgerTable = AccountLedger.renderLedgerTable.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.fillLedger = this.fillLedger.bind(this);
        this.FillForm(0);

    }
    
    FillForm(ID) {
        axios.get(`api/Masters/GetOneAccountLedger/${ID}`)
            .then(response => {
                console.log(response.data);
                const UserInput = this.state.FormGroup;
                
                UserInput["LedgerId"] = response.data[0].LedgerId;
                UserInput["GroupId"] = response.data[0].GroupId;
                UserInput["LedgerName"] = response.data[0].LedgerName;

                this.setState({
                    FormGroup: UserInput
                });
                
                this.fillLedger();
            });
    }


    fillLedger() {
        axios.get('api/Masters/FillAccountLedger')
            .then(response => {
                this.setState({ LedgerList: response.data, loading: false });
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

        const Ledger = this.state.FormGroup

        axios.post(`api/Masters/SaveAccountLedger`, Ledger)
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

            axios.get(`api/Masters/DeleteAccountLedger/${ID}`)
                .then(res => {
                    this.fillLedger();
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

        const renderLedgerTable = (LedgerList) => {
        return (
            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th style={{ textAlign: "center" }}>Sr. No.</th>
                        <th>Group Group Name</th>
                        <th>Account Ledger Name</th>
                        <th style={{ width: "50px", textAlign: "center" }}>Action</th>
                    </tr>
                </thead>
                <tbody>

                    <tr>
                        <td style={{ textAlign: "center" }}><img src="assets/images/icons/fugue/arrow-circle.png" alt="refresh" width="16" height="16" /></td>
                        <td>
                            <AccountGroupDrop GroupId={this.state.FormGroup.GroupId} func={this.handleInputChange} />
                        </td>
                        <td><input type="text" name="LedgerName" value={this.state.FormGroup.LedgerName} onChange={this.handleInputChange} className="full-width" /></td>
                        <td style={{ textAlign: "center" }}>
                            <button type="submit" className="form_button" style={{ color: "white", textAlign: "center" }}>Save</button>
                        </td>
                    </tr>

                    {LedgerList.map((forecast, i) =>

                        <tr key={forecast.LedgerId}>
                            <td>{i + 1}</td>
                            <td>{forecast.GroupName}</td>
                            <td>{forecast.LedgerName}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <a onClick={this.FillForm.bind(this, forecast.LedgerId)} title="Edit" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></a>
                                <a onClick={this.Delete.bind(this, forecast.LedgerId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }
        

        let LedgerList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderLedgerTable(this.state.LedgerList);

  
      return (

            <section className="grid_12">
                <div className="block-border" style={{ position: "relative" }}>
                  <form className="block-content form" onSubmit={this.handleSubmit} id="Cityform" method="post" action="#">
                      <h1>Account Ledger</h1>

                      <Message message={this.state.Message} />

                        <div className="columns">
                          {LedgerList}
                                  
                        </div>
                    </form>
                </div>
            </section>
          
    );
  }
}
