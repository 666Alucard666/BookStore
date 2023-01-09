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

export const postSignUp = async (formData) => {
  const response = await fetch(endpoint + "user/Registration", {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },

    body: JSON.stringify({
      name: formData.name,
      email: formData.email,
      middleName: formData.middleName,
      phone: formData.phone,
      password: formData.password,
      surname: formData.surname,
      sex: formData.sex,
      dob: formData.dob,
      city: formData.city,
      address: formData.address,
      zip: formData.zip,
      specialty: formData.specialty,
    }),
  });

  return response;
};

export const getProducts = () => {
  return axios.get(endpoint + "product/GetProducts");
};

export const getProductsByCategory = (category) => {
  return axios.get(endpoint + "product/GetProducts",{
    params: {
      category: category,
    },
  });
};

export const refreshToken = (userId) => {
  return axios.get(endpoint + "user/RefreshToken", {
    params: {
      userId: userId,
    },
  });
};

export const getProductsByCategoryTable = async (category) => {
  const {data} = await axios.get(endpoint + "product/GetProducts", {
    params: {
      category: category,
    },
  });
  return data.map(x => {
    switch (category) {
      case "Skin":
        const elems = {
          ...x,
          skinType: x.skinCareProduct.skinType,
          usePurpose: x.skinCareProduct.usePurpose,
          ageRestrictionsStart: x.skinCareProduct.ageRestrictionsStart,
          ageRestrictionsEnd: x.skinCareProduct.ageRestrictionsEnd
        };
        delete elems.skinCareProduct;
        delete elems.hairCareProduct;
        delete elems.nailsCareProduct;
        delete elems.oralCavityProduct;
        return elems;
      case "Nails":
        const elemn = {
          ...x,
          nailsType: x.nailsCareProduct.nailsType,
          nailsDisease: x.nailsCareProduct.nailsDisease,
          fragrance: x.nailsCareProduct.fragrance,
        };
        delete elemn.skinCareProduct;
        delete elemn.hairCareProduct;
        delete elemn.nailsCareProduct;
        delete elemn.oralCavityProduct;
        return elemn;
      case "Hair":
        const elemh = {
          ...x,
          hairType: x.hairCareProduct.hairType,
          hairDisease: x.hairCareProduct.hairDisease,
          isAntiDandruff: x.hairCareProduct.isAntiDandruff,
          notContains: x.hairCareProduct.notContains
        };
        delete elemh.skinCareProduct;
        delete elemh.hairCareProduct;
        delete elemh.nailsCareProduct;
        delete elemh.oralCavityProduct;
        return elemh;
      case "Oral":
        const elemo = {
          ...x,
          gumDiseaseType: x.oralCavityProduct.gumDiseaseType,
          isWhitening: x.oralCavityProduct.isWhitening,
          isHerbalBase: x.oralCavityProduct.isHerbalBase,
        };
        delete elemo.skinCareProduct;
        delete elemo.hairCareProduct;
        delete elemo.nailsCareProduct;
        delete elemo.oralCavityProduct;
        return elemo;
      default:
        return{
          ...x
        }
    }
  });
};

export const getProductById = (id) =>{
  return axios.get(endpoint + "product/GetProductById", {
    params: {
      productId: id,
    },
  });
}

export const getShopsDropDown = () =>{
  return axios.get(endpoint + "shop/GetShopsDropDown");
}

export const editProduct = (product) => {
  return axios.put(endpoint + "product/EditProductInfo", [...[],product]);
}

export const createProduct = (product) => {
  return axios.post(endpoint + "product/CreateProduct", [...[],product]);
}

export const deleteProduct = (array) => {
  return axios.put(endpoint + "product/DeleteProduct", array)
}

export const getWorkersByShop = (shopId) => {
  return axios.get(endpoint + "person/GetWorkersByShop", {
    params:{
      shopId: shopId
    }
  })
}

export const getWorkerById = (id) => {
  return axios.get(endpoint + "person/GetWorkerById", {
    params: {
      userId: id,
    },
  });
};


export const getCustomerById = (id) => {
  return axios.get(endpoint + "person/GetCustomerById", {
    params: {
      userId: id,
    },
  });
};


export const getShopById = (id) => {
  return axios.get(endpoint + "shop/GetShopById", {
    params: {
      id: id,
    },
  });
};

export const updateShop = (shop) => {
  return axios.put(endpoint + "shop/UpdateShopsInfo", [...[], shop]);
}

export const createShop = (shop) => {
  return axios.post(endpoint + "shop/CreateShop", [...[], shop]);
}

export const deleteShop = (shopId) => {
  return axios.put(endpoint + "shop/DelistShops", [...[], shopId]);
}

export const getCities = () => {
  return axios.get(endpoint + "shop/GetCities")
}

export const crudWorker = (model) => {
  return axios.put(endpoint + "person/UpdatePersons", model);
};

export const getShopsByCity = (city) => {
  return axios.get(endpoint + "shop/GetShopsByCity", {
    params: {
      city: city,
    },
  });
};

export const postOrder = async ({
  recipientPhone,
  customerId,
  recipientName,
  recipientSurname,
  recipientCity,
  recipientAdress,
  sum,
  shopId,
  paymentType,
  productsList,
}) => {
  return await axios.post(endpoint + "order/CreateOrder", {
    recipientPhone: recipientPhone,
    customerId: customerId,
    recipientName: recipientName,
    recipientSurname: recipientSurname,
    recipientCity: recipientCity,
    recipientAddress: recipientAdress,
    sum: sum,
    shopId: shopId,
    paymentType: paymentType,
    paymentStatus: "Pending",
    productsList: productsList,
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
    data: {
      Id: book.id,
      Name: book.name,
      Genre: book.genre,
      Author: book.author,
      Price: book.price,
      Publishing: book.publishing,
      AmountOnStore: book.amountOnStore,
      Image: book.image,
      Created: book.created,
    },
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

export const deleteOrder = async (id) => {
  return await axios.put(endpoint + "order/DeleteOrder",{
    orderId: id
  });
}
