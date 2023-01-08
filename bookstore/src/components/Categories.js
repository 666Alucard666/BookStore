import React from "react";
import { useDispatch } from "react-redux";
import { getProductsByCategory, getProducts } from "../api/api";
import { setFilteredBooks } from "../state/actions/booksAction";

function Categories({ items }) {
  const [filter, setFilter] = React.useState({
    category: "",
  });
  const [active, setActive] = React.useState(null);
  const dispatch = useDispatch();
  const handleClick = (categ, index) => {
    setActive(index);
    setFilter({ ...filter, category: categ });
  };
  const handleStart = () => {
    setActive(null);
    setFilter({ ...filter, category: "" });
  };
  React.useEffect(() => {
    if (filter.category === "") {
      getProducts().then((res) => {
        dispatch(setFilteredBooks(res.data));
      })
    }
    else{
      getProductsByCategory(filter.category).then((res) => {
        dispatch(setFilteredBooks(res.data));
      });
    }
  }, [filter, dispatch]);

  return (
    <div className="categories">
      <ul>
        <li className={active === null ? "active" : ""} onClick={() => handleStart(null)}>
          All
        </li>
        {items.map((categ, index) => (
          <li
            onClick={() => handleClick(categ.value, index)}
            className={index === active ? "active" : ""}
            key={categ.name}>
            {categ.name}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Categories;
