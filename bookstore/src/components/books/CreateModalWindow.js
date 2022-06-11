import { Modal } from "@mui/material";
import React from "react";
import { ModalBody, ModalHeader } from "react-bootstrap";
import { makeStyles } from "@mui/styles";
import { ValidatorForm, TextValidator } from "react-material-ui-form-validator";
import Grid from "@material-ui/core/Grid";
import Button from "../Button";
import Typography from "@mui/material/Typography";
import { useDispatch, useSelector } from "react-redux";
import { Paper } from "material-ui";
import MuiThemeProvider from "material-ui/styles/MuiThemeProvider";
import { createBookRequest } from "../../api/api";
const useStyles = makeStyles(() => ({
  root: {
    display: "flex",
    flexDirection: "row-reverse",
  },
  paper: {
    marginLeft: "auto",
    display: "flex",
    flexDirection: "column",
    textAlign: "center",
  },
  body: {
    padding: 0,
  },
  card: {
    padding: "2rem",
    marginLeft: "600px",
    marginTop: "100px",
    borderRadius: "10px",
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

export default function CreateModalWindow() {
  const classes = useStyles();
  const open = useSelector((state) => state.books.openCreate);
  const dispatch = useDispatch();
  const handleCreateSubmit = async (event) => {
    event.preventDefault();
    event.stopPropagation();

    dispatch({
      type: "ACTION_WITH_CREATE_MODAL",
      payload: false,
    });
    createBookRequest({
      id: bookForm.id,
      name: bookForm.name,
      genre: bookForm.genre,
      author: bookForm.author,
      price: +bookForm.price,
      publishing: bookForm.publishing,
      amountOnStore: +bookForm.amountOnStore,
      image: bookForm.image,
      created: bookForm.created,
    });
    setBookForm({
      ...bookForm,
      id: 0,
      name: "",
      genre: "",
      author: "",
      price: 0.0,
      publishing: "",
      amountOnStore: 0,
      image: "",
      created: undefined,
    });
  };
  const handleExitSubmit = async (event) => {
    dispatch({
      type: "ACTION_WITH_MODAL",
      payload: false,
    });
    setBookForm({
      ...bookForm,
      id: 0,
      name: "",
      genre: "",
      author: "",
      price: 0.0,
      publishing: "",
      amountOnStore: 0,
      image: "",
      created: undefined,
    });
  };
  const [bookForm, setBookForm] = React.useState({
    id: 0,
    name: "",
    genre: "",
    author: "",
    price: 0.0,
    publishing: "",
    amountOnStore: 0,
    image: "",
    created: undefined,
  });
  React.useEffect(() => {
    ValidatorForm.addValidationRule("isHaveLowerCase", (value) => {
      var regexp = /(?=.*[a-z])|(?=.*[а-я])/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveUpperCase", (value) => {
      var regexp = /(?=.*[A-Z])|(?=.*[А-Я])/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveLetters", (value) => {
      var regexp = /^[A-Za-z\s]*$/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isURL", (value) => {
      var regexp = /^(https?:\/\/)?/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isPrice", (value) => {
      var regexp = /[0-9]*\.[0-9]+/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    ValidatorForm.addValidationRule("isHaveNumbers", (value) => {
      var regexp = /[0-9]+/;
      if (value.match(regexp)) {
        return true;
      }
      return false;
    });
    return () => {
      ValidatorForm.removeValidationRule("isHaveLetters");
      ValidatorForm.removeValidationRule("isHaveUpperCase");
      ValidatorForm.removeValidationRule("isHaveLowerCase");
      ValidatorForm.removeValidationRule("isURL");
      ValidatorForm.removeValidationRule("isPrice");
      ValidatorForm.removeValidationRule("isHaveNumbers");
    };
  });
  const handleChange = (event) => {
    console.log(event.target.value);
    setBookForm({
      ...bookForm,
      [event.target.name]: event.target.value,
    });
  };
  return (
    <MuiThemeProvider>
      <Modal open={open} sx={{ width: "300px", borderRadius: "10px" }}>
        <div>
          <Paper className={classes.card}>
            <ModalHeader>
              <Typography component="h1" variant="h5" align="center">
                Create book
              </Typography>
            </ModalHeader>
            <ModalBody>
              <ValidatorForm onSubmit={handleCreateSubmit} className={classes.form}>
                <div>
                  <Grid container direction="column" justifyContent="center" alignItems="center">
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Title"
                        name="name"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.name}
                        margin="normal"
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Genre"
                        name="genre"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.genre}
                        margin="normal"
                        validators={["isHaveLetters", "isHaveLowerCase", "isHaveUpperCase"]}
                        errorMessages={["Invalid genre!"]}
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Author"
                        name="author"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.author}
                        margin="normal"
                        validators={["isHaveLetters", "isHaveLowerCase", "isHaveUpperCase"]}
                        errorMessages={["Invalid author!"]}
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Publishing"
                        name="publishing"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.publishing}
                        margin="normal"
                        validators={["isHaveUpperCase"]}
                        errorMessages={["Invalid publishing!"]}
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Image link"
                        name="image"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.image}
                        validators={["isURL"]}
                        errorMessages={["Invalid URL"]}
                        margin="normal"
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Amount on store"
                        name="amountOnStore"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.amountOnStore}
                        validators={["isHaveNumbers"]}
                        errorMessages={["Invalid number"]}
                        margin="normal"
                      />
                    </Grid>
                    <Grid item className={classes.customInput}>
                      <TextValidator
                        label="Price"
                        name="price"
                        size="small"
                        onChange={handleChange}
                        variant="outlined"
                        value={bookForm.price}
                        validators={["isPrice"]}
                        errorMessages={["Invalid price"]}
                        margin="normal"
                      />
                    </Grid>
                    <div className={classes.root}>
                      <Button className="button--edit">
                        <span className={classes.toggleButtonGroup}>Create</span>
                      </Button>
                      <Button className="button--edit">
                        <span className={classes.toggleButtonGroup} onClick={handleExitSubmit}>
                          Exit
                        </span>
                      </Button>
                    </div>
                  </Grid>
                </div>
              </ValidatorForm>
            </ModalBody>
          </Paper>
        </div>
      </Modal>
    </MuiThemeProvider>
  );
}
