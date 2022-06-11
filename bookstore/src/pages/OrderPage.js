import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { getOrdersByUser } from "../api/api";
import OrderBooksPopup from "../components/OrderBooksPopup";
import { signOut } from "../state/actions/authentification";
import { saveAs } from "file-saver";
import { createReceipt } from './../api/api';
import Button from './../components/Button';

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

  const getReceipt = (id)=>{
    createReceipt(id);
  }

  return (
    <div className="container">
      <div className="main-content">
        <div className="main-content__info">
          <h1>Orders Page</h1>
        </div>
      </div>
      <ul>
        {orders.map((o) => {
          return (
            <li className="orderFrame">
              <ul>
                <li className="number">№{o.orderNumber}</li>
                <li>
                  <img
                    src={books.items.find((b) => b.id === o.ordersBook[0].bookId)?.image}
                    className="bookOrderImage"
                  />
                </li>
                <li>
                  <span className="info">{o.recipient}</span>
                </li>
                <li>
                  <span className="info">{o.adress}</span>
                </li>
                <li>
                  <OrderBooksPopup books={books.items} orderBooks={o.ordersBook} />
                </li>
                <li>
                  <span className="info">{o.sum.toFixed(2)}$</span>
                </li>
                <li>
                  <Button onClick={()=>getReceipt(o.orderId)}>Generate receipt</Button>
                </li>
              </ul>
            </li>
          );
        })}
      </ul>
    </div>
  );
}
