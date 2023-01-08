import {
  Box,
  Grid,
  makeStyles,
  MenuItem,
  OutlinedInput,
  Select,
  TextField,
  Typography,
  FormControl,
  Button,
  CircularProgress,
} from "@material-ui/core";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getShopsDropDown, getWorkerById, crudWorker } from "../../api/api";
import { isNil } from "lodash";
import { DesktopDatePicker, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";

const useStyles = makeStyles((theme) => ({
  root: {
    padding: "0 5px",
  },
  filtersContainer: {
    marginTop: "10px",
    display: "flex",
    justifyContent: "center",
  },
  filter: {
    width: "200px",
  },
  fieldLabelNot: {
    color: "#737373",
  },
  fieldLabel: {
    backgroundColor: "#BEBEBE",
  },
  fieldLabelNit: {
    backgroundColor: "#FFFFFF",
  },
}));

const specialties = ["Administrator", "Shop Assistant", "Cleaner", "Manager"];

export default function EditWorker() {
  const classes = useStyles();
  const history = useNavigate();
  const { workerId } = useParams();
  const [isLoading, setIsLoading] = React.useState(false);
  const [worker, setWorker] = React.useState({
    name: "",
    surname: "",
    middleName: "",
    sex: "",
    dob: new Date(),
    city: "",
    address: "",
    salary: "",
    specialty: "",
    shopId: "",
    password: "",
    email: "",
  });
  const [shops, setShops] = React.useState([]);
  const [selectedShop, setSelectedShop] = React.useState({
    shopId: null,
    name: "",
  });
  const item = {
    workersToUpdate: [],
    workersToDelete: [],
    workerToCreate: null,
  };

  React.useEffect(() => {
    if (!isNil(workerId)) {
      setIsLoading(true);
      getWorkerById(workerId).then((x) => {
        setWorker(x.data);
        setSelectedShop({ ...selectedShop, shopId: x.data.shopId, name: x.data.shop.name });
      });
    }
    getShopsDropDown().then((x) => {
      setShops(x.data);
      setIsLoading(false);
    });
  }, []);

  const handleChange = (event) => {
    setWorker({ ...worker, [event.target.name]: event.target.value });
  };

  const onSave = () => {
    isNil(workerId)
      ? crudWorker({ ...item, workerToCreate: { ...worker, shopId: selectedShop.shopId } })
      : crudWorker({
          ...item,
          workersToUpdate: [...item.workersToUpdate, { ...worker, shopId: selectedShop.shopId }],
        });
    history("/manageWorkers");
  };

  const onDelete = () => {
    !isNil(workerId) &&
      crudWorker({ ...item, workersToDelete: [...item.workersToDelete, workerId] });
    history("/manageWorkers");
  };
  console.log(selectedShop);
  return (
    <>
      <div className="container">
        <div className="main-content">
          {isLoading ? (
            <CircularProgress />
          ) : (
            <div className="main-content__info">
              <h1>{`Manage ${isNil(workerId) ? "New Worker" : worker?.name}`}</h1>
              <Box component="form" sx={{ marginLeft: "200px", marginTop: "50px" }}>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.name}
                    onChange={handleChange}
                    label="Name"
                    name="name"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.surname}
                    onChange={handleChange}
                    label="Surname"
                    name="surname"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.middleName}
                    onChange={handleChange}
                    label="MiddleName"
                    name="middleName"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.sex}
                    onChange={handleChange}
                    label="Sex"
                    name="sex"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <LocalizationProvider dateAdapter={AdapterMoment}>
                    <DesktopDatePicker
                      label="Date of birth"
                      inputFormat="DD/MM/YYYY"
                      name="dob"
                      value={worker?.dob}
                      onChange={(val) => setWorker({ ...worker, dob: val })}
                      renderInput={(params) => <TextField {...params} />}
                    />
                  </LocalizationProvider>
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.city}
                    onChange={handleChange}
                    label="City"
                    name="city"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.address}
                    onChange={handleChange}
                    label="Address"
                    name="address"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    type="number"
                    value={worker?.salary}
                    onChange={handleChange}
                    label="Salary"
                    name="salary"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={worker?.email}
                    onChange={handleChange}
                    label="Email"
                    name="email"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    type="password"
                    value={worker?.password}
                    onChange={handleChange}
                    label="Password"
                    name="password"
                    variant="outlined"
                    size="small"
                  />
                </Grid>

                <Grid sx={{ marginTop: "20px" }} item>
                  <FormControl variant="outlined" fullWidth className={classes.filter}>
                    <Typography className={classes.fieldLabelNot}>Specialty</Typography>
                    <Select
                      style={{ maxWidth: "220px" }}
                      labelId="service-line-label"
                      name="specialty"
                      value={worker?.specialty}
                      onChange={handleChange}
                      input={<OutlinedInput name="specialty" size="small" />}>
                      {(specialties || []).map((type) => {
                        return <MenuItem value={type}>{type}</MenuItem>;
                      })}
                    </Select>
                  </FormControl>
                </Grid>
                <Grid sx={{ marginTop: "10px" }} item>
                  <FormControl variant="outlined" fullWidth className={classes.filter}>
                    <Typography className={classes.fieldLabelNot}>Shop</Typography>
                    <Select
                      style={{ maxWidth: "240px" }}
                      labelId="service-line-label"
                      name="shop"
                      value={selectedShop}
                      input={<OutlinedInput name="shop" />}
                      onChange={(event) => {
                        setSelectedShop(event.target.value);
                      }}
                      size="small">
                      {shops.map((type) => (
                        <MenuItem value={type}>{type.name}</MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                </Grid>

                <Grid item>
                  <Button
                    variant="contained"
                    onClick={onSave}
                    style={{ marginTop: "20px", marginLeft: "30px" }}>
                    Save
                  </Button>
                  {!isNil(workerId) && (
                    <Button
                      variant="contained"
                      onClick={onDelete}
                      style={{ marginTop: "20px", marginLeft: "30px" }}>
                      Delete
                    </Button>
                  )}
                </Grid>
              </Box>
            </div>
          )}
        </div>
      </div>
    </>
  );
}
