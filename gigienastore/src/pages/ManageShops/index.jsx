import { Box, Button, FormControl, InputLabel, MenuItem, OutlinedInput, Select } from '@material-ui/core';
import { DataGrid } from '@mui/x-data-grid';
import React from 'react'
import { useNavigate } from 'react-router-dom';
import { getCities, getShopsByCity } from '../../api/api';
import { ArrowForwardIos } from "@mui/icons-material";

export default function ManageShops() {
    const [shops, setShops] = React.useState([]);
    const [cities, setCities] = React.useState([]);
    const [currentCity, setCurrentCity] = React.useState(shops[0]);
    const history = useNavigate();
    const initial = [
      { field: "name", headerName: "Name", width: 150 },
      { field: "region", headerName: "Region", width: 150 },
      { field: "endWorkingHours", headerName: "EndWorking Hours", width: 150 },
      { field: "startWorkingHours", headerName: "StartWorking Hours", width: 150 },
      { field: "size", headerName: "Size", width: 150 },
      { field: "city", headerName: "City", width: 150 },
      { field: "address", headerName: "Address", width: 150 },
      {
        field: "",
        headerName: "",
        flex: 0.1, //width
        sortable: false,
        hideSortIcons: true,
        align: "right",
        renderCell: (p) => {
          return (
            <Button onClick={() => history(`/editshops/${p.row.shopId}`)}>
              <ArrowForwardIos></ArrowForwardIos>
            </Button>
          );
        },
      },
    ];
  
    React.useEffect(() => {
        getCities().then((x) => setCities(x.data));
    }, []);
  
    React.useEffect(() => {
        getShopsByCity(currentCity).then((x) => setShops(x.data)).catch(e => setShops([]));
    }, [currentCity]);
  
    return (
      <>
        <div className="container">
          <div className="main-content">
            <div className="main-content__info">
              <h1>Manage shops</h1>
              <Box sx={{ minWidth: 250, marginTop: "10px", marginLeft:"160px" }}>
                <FormControl fullWidth>
                  <InputLabel id="demo-simple-select-label">Cities</InputLabel>
                  <Select
                    style={{ maxWidth: "240px" }}
                    labelId="service-line-label"
                    name="city"
                    value={currentCity}
                    input={<OutlinedInput name="city" />}
                    onChange={(event) => {
                        setCurrentCity(event.target.value);
                    }}
                    size="small">
                    {cities.map((type) => (
                      <MenuItem value={type}>{type}</MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Box>
              <Button
                variant="contained"
                onClick={() => history(`/editShops`)}
                style={{ marginTop: "20px", marginLeft: "200px" }}>
                Add new shop
              </Button>
            </div>
          </div>
          <div>
            <DataGrid
              sx={{ width: "1300px", marginRight: "400px", marginTop: "30px" }}
              rows={shops}
              autoHeight
              columns={initial}
              getRowId={(row) => row.shopId}
            />
          </div>
        </div>
      </>
    );
}
