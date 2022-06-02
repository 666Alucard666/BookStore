import React, { useState, useEffect } from "react";
import { postSignIn } from "../api/api";
import Preloader from "../components/Preloader";
import Grid from "@material-ui/core/Grid";
import { makeStyles } from "@mui/styles";
import { ValidatorForm, TextValidator } from "react-material-ui-form-validator";
import Alert from "../components/Alert";
import { MDBContainer, MDBCard, MDBCardBody, MDBCardHeader } from "mdbreact";
import Button from "../components/Button";
import Typography from "@mui/material/Typography";
import { Link } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";

const useStyles = makeStyles(() => ({
  root: {
    height: "300px",
    width: "400px",
    marginLeft: "540px",
  },
  form: {
    width: "250px",
    "& input": {
      width: "250px",
    },
  },
  card: {
    minWidth: "290px",
    borderRadius: "1rem",
    marginBottom: "170px",
  },
}));

const SignIn = () => {
  const dispatch = useDispatch();
  const account = useSelector((state) => state.account);
  const classes = useStyles();
  const [alertProps, setAlertProps] = useState({
    open: false,
    severity: "info",
    message: "",
    redirectPath: undefined,
  });
  const [formData, setFormData] = useState({
    login: "",
    password: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [signInDisabled, setSignInDisabled] = useState(false);

  const handleChange = async (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };

  const handleSignInSubmit = async (event) => {
    event.preventDefault();
    event.stopPropagation();

    setIsLoading(true);
    dispatch(postSignIn(formData.login, formData.password));
    setIsLoading(false);
  };
  useEffect(() => {
    if (account.userId !== null && account.userId !== undefined) {
      setSignInDisabled(true);
      setAlertProps({
        open: true,
        severity: "success",
        message: "You are logined!",
        redirectPath: "/home",
      });
    } else {
      setAlertProps({
        open: true,
        severity: "error",
        message: "You are not logined",
        redirectPath: undefined,
      });
      setSignInDisabled(false);
    }
  }, [account, signInDisabled]);

  useEffect(() => {
    ValidatorForm.addValidationRule("isHaveOneDigit", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /(?=.*\d)/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveLowerCase", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /(?=.*[a-z])|(?=.*[а-я])/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveUpperCase", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /(?=.*[A-Z])|(?=.*[А-Я])/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveSixCharacters", (value) => {
      if (value === "") {
        return true;
      }
      if (value.length > 5) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveNonAlphaNumericChars", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /[^a-zA-Z0-9]+/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });

    return () => {
      ValidatorForm.removeValidationRule("isHaveOneDigit");
      ValidatorForm.removeValidationRule("isHaveLowerCase");
      ValidatorForm.removeValidationRule("isHaveUpperCase");
      ValidatorForm.removeValidationRule("isHaveSixCharacters");
      ValidatorForm.removeValidationRule("isHaveNonAlphaNumericChars");
    };
  }, []);

  return (
    <React.Fragment>
      <MDBContainer className={classes.root}>
        <MDBCard className={classes.card}>
          <MDBCardBody>
            <MDBCardHeader>
              <Typography component="h1" variant="h5" marginLeft="100px">
                Sign In
              </Typography>
            </MDBCardHeader>
            <ValidatorForm onSubmit={handleSignInSubmit} className={classes.form}>
              <Grid container direction="column" justifyContent="center" alignItems="center">
                <Grid item className={classes.customInput}>
                  <TextValidator
                    label="Login"
                    name="login"
                    size="small"
                    onChange={handleChange}
                    variant="outlined"
                    value={formData.login}
                    validators={["required", "isHaveSixCharacters"]}
                    errorMessages={["Input here!", "Uncorrect Login"]}
                    margin="normal"
                  />
                </Grid>
                <Grid item className={classes.customInput}>
                  <TextValidator
                    margin="normal"
                    type="password"
                    label="Password"
                    name="password"
                    size="small"
                    onChange={handleChange}
                    variant="outlined"
                    value={formData.password}
                    validators={[
                      "required",
                      "isHaveLowerCase",
                      "isHaveUpperCase",
                      "isHaveOneDigit",
                      "isHaveNonAlphaNumericChars",
                      "isHaveSixCharacters",
                    ]}
                    errorMessages={[
                      "Input here!",
                      "At least one small letter is required",
                      "At least one capital letter is required",
                      "At least one digit is required",
                      "At least one special character is required",
                      "Password must be at least 6 characters",
                    ]}
                  />
                </Grid>
                {isLoading || signInDisabled ? (
                  <Preloader />
                ) : (
                  <Button className="button--cart">Sign In</Button>
                )}
                <Grid container direction="row" justifyContent="center">
                  {!signInDisabled && (
                    <Link to="/SignUp" variant="body2">
                      Sign Up
                    </Link>
                  )}
                </Grid>
              </Grid>
            </ValidatorForm>
          </MDBCardBody>
        </MDBCard>
      </MDBContainer>
      <Alert
        open={alertProps.open}
        handleClose={() => setAlertProps({ ...alertProps, open: false })}
        redirectPath={alertProps.redirectPath}
        severity={alertProps.severity}
        message={alertProps.message}
      />
    </React.Fragment>
  );
};

export default SignIn;
