import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Router } from 'react-router-dom';
import App from './App';
import { Provider } from 'react-redux';
import store from './store';
import registerServiceWorker from './registerServiceWorker';
import createBrowserHistory from 'history/createBrowserHistory';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const customHistory = createBrowserHistory({ basename: baseUrl });

const rootElement = document.getElementById('root');

ReactDOM.render(
  <Provider store={store}>
      <Router history={customHistory}>
          <App />
      </Router>
  </Provider>,
  rootElement);

registerServiceWorker();
