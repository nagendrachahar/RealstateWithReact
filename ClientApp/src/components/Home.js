import React, { Component } from 'react';

export class Home extends Component {


    render() {
        const sectionStyle = {
            marginBottom: '10px'
        };

        const divStyle = {
            overflow: 'auto'
        };

        const tableStyle = {
            border: 'black',
            borderStyle: 'solid',
            borderWidth: 'thin',
            width: '100%'
        };

      return (

          <div>
              <section className="grid_12" style={sectionStyle}>
                  <div className="grid_6">
                      <div className="block-border">

                          <div style={divStyle}>
                              <table className="table" style={tableStyle}>
                                  <tbody>
                                      <tr>
                                          <td>Member</td>
                                          <td>1234</td>
                                          <td>Branch</td>
                                          <td>5678</td>
                                      </tr>
                                      <tr>
                                          <td>Employee</td>
                                          <td>1234</td>
                                          <td>CASH</td>
                                          <td>1234</td>
                                      </tr>
                                      <tr>
                                          <td>Fixed Deposit</td>
                                          <td>1234</td>
                                          <td>BANK</td>
                                          <td>1234</td>
                                      </tr>
                                      <tr>
                                          <td>Recurring Deposit</td>
                                          <td>1234</td>
                                          <td>Income</td>
                                          <td>1234</td>
                                      </tr>
                                      <tr>
                                          <td>Loan A/c</td>
                                          <td>1234</td>
                                          <td>Expense</td>
                                          <td>1234</td>
                                      </tr>
                                      <tr>
                                          <td>Saving A/c</td>
                                          <td>1234</td>
                                          <td>Outstanding</td>
                                          <td>1234</td>
                                      </tr>

                                  </tbody>

                              </table>
                          </div>
                      </div>
                  </div>

                  <div className="grid_6">
                      <div className="block-border">
                          <div style={divStyle}>
                              <table className="table" style={tableStyle}>
                                  <thead>
                                      <tr>
                                          <th>&nbsp;</th>
                                          <th>User Code</th>
                                          <th>User Name</th>
                                          <th>Login Time</th>
                                          <th>IP Address</th>
                                      </tr>
                                  </thead>

                                  <tbody>

                                  </tbody>

                              </table>
                          </div>
                      </div>
                  </div>

              </section>
              <section className="grid_12">
                  <div className="grid_4">
                      <div className="block-border">

                          <div style={divStyle}>
                              <table className="table" style={tableStyle}>
                                  <tbody>
                                      <tr>
                                          <td>Policy Code</td>
                                          <td>Due Date</td>
                                          <td>Maturity Date</td>
                                      </tr>

                                  </tbody>

                              </table>
                          </div>
                      </div>
                  </div>

                  <div className="grid_4">
                      <div className="block-border">

                          <div style={{ divStyle }}>
                              <table className="table" style={{ tableStyle }}>
                                  <tbody>
                                      <tr>
                                          <td>Member Code</td>
                                          <td>Birthday Date</td>
                                          <td>Year Completion</td>
                                      </tr>

                                  </tbody>

                              </table>
                          </div>
                      </div>
                  </div>

                  <div className="grid_4">
                      <div className="block-border">

                          <div style={{ divStyle }}>
                              <table className="table" style={{ tableStyle }}>
                                  <tbody>
                                      <tr>
                                          <td>Event</td>
                                          <td>Holiday</td>
                                          <td>Date</td>
                                      </tr>

                                  </tbody>

                              </table>
                          </div>
                      </div>
                  </div>
              </section>

          </div>
          

    );
  }
}
