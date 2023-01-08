import React from "react";
import CartItem from "../components/CartItem";
import { makeStyles } from "@mui/styles";
import { ValidatorForm, TextValidator } from "react-material-ui-form-validator";
import Dialog from "@mui/material/Dialog";
import Grid from "@material-ui/core/Grid";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { useDispatch, useSelector } from "react-redux";
import {
  clearCart,
  minusCartItem,
  plusCartItem,
  removeCartItem,
} from "../state/actions/cartAction";
import emptyCard from "../assets/img/empty-cart.png";
import { Button } from "../components";
import { postOrder, getCustomerById, getShopsByCity } from "../api/api";
import { signOut } from "../state/actions/authentification";
import { useNavigate } from "react-router-dom";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import InputLabel from "@mui/material/InputLabel";
import { Box, Checkbox, FormControl, Typography } from "@mui/material";

const useStyles = makeStyles(() => ({
  root: {
    height: "300px",
    width: "400px",
    marginLeft: "540px",
  },
  form: {
    width: "300px",
    "& input": {
      width: "220px",
    },
  },
  card: {
    minWidth: "290px",
    borderRadius: "1rem",
    marginBottom: "170px",
  },
}));

const paymentTypes = ["Cash", "Credit card", "Crypto"];

function Cart() {
  const classes = useStyles();
  const [open, setOpen] = React.useState(false);
  const dispatch = useDispatch();
  const cart = useSelector((state) => state.cart);
  const books = Object.keys(cart.items).map((k) => {
    return cart.items[k].items[0];
  });
  const [customer, setCustomer] = React.useState({});
  const [shops, setShops] = React.useState([]);
  const navigate = useNavigate();
  const [intoShop, setIntoShop] = React.useState(false);
  const userId = useSelector((state) => state.account.userId);
  const [orderData, setOrderData] = React.useState({
    recipientPhone: "",
    customerId: userId,
    recipientName: "",
    recipientSurname: "",
    recipientCity: "",
    recipientAdress: "",
    sum: cart.totalPrice,
    shopId: null,
    paymentType: paymentTypes[0],
    productsList: [],
  });

  React.useEffect(() => {
    getCustomerById(userId).then((res) => {
      setCustomer(res.data);
      setOrderData({
        ...orderData,
        recipientPhone: res.data.phone,
        recipientName: res.data.name,
        recipientSurname: res.data.surname,
        recipientCity: res.data.city,
        recipientAdress: res.data.address,
      });
    });
  }, [userId]);

  React.useEffect(() => {
    getShopsByCity(orderData.recipientCity).then((res) => setShops(res.data));
  }, [orderData.recipientCity]);

  const handleClearCart = () => {
    if (window.confirm("Do you really want clear your cart?")) {
      dispatch(clearCart());
    }
  };

  const onRemoveItem = (id) => {
    if (window.confirm("Do u really wanna delete?")) {
      dispatch(removeCartItem(id));
    }
  };

  const onPlusItem = (id) => {
    dispatch(plusCartItem(id));
  };

  const onMinusItem = (id) => {
    dispatch(minusCartItem(id));
  };
  const onOpen = (event) => {
    event.preventDefault();
    event.stopPropagation();

    setOrderData({
      ...orderData,
      productsList: books.map((b) => {
        return {
          productId: b.productId,
          count: cart.items[b.productId].items.length,
        };
      }),
    });
    setOpen(true);
  };
  const onClickOrder = async (event) => {
    event.preventDefault();
    event.stopPropagation();

    postOrder(orderData)
      .catch((err) => {
        if (err.response.data === undefined) {
          dispatch(signOut());
          navigate("/signIn");
        }
      })
      .then((res) => {
        if (res.status === 200) {
          dispatch(clearCart());
          setOrderData({
            recipientPhone: "",
            customerId: userId,
            recipientName: "",
            recipientSurname: "",
            recipientCity: "",
            recipientAdress: "",
            sum: cart.totalPrice,
            shopId: null,
            paymentType: paymentTypes[0],
            productsList: [],
          });
          navigate("/home");
        }
      });
  };
  const handleChange = async (event) => {
    setOrderData({ ...orderData, [event.target.name]: event.target.value });
  };
  const handleClose = () => {
    setOrderData({
      recipientPhone: "",
      customerId: userId,
      recipientName: "",
      recipientSurname: "",
      recipientCity: "",
      recipientAdress: "",
      sum: cart.totalPrice,
      shopId: null,
      paymentType: paymentTypes[0],
      productsList: [],
    });
    setOpen(false);
  };

  React.useEffect(() => {
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
      ValidatorForm.removeValidationRule("isPhoneNumber");
    };
  });

  return (
    <div className="content">
      <div className="container container--cart">
        {cart.totalCount ? (
          <div className="cart">
            <div className="cart__top">
              <h2 className="content__title">
                <svg
                  width="18"
                  height="18"
                  viewBox="0 0 18 18"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M6.33333 16.3333C7.06971 16.3333 7.66667 15.7364 7.66667 15C7.66667 14.2636 7.06971 13.6667 6.33333 13.6667C5.59695 13.6667 5 14.2636 5 15C5 15.7364 5.59695 16.3333 6.33333 16.3333Z"
                    stroke="white"
                    strokeWidth="1.8"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M14.3333 16.3333C15.0697 16.3333 15.6667 15.7364 15.6667 15C15.6667 14.2636 15.0697 13.6667 14.3333 13.6667C13.597 13.6667 13 14.2636 13 15C13 15.7364 13.597 16.3333 14.3333 16.3333Z"
                    stroke="white"
                    strokeWidth="1.8"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M4.78002 4.99999H16.3334L15.2134 10.5933C15.1524 10.9003 14.9854 11.176 14.7417 11.3722C14.4979 11.5684 14.1929 11.6727 13.88 11.6667H6.83335C6.50781 11.6694 6.1925 11.553 5.94689 11.3393C5.70128 11.1256 5.54233 10.8295 5.50002 10.5067L4.48669 2.82666C4.44466 2.50615 4.28764 2.21182 4.04482 1.99844C3.80201 1.78505 3.48994 1.66715 3.16669 1.66666H1.66669"
                    stroke="white"
                    strokeWidth="1.8"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
                Cart
              </h2>
              <div className="cart__clear" onClick={() => handleClearCart()}>
                <svg
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg">
                  <path
                    d="M2.5 5H4.16667H17.5"
                    stroke="#B6B6B6"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M6.66663 5.00001V3.33334C6.66663 2.89131 6.84222 2.46739 7.15478 2.15483C7.46734 1.84227 7.89127 1.66667 8.33329 1.66667H11.6666C12.1087 1.66667 12.5326 1.84227 12.8451 2.15483C13.1577 2.46739 13.3333 2.89131 13.3333 3.33334V5.00001M15.8333 5.00001V16.6667C15.8333 17.1087 15.6577 17.5326 15.3451 17.8452C15.0326 18.1577 14.6087 18.3333 14.1666 18.3333H5.83329C5.39127 18.3333 4.96734 18.1577 4.65478 17.8452C4.34222 17.5326 4.16663 17.1087 4.16663 16.6667V5.00001H15.8333Z"
                    stroke="#B6B6B6"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M8.33337 9.16667V14.1667"
                    stroke="#B6B6B6"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M11.6666 9.16667V14.1667"
                    stroke="#B6B6B6"
                    strokeWidth="1.2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
                <span>Clear cart</span>
              </div>
            </div>
            <div className="content__items">
              {books.map((book) => {
                return (
                  <CartItem
                    book={book}
                    totalCount={cart.items[book.productId].items.length}
                    totalPrice={cart.items[book.productId].totalPrice.toFixed(2)}
                    onRemove={onRemoveItem}
                    onMinus={onMinusItem}
                    onPlus={onPlusItem}
                  />
                );
              })}
              <Dialog open={open} onClose={handleClose}>
                <DialogTitle textAlign={"center"}>Order Form</DialogTitle>
                <DialogContent>
                  <ValidatorForm className={classes.form}>
                    <Grid container direction="column" justifyContent="center" alignItems="center">
                      <Grid item className={classes.customInput}>
                        <TextValidator
                          label="Phone number"
                          name="recipientPhone"
                          size="small"
                          onChange={handleChange}
                          variant="outlined"
                          value={orderData.recipientPhone}
                          margin="normal"
                          validators={["required", "isPhoneNumber"]}
                          errorMessages={["RequiredField", "Invalid phone number"]}
                        />
                      </Grid>
                      <Grid item className={classes.customInput}>
                        <TextValidator
                          label="Recipient name"
                          name="recipientName"
                          size="small"
                          onChange={handleChange}
                          variant="outlined"
                          value={orderData.recipientName}
                          margin="normal"
                        />
                      </Grid>
                      <Grid item className={classes.customInput}>
                        <TextValidator
                          label="Recipient surname"
                          name="recipientSurname"
                          size="small"
                          onChange={handleChange}
                          variant="outlined"
                          value={orderData.recipientSurname}
                          margin="normal"
                        />
                      </Grid>
                      <Grid item className={classes.customInput}>
                        <TextValidator
                          label="Recipient city"
                          name="recipientCity"
                          size="small"
                          onChange={handleChange}
                          variant="outlined"
                          value={orderData.recipientCity}
                          margin="normal"
                        />
                      </Grid>
                      <Grid item className={classes.customInput}>
                        <TextValidator
                          label="Recipient address"
                          name="recipientAdress"
                          size="small"
                          onChange={handleChange}
                          variant="outlined"
                          value={orderData.recipientAdress}
                          margin="normal"
                        />
                      </Grid>
                      <Box sx={{ minWidth: 250, marginTop: "10px" }}>
                        <FormControl fullWidth>
                          <InputLabel id="demo-simple-select-label">Payment type</InputLabel>
                          <Select
                            labelId="demo-simple-select-label"
                            value={orderData.paymentType}
                            name="paymentType"
                            onChange={handleChange}
                            size="small">
                            {paymentTypes.map((type) => (
                              <MenuItem value={type}>{type}</MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </Box>
                      <Typography>Use shop for shipping:</Typography>
                      <Checkbox
                        checked={intoShop}
                        onChange={(event) => setIntoShop(event.target.checked)}></Checkbox>
                      {intoShop && (
                        <Box sx={{ minWidth: 250, marginBottom: "10px"}}>
                          <FormControl fullWidth>
                            <InputLabel id="demo-simple-select-label">Shops</InputLabel>
                            <Select
                              labelId="demo-simple-select-label"
                              value={orderData.shopId}
                              name="shopId"
                              onChange={handleChange}
                              size="small">
                              {shops.length === 0 ? (
                                <MenuItem value={null}>None</MenuItem>
                              ) : (
                                shops.map((shop) => (
                                  <MenuItem value={shop.shopId}>{shop.name}</MenuItem>
                                ))
                              )}
                            </Select>
                          </FormControl>
                        </Box>
                      )}
                    </Grid>
                    <div style={{ display: "flex", justifyContent: "space-between" }}>
                      <Button onClick={handleClose}>Cancel</Button>
                      <Button onClick={onClickOrder} className="pay-btn">
                        <span>Make order</span>
                      </Button>
                    </div>
                  </ValidatorForm>
                </DialogContent>
              </Dialog>
            </div>
            <div className="cart__bottom">
              <div className="cart__bottom-details">
                <span>
                  {" "}
                  Total products: <b>{cart.totalCount} pc.</b>{" "}
                </span>
                <span>
                  {" "}
                  Order sum: <b>{cart.totalPrice.toFixed(2)}$</b>{" "}
                </span>
              </div>
              <div className="cart__bottom-buttons">
                <a href="/home" className="button button--outline button--add go-back-btn">
                  <svg
                    width="8"
                    height="14"
                    viewBox="0 0 8 14"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg">
                    <path
                      d="M7 13L1 6.93015L6.86175 1"
                      stroke="#D3D3D3"
                      strokeWidth="1.5"
                      strokeLinecap="round"
                      strokeLinejoin="round"
                    />
                  </svg>

                  <span>Return</span>
                </a>
                <Button onClick={onOpen} className="pay-btn">
                  <span>Create order</span>
                </Button>
              </div>
            </div>
          </div>
        ) : (
          <div className="container container--cart">
            <div className="cart cart--empty">
              <h2>
                Empty cart <icon>😕</icon>
              </h2>
              <p>
                Seems like u haven't added products yet
                <br />
                Comeback to main page for resolve this.
              </p>
              <img src={emptyCard} alt="Empty cart" />
              <a href="/home" className="button button--black">
                <span>Return</span>
              </a>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default Cart;
