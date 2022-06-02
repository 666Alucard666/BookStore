import React from "react";
import BookBlock from "./BookBlock";

export const BooksWrap = ({ data, cart }) => {
  return (
    <>
      {data.map((book) => (
        <BookBlock
          key={book.id}
          addedCount={cart.items[book.id] && cart.items[book.id].items.length}
          book={book}
        />
      ))}
    </>
  );
};
