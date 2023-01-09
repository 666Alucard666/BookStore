import React from "react";
import { useSelector } from "react-redux";
import { Route, Routes } from "react-router-dom";
import { Header } from "./components";
import { Home, Cart, SignIn, SignUp, BookPage, OrderPage } from "./pages";
import ManageProducts from "./pages/ManageProducts";
import EditProduct from "./pages/ManageProducts/EditProduct.jsx";
import ManageShops from "./pages/ManageShops";
import EditShop from "./pages/ManageShops/EditShop";
import ManageWorkers from "./pages/ManageWorkers";
import EditWorker from "./pages/ManageWorkers/EditWorker";

function App() {
  const user = useSelector((state) => state.account).userId;
  return (
    <div className="wrapper">
      <Header />
      <div className="content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/home" element={<Home />} />
          <Route path="/cart" element={user !== null ? <Cart /> : <SignIn />} />
          <Route path="/signIn" element={<SignIn />} />
          <Route path="/signUp" element={<SignUp />} />
          <Route path="/productPage" element={<BookPage />} />
          <Route path="/orderPage" element={user !== null ? <OrderPage /> : <SignIn />} />
          <Route path="/manageProducts" element={user !== null ? <ManageProducts /> : <SignIn />} />
          <Route path="/manageWorkers" element={user !== null ? <ManageWorkers /> : <SignIn />} />
          <Route path="/editProduct/:category/:productId" element={user !== null ? <EditProduct /> : <SignIn />} />
          <Route path="/editProduct/:category" element={user !== null ? <EditProduct /> : <SignIn />} />
          <Route path="/editWorkers/:workerId" element={user !== null ? <EditWorker /> : <SignIn />} />
          <Route path="/editWorkers" element={user !== null ? <EditWorker /> : <SignIn />} />
          <Route path="/manageShops" element={user !== null ? <ManageShops /> : <SignIn />} />
          <Route path="/editShops/:shopId" element={user !== null ? <EditShop /> : <SignIn />} />
          <Route path="/editShops" element={user !== null ? <EditShop /> : <SignIn />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
