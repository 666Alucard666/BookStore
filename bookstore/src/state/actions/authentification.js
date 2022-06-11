export const signOut = () => {
  return (dispatch) => {
    dispatch({
      type: "LOGOUT",
    });
  };
};

export const refreshedToken = (data) => ({
  type: "REFRESH_TOKEN",
  payload: data,
});
