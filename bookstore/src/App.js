import React from "react";
import { Route, Routes } from "react-router-dom";
import { Header } from "./components";
import { Home, Cart, SignIn, SignUp, BookPage, OrderPage } from "./pages";

function App() {
  return (
    <div className="wrapper">
      <Header />
      <div className="content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/home" element={<Home />} />
          <Route path="/cart" element={<Cart />} />
          <Route path="/signIn" element={<SignIn />} />
          <Route path="/signUp" element={<SignUp />} />
          <Route path="/bookPage" element={<BookPage />} />
          <Route path="/orderPage" element={<OrderPage />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
