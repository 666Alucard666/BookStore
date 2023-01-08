import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { getOrdersByUser } from "../api/api";
import OrderBooksPopup from "../components/OrderBooksPopup";
import { signOut } from "../state/actions/authentification";
import emptyCard from "../assets/img/empty-cart.png";
import { createReceipt, deleteOrder } from "./../api/api";
import Button from "./../components/Button";

export default function OrderPage() {
  const userId = useSelector((state) => state.account.userId);
  const books = useSelector((state) => state.books);
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [orders, setOrders] = React.useState([]);
  React.useEffect(() => {
    getOrdersByUser(userId)
      .catch((err) => {
        if (err.response.data === undefined) {
          dispatch(signOut());
          navigate("/signIn");
        }
      })
      .then((res) => {
        setOrders(res.data);
      });
  }, [userId]);

  const getReceipt = (id) => {
    createReceipt(id);
  };
  
  return (
    <div className="container">
      <div className="main-content">
        <div className="main-content__info">
          <h1>Orders Page</h1>
        </div>
      </div>
      {orders !== [] || orders !== undefined ? (
        <ul>
          {orders.map((o) => {
            return (
              <li className="orderFrame">
                <ul>
                  <li className="number">№{o.orderId}</li>
                  <li>
                    <img
                      src={books.items.find((b) => b.productId === o.orderProducts[0]?.productId)?.image}
                      className="bookOrderImage"
                    />
                  </li>
                  <li>
                    <span className="info">{o.recipientName}</span>
                  </li>
                  <li>
                    <span className="info">{o.recipientAddress}</span>
                  </li>
                  <li>
                    <OrderBooksPopup books={books.items} orderBooks={o.orderProducts} />
                  </li>
                  <li>
                    <span className="info">{o.sum.toFixed(2)}$</span>
                  </li>
                  <li>
                    <Button onClick={() => getReceipt(o.orderId)}>Generate receipt</Button>
                  </li>
                  <li>
                    <Button onClick={() => deleteOrder(o.orderId)}>Delete order</Button>
                  </li>
                </ul>
              </li>
            );
          })}
        </ul>
      ) : (
        <div className="container container--cart">
          <div className="cart cart--empty">
            <h2>
              None orders <icon>😕</icon>
            </h2>
            <p>
              Seems like u haven't any orders yet
              <br />
              Comeback to main page for resolve this.
            </p>
            <img src={emptyCard} alt="Empty cart" />
            <a href="/home" className="button button--black">
              <span>Return</span>
            </a>
          </div>
        </div>
      )}
    </div>
  );
}
