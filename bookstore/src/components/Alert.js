import React from "react";
import Snackbar from "@material-ui/core/Snackbar";
import MuiAlert from "@mui/material/Alert";
import { makeStyles } from "@mui/styles";
import { useNavigate } from "react-router-dom";
import Slide from "@material-ui/core/Slide";
import PropTypes from "prop-types";

const useStyles = makeStyles((theme) => ({
  root: {
    width: "100%",
    "& > * + *": {},
  },
}));

const CustomAlert = React.forwardRef((props, ref) => {
  return <MuiAlert ref={ref} elevation={6} variant="filled" {...props} />;
});

const slideTransition = (props) => {
  return <Slide {...props} direction="down" />;
};

const Alert = (props) => {
  const classes = useStyles();
  const history = useNavigate();
  const vertical = "top";
  const horizontal = "center";
  const enter = 500;
  const exit = 500;

  const handleClose = (event, reason) => {
    if (reason === "clickaway") {
      return;
    }
    props.handleClose();
    if (props.redirectPath !== undefined) {
      setTimeout(function () {
        history(props.redirectPath);
      }, exit);
    }
  };

  return (
    <div className={classes.root}>
      <Snackbar
        open={props.open}
        autoHideDuration={3000}
        anchorOrigin={{ vertical, horizontal }}
        onClose={handleClose}
        transitionDuration={{ enter: enter, exit: exit }}
        TransitionComponent={slideTransition}>
        <CustomAlert onClose={handleClose} severity={props.severity}>
          {props.message}
        </CustomAlert>
      </Snackbar>
    </div>
  );
};

Alert.propTypes = {
  open: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
  redirectPath: PropTypes.string,
  updateLoginStatus: PropTypes.func,
  severity: PropTypes.oneOf(["error", "warning", "info", "success"]).isRequired,
  message: PropTypes.string.isRequired,
};

export default Alert;
