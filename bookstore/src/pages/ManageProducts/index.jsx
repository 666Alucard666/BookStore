import React from "react";
import { getProductsByCategoryTable } from "../../api/api";
import { DataGrid } from "@mui/x-data-grid";
import Select from "@mui/material/Select";
import MenuItem from "@mui/material/MenuItem";
import InputLabel from "@mui/material/InputLabel";
import { Box, Button, FormControl } from "@mui/material";
import { ArrowForwardIos, DeleteOutline } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";

const categories = [
  {
    name: "Nails Care",
    value: "Nails",
  },
  {
    name: "Skin Care",
    value: "Skin",
  },
  {
    name: "Oral Cave Care",
    value: "Oral",
  },
  {
    name: "Hair Care",
    value: "Hair",
  },
];

export default function ManageProducts() {
  const [products, setProducts] = React.useState([]);
  const [category, setCategory] = React.useState("Skin");
  const history = useNavigate();
  const initial = [
    { field: "name", headerName: "Product Name", width: 150 },
    { field: "producingCountry", headerName: "Producing Country", width: 150 },
    { field: "producingDate", headerName: "Producing Date", width: 150 },
    { field: "price", headerName: "Price", width: 150 },
    { field: "producingCompany", headerName: "Producing Company", width: 150 },
    { field: "instruction", headerName: "Instruction", width: 150 },
    { field: "capacity", headerName: "Capacity", width: 150 },
    { field: "contraindication", headerName: "Contraindication", width: 150 },
    { field: "category", headerName: "Category", width: 150 },
    { field: "gender", headerName: "Gender", width: 150 },
    { field: "amountOnStore", headerName: "Amount on store", width: 150 },
  ];
  const [columns, setColumns] = React.useState(initial);

  React.useEffect(() => {
    switch (category) {
      case "Skin":
        const skinCol = [
          { field: "skinType", headerName: "Skin Type", width: 150 },
          { field: "usePurpose", headerName: "Use Purpose", width: 150 },
          { field: "ageRestrictionsStart", headerName: "Age Restrictions Start", width: 150 },
          { field: "ageRestrictionsEnd", headerName: "Age Restrictions End", width: 150 },
          {
            field: "",
            headerName: "",
            flex: 0.1, //width
            sortable: false,
            hideSortIcons: true,
            align: "right",
            renderCell: (p) => {
              return (
                <Button
                  onClick={() => history(`/editProduct/${p.row.category}/${p.row.productId}`)}>
                  <ArrowForwardIos></ArrowForwardIos>
                </Button>
              );
            },
          },
          
        ];
        setColumns([...initial, ...skinCol]);
        break;
      case "Nails":
        const nailCol = [
          { field: "nailsType", headerName: "Nails Type", width: 150 },
          { field: "nailsDisease", headerName: "Nails Disease", width: 150 },
          { field: "fragrance", headerName: "Fragrance", width: 150 },
          {
            field: "",
            headerName: "",
            flex: 0.1, //width
            sortable: false,
            hideSortIcons: true,
            align: "right",
            renderCell: (p) => {
              return (
                <Button
                  onClick={() => history(`/editProduct/${p.row.category}/${p.row.productId}`)}>
                  <ArrowForwardIos></ArrowForwardIos>
                </Button>
              );
            },
          },
          
        ];
        setColumns([...initial, ...nailCol]);
        break;
      case "Hair":
        const hairCol = [
          { field: "hairType", headerName: "Hair Type", width: 150 },
          { field: "hairDisease", headerName: "Hair Disease", width: 150 },
          { field: "isAntiDandruff", headerName: "Is Anti Dandruff", width: 150 },
          { field: "notContains", headerName: "Not Contains", width: 150 },
          {
            field: "",
            headerName: "",
            flex: 0.1, //width
            sortable: false,
            hideSortIcons: true,
            align: "right",
            renderCell: (p) => {
              return (
                <Button
                  onClick={() => history(`/editProduct/${p.row.category}/${p.row.productId}`)}>
                  <ArrowForwardIos></ArrowForwardIos>
                </Button>
              );
            },
          },
         
        ];
        setColumns([...initial, ...hairCol]);
        break;
      case "Oral":
        const oralCol = [
          { field: "gumDiseaseType", headerName: "Gum Disease Type", width: 150 },
          { field: "isWhitening", headerName: "Is Whitening", width: 150 },
          { field: "isHerbalBase", headerName: "Is Herbal Base", width: 150 },
          {
            field: "",
            headerName: "",
            flex: 0.1, //width
            sortable: false,
            hideSortIcons: true,
            align: "right",
            renderCell: (p) => {
              return (
                <Button
                  onClick={() => history(`/editProduct/${p.row.category}/${p.row.productId}`)}>
                  <ArrowForwardIos></ArrowForwardIos>
                </Button>
              );
            },
          },
         
        ];
        setColumns([...initial, ...oralCol]);
        break;
      default:
        break;
    }
    getProductsByCategoryTable(category).then((res) => setProducts(res)).catch(e => setProducts([]));
  }, [category]);

  return (
    <>
      <div className="container">
        <div className="main-content">
          <div className="main-content__info">
            <h1>Manage products</h1>
            <Box sx={{ minWidth: 250, marginTop: "10px" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-simple-select-label">Product type</InputLabel>
                <Select
                  labelId="demo-simple-select-label"
                  value={category}
                  onChange={(e) => setCategory(e.target.value)}
                  size="small">
                  {categories.map((type) => (
                    <MenuItem value={type.value}>{type.name}</MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Box>
            <Button
              variant="contained"
              onClick={() => history(`/editProduct/${category}`)}
              style={{ marginTop: "20px", marginLeft: "200px" }}>
              Add new product
            </Button>
          </div>
        </div>
        <div>
          <DataGrid
            sx={{ width: "1300px", marginRight: "400px", marginTop: "30px" }}
            rows={products}
            autoHeight
            columns={columns}
            getRowId={(row) => row.productId}
          />
        </div>
      </div>
    </>
  );
}
