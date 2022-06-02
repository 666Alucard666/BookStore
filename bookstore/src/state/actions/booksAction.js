export const setBooks = (items) => ({
  type: "SET_BOOKS",
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
export const pickBook = (book) => ({
  type: "CHOOSE_BOOK",
  payload: book,
});
