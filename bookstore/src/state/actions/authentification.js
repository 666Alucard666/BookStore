export const signOut = () => {
  return (dispatch) => {
    dispatch({
      type: "LOGOUT",
    });
  };
};
