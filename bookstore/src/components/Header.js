import React from "react";
import { Link, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import moment from "moment";

import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import Tooltip from "@mui/material/Tooltip";

import logoSvg from "../assets/img/books-logo.svg";
import Button from "./Button";
import { refreshedToken, signOut } from "../state/actions/authentification";
import { clearCart } from "../state/actions/cartAction";
import { refreshToken } from "../api/api";

export default function Header() {
  const user = useSelector((state) => state.account);
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();
  const [anchorEl, setAnchorEl] = React.useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const navigate = useNavigate();

  const createBook = () => {
    dispatch({
      type: "ACTION_WITH_CREATE_MODAL",
      payload: true,
    });
  };
  const logOut = () => {
    dispatch(signOut());
    dispatch(clearCart());
    navigate("/signIn");
  };
  const goto = () => {
    if (user.userId === null) {
      navigate("/signIn");
    } else navigate("/orderPage");
  };
  const gotoCart = () => {
    if (user.userId === null) {
      navigate("/signIn");
    } else navigate("/cart");
  };
  React.useEffect(() => {
    if ((moment(user.cancelData, moment.ISO_8601) < moment(moment(), moment.ISO_8601))) {
      refreshToken(user.userId).then((res) => dispatch(refreshedToken(res.data)));
    }
  }, []);

  return (
    <div className="header">
      <div className="container">
        <Link to="/home">
          <div className="header__logo">
            <img width="70" height="60" src={logoSvg} alt="Books logo" />
            <div>
              <h1>Freedom Read</h1>
              <p>Only on book pages we feel freedom</p>
            </div>
          </div>
        </Link>

        <React.Fragment>
          <Box sx={{ display: "flex", alignItems: "center", textAlign: "center" }}>
            <Tooltip title="Account settings">
              <IconButton
                onClick={handleClick}
                size="small"
                sx={{ ml: 2 }}
                aria-controls={open ? "account-menu" : undefined}
                aria-haspopup="true"
                aria-expanded={open ? "true" : undefined}>
                <Avatar sx={{ width: 32, height: 32 }}>U</Avatar>
              </IconButton>
            </Tooltip>
          </Box>
          <Menu
            anchorEl={anchorEl}
            id="account-menu"
            open={open}
            onClose={handleClose}
            onClick={handleClose}
            PaperProps={{
              elevation: 0,
              sx: {
                overflow: "visible",
                filter: "drop-shadow(0px 2px 8px rgba(0,0,0,0.32))",
                mt: 1.5,
                "& .MuiAvatar-root": {
                  width: 32,
                  height: 32,
                  ml: -0.5,
                  mr: 1,
                },
                "&:before": {
                  content: '""',
                  display: "block",
                  position: "absolute",
                  top: 0,
                  right: 14,
                  width: 10,
                  height: 10,
                  bgcolor: "background.paper",
                  transform: "translateY(-50%) rotate(45deg)",
                  zIndex: 0,
                },
              },
            }}
            transformOrigin={{ horizontal: "right", vertical: "top" }}
            anchorOrigin={{ horizontal: "right", vertical: "bottom" }}>
            {user.userId === null ? (
              <MenuItem onClick={goto}>
                <Avatar />
                Sign In
              </MenuItem>
            ) : (
              <MenuItem onClick={goto}>
                <Avatar />
                Profile
              </MenuItem>
            )}
            <Divider />
            <MenuItem>
              <div className="header__cart">
                <Button className="button--cart" onClick={() => gotoCart()}>
                  <span>{cart.totalPrice.toFixed(2)}$</span>
                  <div className="button__delimiter"></div>
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
                  <span>{cart.totalCount}</span>
                </Button>{" "}
              </div>
            </MenuItem>
            {user.userId !== null ? (
              <MenuItem sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                <Button onClick={logOut}>Logout</Button>
              </MenuItem>
            ) : (
              ""
            )}
            {user.role === "Admin" ? (
              <MenuItem sx={{ display: "flex", alignItems: "center", justifyContent: "center" }}>
                <Button onClick={createBook}>Create book</Button>
              </MenuItem>
            ) : (
              ""
            )}
          </Menu>
        </React.Fragment>
      </div>
    </div>
  );
}
