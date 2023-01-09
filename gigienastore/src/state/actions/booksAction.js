export const setProducts = (items) => ({
  type: "SET_PRODUCTS",
  payload: items,
});
export const setFilteredBooks = (items) => ({
  type: "SET_FILTERED_BOOKS",
  payload: items,
});
export const sortBooksAlphabet = () => ({
  type: "SORT_BOOKS_ALPHABET",
});
export const sortBooksPrice = () => ({
  type: "SORT_BOOKS_PRICE",
});
export const sortBooksDate = () => ({
  type: "SORT_BOOKS_DATE",
});
export const pickProduct = (book) => ({
  type: "CHOOSE_PRODUCT",
  payload: book,
});
export const deleteBook = (book) => ({
  type: "DELETE_BOOK",
  payload: book,
});
