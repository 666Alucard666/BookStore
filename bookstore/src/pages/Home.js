import React from "react";

import TextField from "@mui/material/TextField";
import InputAdornment from "@mui/material/InputAdornment";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import IconButton from "@mui/material/IconButton";
import SearchIcon from "@mui/icons-material/Search";
import ClearIcon from "@mui/icons-material/Clear";
import detecive from "../assets/img/detective.png";
import { getProducts } from "../api/api";
import { Categories, SortPopup } from "../components/index";
import { useDispatch, useSelector } from "react-redux";
import { pickProduct, setProducts } from "../state/actions/booksAction";
import { BooksWrap } from "../components/books/BooksWrap";
import { Grid } from "@mui/material";

function Home() {

  const categories = [
    {
      name:"Nails Care",
      value:"Nails"
    },
    {
      name:"Skin Care",
      value:"Skin"
    },
    {
      name:"Oral Cave Care",
      value:"Oral"
    },
    {
      name:"Hair Care",
      value:"Hair"
    },
    ];
  const sortCategories = ["Alphabet", "Price"];
  const [search, setSearch] = React.useState({
    name: "",
    producingCountry: "",
    producingCompany: "",
  });
  const books = useSelector((state) => state.books);
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();

  React.useEffect(() => {
    getProducts().then((res) => dispatch(setProducts(res.data)));
    dispatch(pickProduct({}));
  }, [dispatch]);

  function searchBook(rows) {
    return rows?.filter(function (item) {
      for (var key in search) {
        if (search[key] === "") {
          continue;
        }
        if (item[key] === undefined || !item[key].toLowerCase().includes(search[key].toLowerCase()))
          return false;
      }
      return true;
    });
  }

  const handleSearch = (e) => {
    setSearch({ ...search, [e.target.name]: e.target.value });
  };

  const clearSearchField = () => {
    setSearch({
      name: "",
      producingCountry: "",
      producingCompany: "",
    });
  };

  return (
    <div className="container">
      <div className="content__top">
        <Categories items={categories} />
        <SortPopup items={sortCategories} />
      </div>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <div>
          <h2 className="content__title">Products</h2>
        </div>
        <div>
          <Accordion>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header">
              <div style={{ display: "flex", justifyContent: "flex-start", alignItems: "center" }}>
                <div>Search product</div>{" "}
                <div>
                  <SearchIcon
                    fontSize="small"
                    sx={{ padding: "1px", margin: "3px 0px 0px 10px" }}
                  />
                </div>
              </div>
            </AccordionSummary>
            <AccordionDetails>
              <Grid item>
                <TextField
                  value={search.name}
                  onChange={handleSearch}
                  label="Name"
                  name="name"
                  variant="outlined"
                  color="warning"
                  size="small"
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        {search !== "" && (
                          <IconButton onClick={clearSearchField}>
                            <ClearIcon />
                          </IconButton>
                        )}
                      </InputAdornment>
                    ),
                  }}
                />
              </Grid>
              <Grid item sx={{ marginTop: "10px" }}>
                <TextField
                  value={search.producingCompany}
                  onChange={handleSearch}
                  label="Company"
                  name="producingCompany"
                  variant="outlined"
                  color="warning"
                  size="small"
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        {search !== "" && (
                          <IconButton onClick={clearSearchField}>
                            <ClearIcon />
                          </IconButton>
                        )}
                      </InputAdornment>
                    ),
                  }}
                />
              </Grid>
              <Grid item sx={{ marginTop: "10px" }}>
                <TextField
                  value={search.producingCountry}
                  onChange={handleSearch}
                  label="Country"
                  name="producingCountry"
                  variant="outlined"
                  color="warning"
                  size="small"
                  InputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        {search !== "" && (
                          <IconButton onClick={clearSearchField}>
                            <ClearIcon />
                          </IconButton>
                        )}
                      </InputAdornment>
                    ),
                  }}
                />
              </Grid>
            </AccordionDetails>
          </Accordion>
        </div>
      </div>
      <div className="content__items">
        {searchBook(books.items).length !== 0 ? (
          <BooksWrap data={searchBook(books.items)} cart={cart} />
        ) : (
          <div className="container container--cart">
            <div className="cart cart--empty">
              <h2>
                None products <icon>😕</icon>
              </h2>
              <img src={detecive} alt="Empty cart" />
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default Home;
