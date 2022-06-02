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
    }),
  });

  return response;
};

export const getBooks = () => {
  return axios.get(endpoint + "book/GetBooks");
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
