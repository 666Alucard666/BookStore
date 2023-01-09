import React from "react";
import { useDispatch, useSelector } from "react-redux";
import moment from "moment";

import { addBooksToCart } from "../state/actions/cartAction";

export default function BookPage() {
  const product = useSelector((state) => state.books.currentBook);
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();
  const handleClick = () => {
    dispatch(addBooksToCart(product));
  };
  const count = cart.items[product.productId] && cart.items[product.productId].items.length;
  return (
    <div className="container">
      <div className="main-content">
        <div className="main-content__image">
          <img class="rectangle-3" src={product.image} />
        </div>
        <div className="main-content__info">
          <h1>{product.name}</h1>
          <ul className="info-list">
            <li>
              <b className="infoLabel">Company:</b>&nbsp;
              <span className="info">{product.producingCompany}</span>
            </li>
            <li>
              <b className="infoLabel">Category:</b>&nbsp;
              <span className="info">{product.category}</span>
            </li>
            <li>
              <b className="infoLabel">Country:</b>&nbsp;
              <span className="info">{product.producingCountry}</span>
            </li>
            <li>
              <b className="infoLabel">Produced:</b>&nbsp;
              <span className="info">{moment(product.producingDate).format("MMMM Do YYYY")}</span>
            </li>
            <li>
              <b className="infoLabel">Capacity:</b>&nbsp;
              <span className="info">{`${product.capacity}g`}</span>
            </li>
            <li>
              <b className="infoLabel">Gender:</b>&nbsp;
              <span className="info">{product.gender}</span>
            </li>
            <li>
              <b className="infoLabel">Contraindications:</b>&nbsp;
              <span className="info">{product.contraindication}</span>
            </li>
            <li>
              <b className="infoLabel">Instruction:</b>&nbsp;
              <div className="bookDescription">{product.instruction}</div>
            </li>
            <li>
              <b className="infoLabel">Additional info:</b>&nbsp;
              <ul className="info-list">
                {product.category === "Skin" && (
                  <>
                    <li>
                      <b className="infoLabel">For skin:</b>&nbsp;
                      <span className="info">{product.skinCareProduct.skinType}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Use purpose:</b>&nbsp;
                      <span className="info">{product.skinCareProduct.usePurpose}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Age restrictions:</b>&nbsp;
                      <span className="info">{`${product.skinCareProduct.ageRestrictionsStart} - ${product.skinCareProduct.ageRestrictionsEnd}`}</span>
                    </li>
                  </>
                )}
                {product.category === "Oral" && (
                  <>
                    <li>
                      <b className="infoLabel">Gum disease:</b>&nbsp;
                      <span className="info">{product.oralCavityProduct.gumDiseaseType}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Whitening:</b>&nbsp;
                      <span className="info">{product.oralCavityProduct.ssWhitening}</span>
                    </li>
                    <li>
                      <b className="infoLabel">On herbal base:</b>&nbsp;
                      <span className="info">{product.oralCavityProduct.isHerbalBase}</span>
                    </li>
                  </>
                )}
                {product.category === "Hair" && (
                  <>
                    <li>
                      <b className="infoLabel">For hair:</b>&nbsp;
                      <span className="info">{product.hairCareProduct.hairType}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Hair disease:</b>&nbsp;
                      <span className="info">{product.hairCareProduct.hairDisease}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Anti dandruff:</b>&nbsp;
                      <span className="info">{product.hairCareProduct.isAntiDandruff}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Not contains:</b>&nbsp;
                      <span className="info">{product.hairCareProduct.notContains}</span>
                    </li>
                  </>
                )}
                {product.category === "Nails" && (
                  <>
                    <li>
                      <b className="infoLabel">For nails:</b>&nbsp;
                      <span className="info">{product.nailsCareProduct.nailsType}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Nails disease:</b>&nbsp;
                      <span className="info">{product.nailsCareProduct.nailsDisease}</span>
                    </li>
                    <li>
                      <b className="infoLabel">Fragrance:</b>&nbsp;
                      <span className="info">{product.nailsCareProduct.fragnance}</span>
                    </li>
                  </>
                )}
              </ul>

            </li>
          </ul>
        </div>
      </div>
      <div className="navigation">
        <div className="navigation__price">
          <b className="infoLabel price">Price:</b>&nbsp;
          <span className="info price">{product.price}$</span>
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
