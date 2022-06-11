import React from "react";
import { useDispatch } from "react-redux";
import { getFilteredBooks } from "../api/api";
import { setFilteredBooks } from "../state/actions/booksAction";

function Categories({ items }) {
  const [filter, setFilter] = React.useState({
    author: "_",
    genre: "_",
    startPrice: 0,
    endPrice: 99999,
  });
  const [active, setActive] = React.useState(null);
  const dispatch = useDispatch();
  const handleClick = (categ, index) => {
    setActive(index);
    setFilter({ ...filter, genre: categ });
  };
  const handleStart = () => {
    setActive(null);
    setFilter({ ...filter, genre: "_" });
  };
  React.useEffect(() => {
    getFilteredBooks(filter).then((res) => {
      dispatch(setFilteredBooks(res.data));
    });
  }, [filter, dispatch]);

  return (
    <div className="categories">
      <ul>
        <li className={active === null ? "active" : ""} onClick={() => handleStart(null)}>
          All
        </li>
        {items.map((categ, index) => (
          <li
            onClick={() => handleClick(categ, index)}
            className={index === active ? "active" : ""}
            key={categ}>
            {categ}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Categories;
