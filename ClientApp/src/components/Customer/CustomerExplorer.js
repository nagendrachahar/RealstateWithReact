import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import $ from 'jquery';

export class CustomerExplorer extends Component {

    constructor(props) {
        super(props);
        this.state = {
            CustomerList: [],
            loading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

        this.fillCustomer = this.fillCustomer.bind(this);
        this.fillCustomer();
        
    }

    fillCustomer() {

        axios.get('api/Masters/FillCustomer')
            .then(response => {
                console.log(response.data);
                this.setState({ CustomerList: response.data, loading: false });
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

            axios.delete(`api/Masters/DeleteCustomer/${ID}`)                .then(res => {                    this.fillCustomer();                    this.setState({
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
        
        const renderCustomerTable = (CustomerList) => {
        return (

            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        <th className="table-actions">&nbsp;</th>
                        <th className="table-actions">Sr.</th>
                        <th className="table-actions">CustomerCode</th>
                        <th className="table-actions">CustomerName</th>
                        <th className="table-actions">ContactNo</th>
                        <th className="table-actions">FatherHusband</th>
                        <th className="table-actions">DOJ</th>
                    </tr>
                </thead>
                <tbody>
                    {CustomerList.map((forecast, i) =>

                        <tr key={forecast.CustomerId}>
                            <td>{i + 1}</td>
                            <td>{forecast.CustomerCode}</td>
                            <td>{forecast.CustomerName}</td>
                            <td>{forecast.ContactNo}</td>
                            <td>{forecast.FatherHusband}</td>
                            <td>{forecast.DOJ}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <Link to={'ManageCustomer/' + forecast.CustomerCode} className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></Link>
                                <a onClick={this.Delete.bind(this, forecast.CustomerId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>


        );
    }


        let CustomerList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            : renderCustomerTable(this.state.CustomerList);

  
      return (

          <section className="grid_12">
              <div className="block-border">
                  <form className="block-content form" id="table_form" method="post" action="#" style={{ paddingBottom: "70px" }}>
                      <h1>Customer Explorer</h1>

                      {CustomerList}

                  </form>
              </div>
          </section >
          
    );
  }
}
