import React from "react";

import TextField from "@mui/material/TextField";
import InputAdornment from "@mui/material/InputAdornment";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import IconButton from "@mui/material/IconButton";
import SearchIcon from "@mui/icons-material/Search";
import EditModalWindow from "../components/books/EditModalWindow";
import ClearIcon from "@mui/icons-material/Clear";

import { getBooks } from "../api/api";
import { Categories, SortPopup } from "../components/index";
import { useDispatch, useSelector } from "react-redux";
import { pickBook, setBooks } from "../state/actions/booksAction";
import { BooksWrap } from "../components/books/BooksWrap";
import CreateModalWindow from "../components/books/CreateModalWindow";

function Home() {
  const categories = [
    "Fantasy",
    "Comics",
    "Adventure",
    "Classics",
    "Detective",
    "Romance",
    "Horror",
  ];
  const sortCategories = ["Alphabet", "Price", "Date"];
  const [search, setSearch] = React.useState("");
  const books = useSelector((state) => state.books);
  const cart = useSelector((state) => state.cart);
  const dispatch = useDispatch();

  React.useEffect(() => {
    getBooks().then((res) => dispatch(setBooks(res.data)));
    dispatch(pickBook({}));
  }, [dispatch]);

  const universalSearch = (row) => {
    const value = row.name + row.author;
    return value?.toLowerCase().indexOf(search.toLowerCase()) > -1;
  };

  function searchBook(rows) {
    return rows?.filter((row) => universalSearch(row));
  }

  const handleSearch = (e) => {
    setSearch(e.target.value);
  };

  const clearSearchField = () => {
    setSearch("");
  };

  return (
    <div className="container">
      <div className="content__top">
        <Categories items={categories} />
        <SortPopup items={sortCategories} />
      </div>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
        <div>
          <h2 className="content__title">Books</h2>
        </div>
        <div>
          <Accordion>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1a-content"
              id="panel1a-header">
              <div style={{ display: "flex", justifyContent: "flex-start", alignItems: "center" }}>
                <div>Search book</div>{" "}
                <div>
                  <SearchIcon
                    fontSize="small"
                    sx={{ padding: "1px", margin: "3px 0px 0px 10px" }}
                  />
                </div>
              </div>
            </AccordionSummary>
            <AccordionDetails>
              <TextField
                value={search}
                onChange={handleSearch}
                label="Search book"
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
            </AccordionDetails>
          </Accordion>
        </div>
      </div>
      <div className="content__items">
        <BooksWrap data={searchBook(books.items)} cart={cart} />
      </div>
      <EditModalWindow />
      <CreateModalWindow/>
    </div>
  );
}

export default Home;
