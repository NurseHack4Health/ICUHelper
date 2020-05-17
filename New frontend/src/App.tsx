import * as React from "react";
import { connect } from "react-redux";
import { Route, Router } from "react-router-dom";

import history from "./router/history";
import Pages from "./routes/Pages";
import { checkAuthentication } from "./actions/current";

import Toasts from "./components/Toasts/Toast.js";
import { addToast } from "./actions/toasts";

interface IProps {
  checkAuthenticationConnect: () => void;
  isAuthenticated: boolean | null;
}

const App = ({
  checkAuthenticationConnect,
  isAuthenticated
}: IProps) => {
  React.useEffect(() => {
    checkAuthenticationConnect();
  }, []);

  const app = isAuthenticated !== null ? (
    <Router history={history}>
      <Route component={Pages} />
    </Router>
    
  ) : null;

  return (
    <div className="App">
      {app}
    </div>
  );
}

const mapStateToProps = (state) => ({
  isAuthenticated: state.auth.isAuthenticated
});

/*function mapDispatchToProps(dispatch) {
  return {
    actions: {
      toastActions: bindActionCreators({ addToast }, dispatch),
    },
    checkAuthenticationConnect: checkAuthentication,

  };
}*/

const mapDispatchToProps = {
  checkAuthenticationConnect: checkAuthentication,
  toastActions: addToast
}

console.log(mapDispatchToProps);

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(App);