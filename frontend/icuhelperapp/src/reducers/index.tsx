import { combineReducers } from "redux";
import auth from "./auth";
import toasts from "./toasts.js";

export default combineReducers({
  auth,
  toasts
});