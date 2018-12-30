import React, { Component } from 'react';
import { Route } from 'react-router';

import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ManageUser } from './components/Master/ManageUser';
import { UserPermission } from './components/Master/UserPermission';
import { ManageEmployee } from './components/Employee/ManageEmployee';
import { EmployeeExplorer } from './components/Employee/EmployeeExplorer';
import { ManageCustomer } from './components/Customer/ManageCustomer';
import { CustomerExplorer } from './components/Customer/CustomerExplorer';
import { RemunerationStructure } from './components/Employee/RemunerationStructure';
import { ExtraRemuneration } from './components/Employee/ExtraRemuneration';
import { GeneralMaster } from './components/Master/GeneralMaster';
import { AccountTransaction } from './components/Accounts/AccountTransaction';
import { VisitorEntry } from './components/Visitor/VisitorEntry';
import { ProjectView } from './components/GraphicView/ProjectView';
import { SectorView } from './components/GraphicView/SectorView';
import { BlockView } from './components/GraphicView/BlockView';
import { PlotView } from './components/GraphicView/PlotView';
import PlotBooking from './components/Customer/PlotBooking';
import { Configuration } from './components/Configuration/Configuration';

//import createBrowserHistory from 'history/createBrowserHistory';
//const customHistory = createBrowserHistory();

export default class App extends Component {
  displayName = App.name

  render() {
      return (
              <Layout>
                  <Route exact path='/' component={Home} />
                  <Route exact path='/home' component={Home} />
                  <Route path='/ManageUser' component={ManageUser} />
                  <Route path='/UserPermission' component={UserPermission} />
                  <Route path='/ManageEmployee/:id' component={ManageEmployee} />
                  <Route path='/EmployeeExplorer' component={EmployeeExplorer} />
                  <Route path='/GeneralMaster' component={GeneralMaster} />
                  <Route path='/RemunerationStructure' component={RemunerationStructure} />
                  <Route path='/ExtraRemuneration' component={ExtraRemuneration} />
                  <Route path='/AccountTransaction' component={AccountTransaction} />
                  <Route path='/VisitorEntry' component={VisitorEntry} />
                  <Route path='/GraphicView' component={ProjectView} />
                  <Route path='/GraphicViewSector/:ID' component={SectorView} />
                  <Route path='/GraphicViewBlock/:ID' component={BlockView} />
                  <Route path='/GraphicViewPlot/:ID' component={PlotView} />
                  <Route path='/ManageCustomer/:id' component={ManageCustomer} />
                  <Route path='/CustomerExplorer' component={CustomerExplorer} />
                  <Route path='/PlotBooking' component={PlotBooking} />
                  <Route path='/Configuration' component={Configuration} />
              </Layout>
      
    );
  }
}
