import React from "react";
import { useDispatch, useSelector } from "react-redux";
import CircularProgress from "@mui/material/CircularProgress";
import moment from "moment";

import { addBooksToCart } from "../state/actions/cartAction";
import { getBooksDescription } from "../api/api";

export default function BookPage() {
  const book = useSelector((state) => state.books.currentBook);
  const cart = useSelector((state) => state.cart);
  const [description, setDescription] = React.useState("");
  const [loading, setLoading] = React.useState(true);
  const dispatch = useDispatch();
  const handleClick = () => {
    dispatch(addBooksToCart(book));
  };
  getBooksDescription(book.name, book.author).then((res) => {
    setDescription(res);
    setLoading(false);
  });
  const count = cart.items[book.id] && cart.items[book.id].items.length;
  return (
    <div className="container">
      <div className="main-content">
        <div className="main-content__image">
          <img class="rectangle-3" src={book.image} />
        </div>
        <div className="main-content__info">
          <h1>{book.name}</h1>
          <ul className="info-list">
            <li>
              <b className="infoLabel">Author:</b>&nbsp;
              <span className="info">{book.author}</span>
            </li>
            <li>
              <b className="infoLabel">Genre:</b>&nbsp;
              <span className="info">{book.genre}</span>
            </li>
            <li>
              <b className="infoLabel">Publishing:</b>&nbsp;
              <span className="info">{book.publishing}</span>
            </li>
            <li>
              <b className="infoLabel">Added:</b>&nbsp;
              <span className="info">{moment(book.created).format("MMMM Do YYYY")}</span>
            </li>
            <li>
              <b className="infoLabel">Description:</b>&nbsp;
              <div className="bookDescription">{loading ? <CircularProgress /> : description}</div>
            </li>
          </ul>
        </div>
      </div>
      <div className="navigation">
        <div className="navigation__price">
          <b className="infoLabel price">Price:</b>&nbsp;
          <span className="info price">{book.price}$</span>
        </div>
        <div className="navigation__buttons">
          <div className="button button--outline button--add" onClick={() => handleClick()}>
            <svg
              width="12"
              height="12"
              viewBox="0 0 12 12"
              fill="none"
              xmlns="http://www.w3.org/2000/svg">
              <path
                d="M10.8 4.8H7.2V1.2C7.2 0.5373 6.6627 0 6 0C5.3373 0 4.8 0.5373 4.8 1.2V4.8H1.2C0.5373 4.8 0 5.3373 0 6C0 6.6627 0.5373 7.2 1.2 7.2H4.8V10.8C4.8 11.4627 5.3373 12 6 12C6.6627 12 7.2 11.4627 7.2 10.8V7.2H10.8C11.4627 7.2 12 6.6627 12 6C12 5.3373 11.4627 4.8 10.8 4.8Z"
                fill="white"
              />
            </svg>
            <span>Add to cart</span>
            {count && <i>{count}</i>}
          </div>

          <div>
            <a href="/home" className="button button--black">
              <span>Return</span>
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}
