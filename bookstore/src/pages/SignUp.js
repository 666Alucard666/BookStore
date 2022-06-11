import React, { useState, useEffect } from "react";

import { postSignUp } from "../api/api";

import Preloader from "../components/Preloader";

import Button from "../components/Button";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@mui/styles";
import Grid from "@material-ui/core/Grid";

import { ValidatorForm, TextValidator } from "react-material-ui-form-validator";
import Alert from "../components/Alert";
import { MDBContainer, MDBCard, MDBCardBody, MDBCardHeader } from "mdbreact";
import { Link } from "react-router-dom";

const useStyles = makeStyles(() => ({
  root: {},
  paper: {
    margin: "1rem auto",
    display: "flex",
    flexDirection: "column",
    textAlign: "center",
  },
  body: {
    padding: 0,
  },
  card: {
    padding: "2rem",
    margin: "auto",
    borderRadius: "1.3rem",
    minWidth: "290px",
    maxWidth: "40rem",
  },
  row: {
    marginRight: "-200px",
    marginLeft: "-200px",
  },
  form: {
    "& input": {
      width: "100%",
    },
  },
  toggleButtonGroup: {
    textAlign: "center",
    display: "block",
    width: "100 %",
    margin: "1rem auto",
  },
}));

const SignUp = () => {
  const classes = useStyles();
  const [alertProps, setAlertProps] = useState({
    open: false,
    severity: "info",
    message: "",
    redirectPath: undefined,
  });
  const [formData, setFormData] = useState({
    name: "",
    email: "",
    phone: "",
    password: "",
    username: "",
    surname: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [disabled, setDisabled] = useState(false);

  useEffect(() => {
    ValidatorForm.addValidationRule("isPasswordMatch", (value) => {
      if (value !== formData.password) {
        return false;
      }
      return true;
    });
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
    ValidatorForm.addValidationRule("isPhoneNumber", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /^\+[\d]+$/;
      if (value.match(regexp) && value.length === 13) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isPhoneNumber", (value) => {
      if (value === "") {
        return true;
      }
      var regexp = /^\+[\d]+$/;
      if (value.match(regexp) && value.length === 13) {
        return true;
      }
      return false;
    });

    return () => {
      ValidatorForm.removeValidationRule("isPasswordMatch");
      ValidatorForm.removeValidationRule("isHaveOneDigit");
      ValidatorForm.removeValidationRule("isHaveLowerCase");
      ValidatorForm.removeValidationRule("isHaveUpperCase");
      ValidatorForm.removeValidationRule("isHaveSixCharacters");
      ValidatorForm.removeValidationRule("isHaveNonAlphaNumericChars");
      ValidatorForm.removeValidationRule("isPhoneNumber");
    };
  });

  const handleChange = (event) => {
    setFormData({ ...formData, [event.target.name]: event.target.value });
  };
  const handleSubmit = async (event) => {
    let isOk = false;
    event.persist();
    event.preventDefault();
    setIsLoading(true);

    await postSignUp(
      formData.name,
      formData.email,
      formData.phone,
      formData.password,
      formData.surname,
      formData.username,
    )
      .then((res) => {
        isOk = res.ok;
        setIsLoading(false);
        setDisabled(true);
        return res.json();
      })
      .then(() => {
        if (isOk) {
          setAlertProps({
            open: true,
            severity: "success",
            message: "Time to login)",
            redirectPath: "/SignIn",
          });
        } else {
          setAlertProps({
            open: true,
            severity: "error",
            message: "Inavalid data",
            redirectPath: undefined,
          });
          setDisabled(false);
        }
      });
  };

  return (
    <React.Fragment>
      <MDBContainer>
        <MDBCard className={classes.card}>
          <MDBCardBody>
            <MDBCardHeader>
              <Typography component="h1" variant="h5" align="center">
                Sign up
              </Typography>
            </MDBCardHeader>
            <ValidatorForm onSubmit={handleSubmit} className={classes.form}>
              <div>
                <Grid container direction="column" justifyContent="center" alignItems="center">
                  <Grid item className={classes.customInput}>
                    <TextValidator
                      label="Name"
                      name="name"
                      size="small"
                      onChange={handleChange}
                      variant="outlined"
                      value={formData.name}
                      margin="normal"
                    />
                  </Grid>
                  <Grid item className={classes.customInput}>
                    <TextValidator
                      label="Surname"
                      name="surname"
                      size="small"
                      onChange={handleChange}
                      variant="outlined"
                      value={formData.surname}
                      margin="normal"
                    />
                  </Grid>
                  <Grid item className={classes.customInput}>
                    <TextValidator
                      label="Username"
                      name="username"
                      size="small"
                      onChange={handleChange}
                      variant="outlined"
                      value={formData.username}
                      validators={["required", "isHaveOneDigit"]}
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
                  <Grid item className={classes.customInput}>
                    <TextValidator
                      label="Email"
                      name="email"
                      size="small"
                      onChange={handleChange}
                      variant="outlined"
                      value={formData.email}
                      validators={["required", "isEmail"]}
                      errorMessages={["RequiredField", "Invalid Email"]}
                      margin="normal"
                    />
                  </Grid>
                  <Grid item className={classes.customInput}>
                    <TextValidator
                      label="PhoneNumber"
                      name="phone"
                      size="small"
                      onChange={handleChange}
                      variant="outlined"
                      value={formData.phone}
                      margin="normal"
                      validators={["required", "isPhoneNumber"]}
                      errorMessages={["RequiredField", "Invalid PhoneNumber"]}
                    />
                  </Grid>
                  {isLoading ? <Preloader /> : <Button className="button--cart">Sign Up</Button>}
                  <Grid container direction="row" justifyContent="center">
                    <Link to="/SignIn" variant="body2">
                      Have an account?
                    </Link>
                  </Grid>
                </Grid>
              </div>
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

export default SignUp;
