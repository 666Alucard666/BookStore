const initialState = {
  items: {},
  totalPrice: 0,
  totalCount: 0,
};
const getTotalPrice = (arr) => arr.reduce((sum, obj) => obj.price + sum, 0);

const _get = (obj, path) => {
  const [firstKey, ...keys] = path.split(".");
  return keys.reduce((val, key) => {
    return val[key];
  }, obj[firstKey]);
};

const getTotalSum = (obj, path) => {
  return Object.values(obj).reduce((sum, obj) => {
    const value = _get(obj, path);
    return sum + value;
  }, 0);
};

const cartReducer = (state = initialState, { type, payload }) => {
  switch (type) {
    case "ADD_BOOK_CART": {
      if (state.items[payload.productId]?.items?.length+1 >= state.items[payload.productId]?.items[0]?.amountOnStore) {
        return state;
      }
      const currentBooksItems = !state.items[payload.productId]
        ? [payload]
        : [...state.items[payload.productId].items, payload];
      const newBooks = {
        ...state.items,
        [payload.productId]: {
          items: currentBooksItems,
          totalPrice: getTotalPrice(currentBooksItems),
        },
      };

      const totalCount = getTotalSum(newBooks, "items.length");
      const totalPrice = getTotalSum(newBooks, "totalPrice");

      return {
        ...state,
        items: newBooks,
        totalCount: totalCount,
        totalPrice: totalPrice,
      };
    }
    case "REMOVE_CART_ITEM": {
      const newItems = {
        ...state.items,
      };
      const currentTotalPrice = newItems[payload].totalPrice;
      const currentTotalCount = newItems[payload].items.length;
      delete newItems[payload];
      return {
        ...state,
        items: newItems,
        totalPrice: state.totalPrice - currentTotalPrice,
        totalCount: state.totalCount - currentTotalCount,
      };
    }

    case "PLUS_CART_ITEM": {
      if (state.items[payload].items.length+1 > state.items[payload].items[0].amountOnStore) {
        return state;
      }
      const newObjItems = [...state.items[payload].items, state.items[payload].items[0]];
      const newItems = {
        ...state.items,
        [payload]: {
          items: newObjItems,
          totalPrice: getTotalPrice(newObjItems),
        },
      };

      const totalCount = getTotalSum(newItems, "items.length");
      const totalPrice = getTotalSum(newItems, "totalPrice");

      return {
        ...state,
        items: newItems,
        totalCount,
        totalPrice,
      };
    }

    case "MINUS_CART_ITEM": {
      const oldItems = state.items[payload].items;
      const newObjItems = oldItems.length > 1 ? state.items[payload].items.slice(1) : oldItems;
      const newItems = {
        ...state.items,
        [payload]: {
          items: newObjItems,
          totalPrice: getTotalPrice(newObjItems),
        },
      };

      const totalCount = getTotalSum(newItems, "items.length");
      const totalPrice = getTotalSum(newItems, "totalPrice");

      return {
        ...state,
        items: newItems,
        totalCount,
        totalPrice,
      };
    }
    case "CLEAR_CART":
      return {
        totalPrice: 0,
        totalCount: 0,
        items: {},
      };

    default:
      return state;
  }
};

export default cartReducer;
