const initialState = {
  token: window.localStorage.getItem("token"),
  userId: null,
  cancelData: null,
  role: null,
};

const accountReducer = (state = initialState, { type, payload }) => {
  switch (type) {
    case "LOGIN":
      return {
        ...initialState,
        token: payload?.token,
        userId: payload?.userId,
        cancelData: payload?.cancelData,
        role: payload?.role,
      };
    case "LOGOUT":
      window.localStorage.removeItem("persist:root");
      return {
        ...initialState,
        token: null,
        userId: null,
        cancelData: null,
        role:null,
      };
    case "REFRESH_TOKEN":
      console.log(payload);
      window.localStorage.setItem("token", payload?.token);
      return {
        ...initialState,
        token: payload?.token,
        cancelData: payload?.cancelDate,
        userId: state.userId,
        role:state.role
      };
    default:
      return state;
  }
};
export default accountReducer;
