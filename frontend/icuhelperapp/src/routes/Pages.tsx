import * as React from "react";
import { Router, Switch, Redirect } from "react-router-dom";

// core components
import Admin from "layouts/Admin.js";
import RTL from "layouts/RTL.js";
import Login from "layouts/Login";
import history from "../router/history";

import AuthenticatedRoute from "../routes/AuthenticatedRoute";
import GuestRoute from "../routes/GuestRoute";

const Pages = () => {
  return (
    <Router history={history}>
      <Switch>
        <AuthenticatedRoute path="/admin" component={Admin} />
        <AuthenticatedRoute path="/rtl" component={RTL} />
        <GuestRoute  path="/login" component={Login} />
        <Redirect from="/" to="/login" />
      </Switch>
    </Router>
  );
};

export default Pages;