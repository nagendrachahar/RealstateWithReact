import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import $ from 'jquery';

export class EmployeeExplorer extends Component {

    constructor(props) {
        super(props);
        this.state = {
            EmployeeList: [],
            loading: true,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

        this.fillEmployee = this.fillEmployee.bind(this);
        this.fillEmployee();
        
    }

    fillEmployee() {

        axios.get('api/Masters/FillEmployee')
            .then(response => {
                this.setState({ EmployeeList: response.data, loading: false });
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

            axios.delete(`api/Masters/DeleteEmployee/${ID}`)                .then(res => {                    this.fillEmployee();                    this.setState({
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

        
    const renderEmployeeTable = (EmployeeList) => {
        return (

            <table className="table" id="tablee" style={{ border: "black", borderStyle: "solid", borderWidth: "thin", width: "100%" }}>
                <thead>
                    <tr>
                        
                        <th className="table-actions">Sr.</th>
                        <th className="table-actions">EmployeeCode</th>
                        <th className="table-actions">EmployeeName</th>
                        <th className="table-actions">ContactNo</th>
                        <th className="table-actions">FatherHusbandName</th>
                        <th className="table-actions">MotherName</th>
                        <th className="table-actions">Qualification</th>
                        <th className="table-actions">Action</th>
                    </tr>
                </thead>
                <tbody>
                    {EmployeeList.map((forecast, i) =>

                        <tr key={forecast.EmployeeId}>
                            <td>{i + 1}</td>
                            <td>{forecast.EmployeeCode}</td>
                            <td>{forecast.EmployeeName}</td>
                            <td>{forecast.ContactNo}</td>
                            <td>{forecast.FatherHusbandName}</td>
                            <td>{forecast.MotherName}</td>
                            <td>{forecast.Qualification}</td>
                            <td className="table-actions" style={{ textAlign: "center" }}>
                                <Link to={'ManageEmployee/' + forecast.EmployeeCode} className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/magnifier.png" width="16" height="16" /></Link>

                                <a onClick={this.Delete.bind(this, forecast.EmployeeId)} title="Delete" className="with-tip" style={{ cursor: "pointer" }}><img alt="btn" src="assets/images/icons/fugue/cross-circle.png" width="16" height="16" /></a>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>


        );
    }


        let EmployeeList = this.state.loading
            ?<p><img alt="Loading" src="assets/images/skeleton.gif" style={{ width:"100%" }} /></p>
            :renderEmployeeTable(this.state.EmployeeList);

  
      return (

          <section className="grid_12">
              <div className="block-border">
                  <form className="block-content form" id="table_form" method="post" action="#" style={{ paddingBottom: "70px" }}>
                      <h1>Employee Explorer</h1>

                      {EmployeeList}

                  </form>
              </div>
          </section >
          
    );
  }
}
