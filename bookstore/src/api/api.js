import moment from "moment";
import { axios } from "./apiRequest";

export const endpoint = process.env.REACT_APP_API;

export const postSignIn = (Login, Password) => {
  return async (dispatch) => {
    await fetch(endpoint + "user/Login", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },

      credentials: "include",
      body: JSON.stringify({
        Login,
        Password,
      }),
    })
      .then((res) => res.json())
      .then((response) => {
        window.localStorage.setItem("token", response.token);
        dispatch({
          type: "LOGIN",
          payload: {
            token: response.token,
            userId: response.userId,
            cancelData: response.cancelDate,
            role: response.role,
          },
        });
      });
  };
};

export const postSignUp = async (Name, Email, Phone, Password, Surname, UserName) => {
  const response = await fetch(endpoint + "user/Registration", {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },

    body: JSON.stringify({
      Name: Name,
      Surname: Surname,
      UserName: UserName,
      Email: Email,
      PhoneNumber: Phone,
      Password: Password,
      Role: "User",
    }),
  });

  return response;
};

export const getBooks = () => {
  return axios.get(endpoint + "book/GetBooks");
};

export const refreshToken = (userId) => {
  return axios.get(endpoint + "user/RefreshToken", {
    params: {
      userId: userId,
    },
  });
};

export const getBooksDescription = async (book, author) => {
  book = book.replace(" ", "%20");
  author = author.replace(" ", "%20");
  const data = await fetch(
    `https://books.googleapis.com/books/v1/volumes?q=${book}%20by%20${author}&langRestrict=en&maxResults=5&key=AIzaSyAil2G4IMDXy5FDAI4xsaBu1xIlveSJlJ0`,
  )
    .then((res) => res.json())
    .then((resp) => {
      let prom = resp.items.filter((b) => b.volumeInfo.description !== undefined);
      prom.sort((a, b) => {
        if (a.volumeInfo.description?.length > b.volumeInfo.description?.length) {
          return -1;
        }
        if (a.volumeInfo.description?.length < b.volumeInfo.description?.length) {
          return 1;
        }
        return 0;
      });
      return prom[0].volumeInfo.description;
    });
  return data;
};
export const getFilteredBooks = ({ author, genre, startPrice, endPrice }) => {
  return axios.get(endpoint + "book/GetBooksByFilter", {
    params: {
      Author: author,
      Genre: genre,
      StartPrice: startPrice,
      EndPrice: endPrice,
    },
  });
};
export const postOrder = async ({ phoneNumber, adress, userId, recipient, sum, books }) => {
  return await axios.post(endpoint + "order/CreateOrder", {
    PhoneNumber: phoneNumber,
    Adress: adress,
    Date: moment(moment().toISOString(), moment.ISO_8601),
    UserId: userId,
    OrderNumber: 0,
    Sum: sum,
    Recipient: recipient,
    Books: books,
  });
};
export const getOrdersByUser = (userId) => {
  return axios.get(endpoint + "order/GetOrdersByUser", {
    params: {
      userId: userId,
    },
  });
};
export const deleteBookRequest = async (book) => {
  return axios.delete(endpoint + "book/DeleteBook", {
    data:{
      Id: book.id,
      Name: book.name,
      Genre: book.genre,
      Author: book.author,
      Price: book.price,
      Publishing: book.publishing,
      AmountOnStore: book.amountOnStore,
      Image: book.image,
      Created: book.created,
    }
  });
};
export const editBookRequest = async (book) => {
  return axios.put(endpoint + "book/EditBook", {
    Id: book.id,
    Name: book.name,
    Genre: book.genre,
    Author: book.author,
    Price: book.price,
    Publishing: book.publishing,
    AmountOnStore: book.amountOnStore,
    Image: book.image,
    Created: book.created,
  });
};
export const createBookRequest = async (book) => {
  return await axios.post(endpoint + "book/CreateBook", {
    Id: book.id,
    Name: book.name,
    Genre: book.genre,
    Author: book.author,
    Price: book.price,
    Publishing: book.publishing,
    AmountOnStore: book.amountOnStore,
    Image: book.image,
    Created: moment(moment(), "YYYY-MM-DDTHH:mm:ssz"),
  });
};

export const createReceipt = async (id) => {
  return await axios.get(endpoint + "order/GetOrdersReceipt", {
    params: {
      id: id,
    },
  });
};
