import React from "react";
import BookBlock from "./BookBlock";

export const BooksWrap = ({ data, cart, }) => {
  return (
    <>
      {data.map((product) => (
        <BookBlock
          key={product.productId}
          addedCount={cart.items[product.productId] && cart.items[product.productId].items.length}
          product={product}
        />
      ))}
    </>
  );
};
