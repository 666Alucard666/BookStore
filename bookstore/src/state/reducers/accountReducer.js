const initialState = {
  token: window.localStorage.getItem("token"),
  userId: null,
  cancelData: null,
};

const accountReducer = (state = initialState, { type, payload }) => {
  switch (type) {
    case "LOGIN":
      return {
        ...initialState,
        token: payload?.token,
        userId: payload?.userId,
        cancelData: payload?.cancelData,
      };
    case "LOGOUT":
      window.localStorage.removeItem("persist:root");
      return {
        token: null,
        userId: null,
        cancelData: null,
      };
    default:
      return state;
  }
};
export default accountReducer;
