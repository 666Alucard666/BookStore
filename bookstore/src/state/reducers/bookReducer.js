const initialState = {
  items: [],
  isLoaded: false,
  sorting: "Alphabet",
  currentBook: {},
  openEdit: false,
  openCreate:false,
};

const bookReducer = (state = initialState, { type, payload }) => {
  switch (type) {
    case "SET_PRODUCTS":
      return {
        ...initialState,
        items: payload.sort(function (a, b) {
          if (state.sorting === "Alphabet") {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          }
          if (state.sorting === "Price") {
            if (a.price < b.price) {
              return -1;
            }
            if (a.price > b.price) {
              return 1;
            }
            return 0;
          }
        }),
        isLoaded: true,
        sorting: state.sorting,
      };
    case "SET_FILTERED_BOOKS":
      return {
        ...state,
        items: payload.sort(function (a, b) {
          if (state.sorting === "Alphabet") {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          } else if (state.sorting === "Price") {
            if (a.price < b.price) {
              return -1;
            }
            if (a.price > b.price) {
              return 1;
            }
            return 0;
          } else {
            if (a.created > b.created) {
              return -1;
            }
            if (a.created < b.created) {
              return 1;
            }
            return 0;
          }
        }),
        isLoaded: true,
        sorting: state.sorting,
      };
    case "SORT_BOOKS_ALPHABET":
      return {
        ...initialState,
        isLoaded: true,
        items: state.items.sort(function (a, b) {
          if (a.name < b.name) {
            return -1;
          }
          if (a.name > b.name) {
            return 1;
          }
          return 0;
        }),
        sorting: "Alphabet",
      };
    case "SORT_BOOKS_PRICE":
      return {
        ...initialState,
        isLoaded: true,
        items: state.items.sort(function (a, b) {
          if (a.price < b.price) {
            return -1;
          }
          if (a.price > b.price) {
            return 1;
          }
          return 0;
        }),
        sorting: "Price",
      };
    case "SORT_BOOKS_DATE":
      return {
        ...initialState,
        isLoaded: true,
        items: state.items.sort(function (a, b) {
          if (a.created > b.created) {
            return -1;
          }
          if (a.created < b.created) {
            return 1;
          }
          return 0;
        }),
        sorting: "Date",
      };
    case "CHOOSE_PRODUCT":
      return {
        ...initialState,
        items: state.items,
        currentBook: payload,
        sorting: state.sorting,
        openEdit: state.openEdit,
      };
    case "DELETE_BOOK":
      state.items.splice(state.items.indexOf(payload), 1);
      return {
        ...initialState,
        items: state.items,
        currentBook: state.currentBook,
        sorting: state.sorting,
        openEdit: state.openEdit,
      };
    case "ACTION_WITH_MODAL":
      return {
        ...initialState,
        items: state.items,
        currentBook: state.currentBook,
        sorting: state.sorting,
        openEdit: payload,
      };
      case "ACTION_WITH_CREATE_MODAL":
      return {
        ...initialState,
        items: state.items,
        currentBook: state.currentBook,
        sorting: state.sorting,
        openEdit: state.openEdit,
        openCreate: payload,
      };
    default:
      return state;
  }
};

export default bookReducer;
