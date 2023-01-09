import { combineReducers } from "redux";
import accountReducer from "./accountReducer";
import bookReducer from "./bookReducer";
import cartReducer from "./cartReducer";

const rootReducer = combineReducers({
  account: accountReducer,
  books: bookReducer,
  cart: cartReducer,
});

export default rootReducer;
