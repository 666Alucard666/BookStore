import {
  Box,
  Grid,
  makeStyles,
  MenuItem,
  TextField,
  Button,
  CircularProgress,
  Menu,
} from "@material-ui/core";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import { updateShop, createShop, getShopById, deleteShop } from "../../api/api";
import { isNil } from "lodash";

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

export default function EditShop() {
  const classes = useStyles();
  const history = useNavigate();
  const { shopId } = useParams();
  const [isLoading, setIsLoading] = React.useState(false);
  const [shop, setShop] = React.useState({
    name: "",
    region: "",
    startWorkingHours: 0,
    endWorkingHours: 0,
    size: "",
    city: "",
    address: "",
    orders: [],
  });

  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  React.useEffect(() => {
    if (!isNil(shopId)) {
      setIsLoading(true);
      getShopById(shopId).then((x) => {
        setShop(x.data);
        setIsLoading(false);

      });
    }
  }, []);

  const handleChange = (event) => {
    setShop({ ...shop, [event.target.name]: event.target.value });
  };


  const onSave = () => {
    delete shop.orders;
    isNil(shopId)
      ? createShop(shop)
      : updateShop(shop)
    history("/manageShops");
  };

  const onDelete = () => {
    !isNil(shopId) && deleteShop(shopId);
    history("/manageShops");
  };
  return (
    <>
      <div className="container">
        <div className="main-content">
          {isLoading ? (
            <CircularProgress />
          ) : (
            <div className="main-content__info">
              <h1>{`Manage ${isNil(shopId) ? "New Worker" : shop?.name}`}</h1>
              <Box component="form" sx={{ marginLeft: "200px", marginTop: "50px" }}>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.name}
                    onChange={handleChange}
                    label="Name"
                    name="name"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.size}
                    onChange={handleChange}
                    label="Size"
                    name="size"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.startWorkingHours}
                    onChange={handleChange}
                    label="Start Working Hours"
                    name="startWorkingHours"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.endWorkingHours}
                    onChange={handleChange}
                    label="End Working Hours"
                    name="endWorkingHours"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.city}
                    onChange={handleChange}
                    label="City"
                    name="city"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.region}
                    onChange={handleChange}
                    label="Region"
                    name="region"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={shop?.address}
                    onChange={handleChange}
                    label="Address"
                    name="address"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                {shop?.orders.length !== 0 && <Grid item style={{ marginTop: "20px" }}>
                  <Button
                    id="basic-button"
                    aria-controls={open ? "basic-menu" : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? "true" : undefined}
                    onClick={handleClick}>
                    Orders
                  </Button>
                  <Menu
                    id="basic-menu"
                    anchorEl={anchorEl}
                    open={open}
                    onClose={handleClose}
                    MenuListProps={{
                      "aria-labelledby": "basic-button",
                    }}>
                    {(shop.orders || []).map(x => <MenuItem onClick={handleClose}>{x.orderId}</MenuItem>)}
                  </Menu>
                </Grid>}

                <Grid item>
                  <Button
                    variant="contained"
                    onClick={onSave}
                    style={{ marginTop: "20px", marginLeft: "30px" }}>
                    Save
                  </Button>
                  {!isNil(shopId) && (
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
