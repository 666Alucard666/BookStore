import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  OutlinedInput,
  Select,
} from "@material-ui/core";
import { ArrowForwardIos } from "@mui/icons-material";
import { DataGrid } from "@mui/x-data-grid";
import React from "react";
import { useNavigate } from "react-router-dom";
import { getShopsDropDown, getWorkersByShop } from "../../api/api";

export default function ManageWorkers() {
  const [workers, setWorkers] = React.useState([]);
  const [shops, setShops] = React.useState([]);
  const [currentShop, setCurrentShop] = React.useState({
    shopId: null,
    name: "",
  });
  const history = useNavigate();
  const initial = [
    { field: "name", headerName: "Name", width: 150 },
    { field: "surname", headerName: "Surname", width: 150 },
    { field: "middleName", headerName: "Middle Name", width: 150 },
    { field: "sex", headerName: "Sex", width: 150 },
    { field: "dob", headerName: "Dob", width: 150 },
    { field: "city", headerName: "City", width: 150 },
    { field: "address", headerName: "Address", width: 150 },
    { field: "salary", headerName: "Salary", width: 150 },
    { field: "specialty", headerName: "Specialty", width: 150 },
    { field: "shopId", headerName: "Shop", width: 150 },
    {
      field: "",
      headerName: "",
      flex: 0.1, //width
      sortable: false,
      hideSortIcons: true,
      align: "right",
      renderCell: (p) => {
        return (
          <Button onClick={() => history(`/editWorkers/${p.row.workerId}`)}>
            <ArrowForwardIos></ArrowForwardIos>
          </Button>
        );
      },
    },
  ];

  React.useEffect(() => {
    getShopsDropDown().then((x) => setShops(x.data));
  }, []);

  React.useEffect(() => {
    getWorkersByShop(currentShop.shopId).then((x) => setWorkers(x.data)).catch(e => setWorkers([]));
  }, [currentShop]);

  return (
    <>
      <div className="container">
        <div className="main-content">
          <div className="main-content__info">
            <h1>Manage workers</h1>
            <Box sx={{ minWidth: 250, marginTop: "10px", marginLeft:"160px" }}>
              <FormControl fullWidth>
                <InputLabel id="demo-simple-select-label">Shops</InputLabel>
                <Select
                  style={{ maxWidth: "240px" }}
                  labelId="service-line-label"
                  name="shop"
                  value={currentShop}
                  input={<OutlinedInput name="shop" />}
                  onChange={(event) => {
                    setCurrentShop(event.target.value);
                  }}
                  size="small">
                  {shops.map((type) => (
                    <MenuItem value={type}>{type.name}</MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Box>
            <Button
              variant="contained"
              onClick={() => history(`/editWorkers`)}
              style={{ marginTop: "20px", marginLeft: "200px" }}>
              Add new worker
            </Button>
          </div>
        </div>
        <div>
          <DataGrid
            sx={{ width: "1300px", marginRight: "400px", marginTop: "30px" }}
            rows={workers}
            autoHeight
            columns={initial}
            getRowId={(row) => row.workerId}
          />
        </div>
      </div>
    </>
  );
}
