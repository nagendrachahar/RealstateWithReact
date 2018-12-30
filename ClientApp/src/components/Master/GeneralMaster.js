import React, { Component } from 'react';
import $ from 'jquery';
import StateMaster from './GeneralMaster/StateMaster.js';
import CityMaster from './GeneralMaster/CityMaster.js';
import BranchMaster from './GeneralMaster/BranchMaster.js';
import SegmentMaster from './GeneralMaster/SegmentMaster.js';
import PlotTypeMaster from './GeneralMaster/PlotTypeMaster.js';
import BankMaster from './GeneralMaster/BankMaster.js';
import VisitorPurpose from './GeneralMaster/VisitorPurpose.js';
import ProjectMaster from './GeneralMaster/Project.js';
import SectorMaster from './GeneralMaster/Sector.js';
import BlockMaster from './GeneralMaster/BlockMaster.js';
import DepartmentMaster from './GeneralMaster/Department.js';
import DesignationMaster from './GeneralMaster/Designation.js';
import AccountGroup from './GeneralMaster/AccountGroup.js';
import AccountLedger from './GeneralMaster/AccountLedger.js';
import PlotDetail from './GeneralMaster/PlotDetail';
import RateMaster from './GeneralMaster/RateMaster';
import EMIPlan from './GeneralMaster/EMIPlan';

export class GeneralMaster extends Component {

    constructor(props) {
        super(props);
        this.state = {
            StateLoading: true,
            CityLoading: false,
            BranchLoading: false,
            SegmentLoading: false,
            PlotTypeLoading: false,
            BankLoading: false,
            VisitorPurposeLoading: false,
            ProjectLoading: false,
            SectorLoading: false,
            BlockLoading: false,
            DepartmentLoading: false,
            DesignationLoading: false,
            AccountGroupLoading: false,
            AccountLedgerLoading: false,
            PlotDetailLoding: false,
            RateMasterLoading: false,
            EMIPlanLoading: false,
            Message: 'wait....!'
        };

        $(document).ready(function () {
            $(".close-bt").click(function () {
                $(".message").hide();
            });
        });

        //this.ShowAll = this.ShowAll.bind(this);
        this.hideAll = this.hideAll.bind(this);
    }

    hideAll() {
        this.setState({
            StateLoading: false,
            CityLoading: false,
            BranchLoading: false,
            SegmentLoading: false,
            PlotTypeLoading: false,
            BankLoading: false,
            VisitorPurposeLoading: false,
            ProjectLoading: false,
            SectorLoading: false,
            BlockLoading: false,
            DepartmentLoading: false,
            DesignationLoading: false,
            AccountGroupLoading: false,
            AccountLedgerLoading: false,
            PlotDetailLoding: false,
            RateMasterLoading: false,
            EMIPlanLoading: false
        });

    }
    
    
    ShowAll(show) {

        this.hideAll();

        if (show === "State") {
            this.setState({
                StateLoading: true
            });
        }
        else if (show === "City") {
            this.setState({
                CityLoading: true
            });
        }
        else if (show === "Branch") {
            this.setState({
                BranchLoading: true
            });
        }
        else if (show === "Segment") {
            this.setState({
                SegmentLoading: true
            });
        }
        else if (show === "PlotType") {
            this.setState({
                PlotTypeLoading: true
            });
        }
        else if (show === "Bank") {
            this.setState({
                BankLoading: true
            });
        }
        else if (show === "VisitorPurpose") {
            this.setState({
                VisitorPurposeLoading: true
            });
        }
        else if (show === "Project") {
            this.setState({
                ProjectLoading: true
            });
        }
        else if (show === "Sector") {
            this.setState({
                SectorLoading: true
            });
        }
        else if (show === "Block") {
            this.setState({
                BlockLoading: true
            });
        }
        else if (show === "Department") {
            this.setState({
                DepartmentLoading: true
            });
        }
        else if (show === "Designation") {
            this.setState({
                DesignationLoading: true
            });
        }
        else if (show === "AccountGroup") {
            this.setState({
                AccountGroupLoading: true
            });
        }
        else if (show === "AccountLedger") {
            this.setState({
                AccountLedgerLoading: true
            });
        }
        else if (show === "PlotDetail") {
            this.setState({
                PlotDetailLoding: true
            });
        }
        else if (show === "Rate") {
            this.setState({
                RateMasterLoading: true
            });
        }
        else if (show === "EMIPlan") {
            this.setState({
                EMIPlanLoading: true
            });
        }

    }
    
    
    render() {
        
  
      return (

          <div>

                <div className="grid_3">

                    <section className="with-margin">
                        <div className="block-border">
                            <form className="block-content form" id="simple-list-form" method="post" action="#">
                                <h1>General Master list</h1>

                                <p className="input-type-text">
                                  <input type="text" name="simple-search" id="simple-search" value="" style={{ width: "90%" }} title="Filter results" />
                                  <img src="assets/images/icons/fugue/magnifier.png" alt="Search" width="16" height="16" className="float-right" />
                                </p>

                              <ul className="simple-list with-icon" style={{ maxHeight: "350px", overflow:"auto" }}>
                                        
                                      <li><a onClick={this.ShowAll.bind(this, "State")}>State</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "City")}>City</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Branch")}>Branch</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Segment")}>Segment</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "PlotType")}>Plot Type</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Bank")}>Bank</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "VisitorPurpose")}>Visitor Purpose</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Project")}>Project</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Sector")}>Sector</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Block")}>Block</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Rate")}>Rate</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "PlotDetail")}>Plot Detail</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Department")}>Department</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "Designation")}>Designation</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "State")}>Remunarration Structure</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "AccountGroup")}>Account Group</a></li>
                                      <li><a onClick={this.ShowAll.bind(this, "AccountLedger")}>Account Ledger</a></li> 
                                      <li><a onClick={this.ShowAll.bind(this, "EMIPlan")}>EMI Plan</a></li> 

                                </ul>
                            </form>
                        </div>
                    </section>
                </div>

                <section className="grid_9">
                    <div className="childrout">
                        {this.state.StateLoading ? <StateMaster /> : null}
                        {this.state.CityLoading ? <CityMaster /> : null}
                        {this.state.BranchLoading ? <BranchMaster /> : null}
                        {this.state.SegmentLoading ? <SegmentMaster /> : null}
                        {this.state.PlotTypeLoading ? <PlotTypeMaster /> : null}
                        {this.state.BankLoading ? <BankMaster /> : null}
                        {this.state.VisitorPurposeLoading ? <VisitorPurpose /> : null}
                        {this.state.ProjectLoading ? <ProjectMaster /> : null}
                        {this.state.SectorLoading ? <SectorMaster /> : null}
                        {this.state.BlockLoading ? <BlockMaster /> : null}
                        {this.state.DepartmentLoading ? <DepartmentMaster /> : null}
                      {this.state.DesignationLoading ? <DesignationMaster /> : null}
                      {this.state.AccountGroupLoading ? <AccountGroup /> : null}
                      {this.state.AccountLedgerLoading ? <AccountLedger /> : null}
                      {this.state.PlotDetailLoding ? <PlotDetail /> : null}
                      {this.state.RateMasterLoading ? <RateMaster /> : null}
                      {this.state.EMIPlanLoading ? <EMIPlan /> : null}
                      
                    </div>
                </section>

          </div>
          
    );
  }
}
