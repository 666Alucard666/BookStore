import { Typography } from "@mui/material";
import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { deleteBookRequest } from "../../api/api";
import { deleteBook, pickBook } from "../../state/actions/booksAction";
import { addBooksToCart } from "../../state/actions/cartAction";

export default function BookBlock({ book, addedCount }) {
  const dispatch = useDispatch();
  const role = useSelector((state) => state.account?.role);
  const navigate = useNavigate();
  const handleClick = () => {
    dispatch(addBooksToCart(book));
  };
  const handleImageClick = () => {
    dispatch(pickBook(book));
    navigate("/bookPage");
  };
  const handleDeleteClick = () => {
    dispatch(deleteBook(book));
    deleteBookRequest(book);
  };
  const handleUpdateClick = () => {
    dispatch({
      type: "ACTION_WITH_MODAL",
      payload: true,
    });
    dispatch(pickBook(book));
  };
  return (
    <div className="pizza-block">
      <img className="pizza-block__image" src={book.image} alt="Book" onClick={handleImageClick} />
      <h4 className="pizza-block__title">{book.name}</h4>
      <div className="pizza-block__selector">
        <Typography
          alignContent={"center"}
          component="h3"
          variant="h5"
          fontWeight={600}
          fontSize={14}>
          {book.author}
        </Typography>
        <Typography
          alignContent={"center"}
          component="h3"
          variant="h5"
          fontWeight={600}
          fontSize={14}>
          {book.genre}
        </Typography>
      </div>
      <div className="pizza-block__bottom">
        <div className="pizza-block__price">{`${book.price}$`}</div>
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
          {addedCount && <i>{addedCount}</i>}
        </div>
      </div>
      {role === "Admin" ? (
        <div className="block">
          <div className="buttonDelete" onClick={handleDeleteClick}>
            <span>Delete</span>
          </div>
          <div className="buttonUpdate" onClick={handleUpdateClick}>
            <span>Update</span>
          </div>
        </div>
      ) : (
        ""
      )}
    </div>
  );
}
