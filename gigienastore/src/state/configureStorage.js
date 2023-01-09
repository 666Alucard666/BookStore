import { createStore } from "redux";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage"; // defaults to localStorage for web
import rootReducer from "./reducers/rootReducer";
import thunk from "redux-thunk";
import { applyMiddleware, compose } from "redux";

const persistConfig = {
  key: "root",
  storage,
};

const persistedReducer = persistReducer(persistConfig, rootReducer);
const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
export default () => {
  let store = createStore(persistedReducer, composeEnhancers(applyMiddleware(thunk)));
  let persistor = persistStore(store);
  return { store, persistor };
};
