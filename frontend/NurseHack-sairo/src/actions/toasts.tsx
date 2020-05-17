import createToast from "../factories/createToast";
import { ADD_TOAST, REMOVE_TOAST } from "../mutations/constants";

/*export function addToast(options = {}) {
  return {
    payload: createToast(options),
    type: ADD_TOAST
  };
}*/

export function addToast(options = {}) {
    return async () => {
        return {
            payload: createToast(options),
            type: ADD_TOAST
        };
    };
  }

export function removeToast(id) {
  return {
    payload: id,
    type: REMOVE_TOAST
  };
}