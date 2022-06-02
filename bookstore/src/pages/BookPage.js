import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addBooksToCart } from "../state/actions/cartAction";

export default function BookPage() {
  const book = useSelector((state) => state.books.currentBook);
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();
  const handleClick = () => {
    dispatch(addBooksToCart(book));
  };
  const navigate = useNavigate();
  const count = cart.items[book.id] && cart.items[book.id].items.length;
  return (
    <div class="desktop-2screen">
      <div class="flex-row">
        <img class="rectangle-3" src={book.image} />
        <div class="flex-col">
          <h1 class="title">{book.name}</h1>
          <div class="frame-1">
            <div class="frame-1-itemproximanova-regular-normal-black-24px">
              <span class="spanproximanova-regular-normal-black-24px">Author:&nbsp;&nbsp;</span>
              <span class="spanproximanova-bold-black-24px">{book.author}</span>
            </div>
            <div class="frame-1-itemproximanova-regular-normal-black-24px">
              <span class="spanproximanova-regular-normal-black-24px">Genre:&nbsp;&nbsp; </span>
              <span class="spanproximanova-bold-black-24px">{book.genre}</span>
            </div>
            <div class="overlap-group1">
              <div class="added-august-28-2021proximanova-regular-normal-black-24px">
                <span class="spanproximanova-regular-normal-black-24px">Added:&nbsp;&nbsp;</span>
                <span class="span1">{book.created}</span>
              </div>
              <div class="publishing-marvelproximanova-regular-normal-black-24px">
                <span class="spanproximanova-regular-normal-black-24px">
                  Publishing:&nbsp;&nbsp;
                </span>
                <span class="spanproximanova-bold-black-24px">{book.publishing}</span>
              </div>
            </div>
            <div class="descriptionproximanova-normal-black-24px">Description:</div>
            <div class="overlap-group">
              <div class="incredible-book-kakaproximanova-normal-black-24px">
                Incredible book
                kakakakakkakakkkakakakakakakkakakkakakakakakakakakkaaaaaaaaaaaaaaaaaaakakakaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="frame-2">
        <div class="flex-col-1">
          <div class="priceproximanova-bold-black-32px">Price:</div>
          <div class="text-1proximanova-bold-black-32px">{book.price}$</div>
        </div>
        <div class="overlap-group-1">
          <img
            class="vector"
            src="https://anima-uploads.s3.amazonaws.com/projects/6297f8160d594225bb962702/releases/6297f89ff5a85b8910628474/img/vector@2x.svg"
          />
          <div class="add-to-cart">Add to cart</div>
        </div>
        <div class="overlap-group1-1">
          <div class="return">Return</div>
        </div>
      </div>
    </div>
  );
}
