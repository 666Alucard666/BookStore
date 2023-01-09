import {
  Button,
  CircularProgress,
  Grid,
  Box,
  OutlinedInput,
  Select,
  TextField,
  Typography,
  makeStyles,
  FormControl,
  MenuItem,
  Checkbox,
} from "@material-ui/core";
import { isNil } from "lodash";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getProductById, getShopsDropDown, editProduct, createProduct, deleteProduct } from "../../api/api";

const useStyles = makeStyles((theme) => ({
  root: {
    padding: "0 5px",
  },
  filtersContainer: {
    marginTop: "10px",
    display: "flex",
    justifyContent: "center",
  },
  filter: {
    width: "200px",
  },
  fieldLabelNot: {
    color: "#737373",
  },
  fieldLabel: {
    backgroundColor: "#BEBEBE",
  },
  fieldLabelNit: {
    backgroundColor: "#FFFFFF",
  },
}));

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

export default function EditProduct() {
  const classes = useStyles();
  const history = useNavigate();
  const { productId, category } = useParams();
  const [isLoading, setIsLoading] = React.useState(false);
  const [product, setProduct] = React.useState({
    productId: productId,
    name: "",
    producingCountry: "",
    price: 0,
    producingDate: new Date(),
    producingCompany: "",
    image: "",
    instruction: "",
    capacity: 0,
    contraindication: "",
    category: categories.find(x => x.value === category)?.value,
    gender: "",
    shopProducts: [],
    amountOnStore: 0,
    hairCareProduct: null,
    nailsCareProduct: null,
    oralCavityProduct: null,
    skinCareProduct: null,
  });
  const [shops, setShops] = React.useState([]);
  const [selectedShop, setSelectedShop] = React.useState({
    shopId: null,
    name: "",
  });
  const [newEntity, setNewEntity] = React.useState({
    shopId: null,
    productId: productId,
    count: 0,
  });
  console.log(selectedShop);

  React.useEffect(() => {
    if (!isNil(productId)) {
      setIsLoading(true);
      getProductById(productId).then((x) => setProduct(x.data));
      getShopsDropDown().then((x) => {
        setShops(x.data);
        setIsLoading(false);
      });
    }
  }, []);

  React.useEffect(() => {
    setNewEntity({ ...newEntity, count: 0 });
  }, [selectedShop]);

  const handleChange = (event) => {
    setProduct({ ...product, [event.target.name]: event.target.value });
  };

  const handleSkinChange = (event) => {
    setProduct({
      ...product,
      skinCareProduct: { ...product.skinCareProduct, [event.target.name]: event.target.value },
    });
  };

  const handleNailsChange = (event) => {
    setProduct({
      ...product,
      nailsCareProduct: { ...product.nailsCareProduct, [event.target.name]: event.target.value },
    });
  };

  const handleHairChange = (event) => {
    setProduct({
      ...product,
      hairCareProduct: { ...product.hairCareProduct, [event.target.name]: event.target.value },
    });
  };

  const handleOralChange = (event) => {
    setProduct({
      ...product,
      oralCavityProduct: { ...product.oralCavityProduct, [event.target.name]: event.target.value },
    });
  };

  const handleChangeCategory = (value) => {
    setProduct({ ...product, category: value });
  };

  const onAdd = () => {
    setProduct({ ...product, shopProducts: [...product.shopProducts, newEntity] });
  };

  const onSave = () => {
    isNil(productId) ? createProduct(product) : editProduct(product);
    history("/manageProducts");
  };

  const onDelete = () => {
    !isNil(productId) && deleteProduct(product.shopProducts.map(x => {
    return {
      shopId: x.shopId,
      productId: x.productId
    }      
    }));
    history("/manageProducts");
  }

  console.log(product, shops, selectedShop);

  return (
    <>
      <div className="container">
        <div className="main-content">
          {isLoading ? (
            <CircularProgress />
          ) : (
            <div className="main-content__info">
              <h1>{`Manage ${isNil(productId) ? "New Product" : product?.name.substring(0, 15)}`}</h1>
              <Box component="form" sx={{ marginLeft: "200px", marginTop: "50px" }}>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.name}
                    onChange={handleChange}
                    label="Name"
                    name="name"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.producingCountry}
                    onChange={handleChange}
                    label="Producing Country"
                    name="producingCountry"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.price}
                    onChange={handleChange}
                    label="Price"
                    name="price"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.producingCompany}
                    onChange={handleChange}
                    label="Producing Company"
                    name="producingCompany"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.image}
                    onChange={handleChange}
                    label="Image"
                    name="image"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.instruction}
                    onChange={handleChange}
                    label="Instruction"
                    name="instruction"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.capacity}
                    onChange={handleChange}
                    label="Capacity"
                    name="capacity"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.contraindication}
                    onChange={handleChange}
                    label="Contraindication"
                    name="contraindication"
                    variant="outlined"
                    size="small"
                  />
                </Grid>
                <Grid item style={{ marginTop: "20px" }}>
                  <TextField
                    value={product?.gender}
                    onChange={handleChange}
                    label="Gender"
                    name="gender"
                    variant="outlined"
                    size="small"
                  />
                </Grid>

                <Grid sx={{ marginTop: "10px" }} item>
                  <FormControl variant="outlined" fullWidth className={classes.filter}>
                    <Typography className={classes.fieldLabelNot}>Category</Typography>
                    <Select
                      style={{ maxWidth: "220px" }}
                      labelId="service-line-label"
                      name="category"
                      inputProps={{ readOnly: true }}
                      value={product?.category}
                      onChange={(event) => handleChangeCategory(event.target.value)}
                      input={<OutlinedInput name="category" size="small" />}>
                      {(categories || []).map((type) => {
                        return (
                          <MenuItem key={type.value} value={type.value}>
                            {type.name}
                          </MenuItem>
                        );
                      })}
                    </Select>
                  </FormControl>
                </Grid>
                {!isNil(productId) && (
                  <>
                    <Grid sx={{ marginTop: "10px" }} item>
                      <FormControl variant="outlined" fullWidth className={classes.filter}>
                        <Typography className={classes.fieldLabelNot}>Shop</Typography>
                        <Select
                          style={{ maxWidth: "240px" }}
                          labelId="service-line-label"
                          name="shop"
                          value={selectedShop}
                          input={<OutlinedInput name="shop" />}
                          onChange={(event) => {
                            setSelectedShop(event.target.value);
                            setNewEntity({ ...newEntity, shopId: event.target.value.shopId });
                          }}
                          size="small">
                          {shops.map((type) => (
                            <MenuItem
                              value={type}
                              className={
                                product?.shopProducts.some((x) => x.shopId === type.shopId)
                                  ? classes.fieldLabel
                                  : classes.fieldLabelNit
                              }>
                              {type.name}
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={
                          product?.shopProducts.find((x) => x.shopId === selectedShop.shopId)
                            ?.count ?? newEntity.count
                        }
                        type="number"
                        onChange={(event) => {
                          isNil(
                            product?.shopProducts.find((x) => x.shopId === selectedShop.shopId)
                              ?.count,
                          )
                            ? setNewEntity({ ...newEntity, count: event.target.value })
                            : setProduct({
                                ...product,
                                shopProducts: product.shopProducts.map((x) => {
                                  if (x.shopId === selectedShop.shopId) {
                                    return {
                                      ...x,
                                      count: event.target.value,
                                    };
                                  }
                                  return x;
                                }),
                              });
                        }}
                        label="Count"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Button
                      variant="contained"
                      onClick={onAdd}
                      style={{ marginTop: "20px", marginLeft: "30px" }}>
                      Add to shop
                    </Button>
                  </>
                )}

                {category === "Skin" && (
                  <>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.skinCareProduct?.skinType}
                        onChange={handleSkinChange}
                        label="Skin type"
                        name="skinType"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.skinCareProduct?.usePurpose}
                        onChange={handleSkinChange}
                        label="Use Purpose"
                        name="usePurpose"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        type="number"
                        value={product?.skinCareProduct?.ageRestrictionsStart}
                        onChange={handleSkinChange}
                        label="Age Restrictions Start"
                        name="ageRestrictionsStart"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        type="number"
                        value={product?.skinCareProduct?.ageRestrictionsEnd}
                        onChange={handleSkinChange}
                        label="Age Restrictions End"
                        name="ageRestrictionsEnd"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                  </>
                )}
                {category === "Nails" && (
                  <>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.nailsCareProduct?.nailsType}
                        onChange={handleNailsChange}
                        label="Nails Type"
                        name="nailsType"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.nailsCareProduct?.nailsDisease}
                        onChange={handleNailsChange}
                        label="Nails Disease"
                        name="nailsDisease"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.nailsCareProduct?.fragrance}
                        onChange={handleNailsChange}
                        label="Fragrance"
                        name="fragrance"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                  </>
                )}
                {category === "Hair" && (
                  <>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.hairCareProduct?.hairType}
                        onChange={handleHairChange}
                        label="Hair Type"
                        name="hairType"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.hairCareProduct?.hairDisease}
                        onChange={handleHairChange}
                        label="Hair Disease"
                        name="hairDisease"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <Typography className={classes.fieldLabelNot}>Is anti dandruff</Typography>
                      <Checkbox
                        checked={product?.hairCareProduct?.isAntiDandruff}
                        onChange={(event) =>
                          setProduct({
                            ...product,
                            hairCareProduct: {
                              ...product.hairCareProduct,
                              isAntiDandruff: event.target.checked,
                            },
                          })
                        }></Checkbox>
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.hairCareProduct?.notContains}
                        onChange={handleHairChange}
                        label="Not Contains"
                        name="notContains"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                  </>
                )}
                {category === "Oral" && (
                  <>
                    <Grid item style={{ marginTop: "20px" }}>
                      <TextField
                        value={product?.oralCavityProduct?.gumDiseaseType}
                        onChange={handleOralChange}
                        label="Gum Disease Type"
                        name="gumDiseaseType"
                        variant="outlined"
                        size="small"
                      />
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <Typography className={classes.fieldLabelNot}>Is Whitening</Typography>
                      <Checkbox
                        checked={product?.oralCavityProduct?.isWhitening}
                        onChange={(event) =>
                          setProduct({
                            ...product,
                            oralCavityProduct: {
                              ...product.oralCavityProduct,
                              isWhitening: event.target.checked,
                            },
                          })
                        }></Checkbox>
                    </Grid>
                    <Grid item style={{ marginTop: "20px" }}>
                      <Typography className={classes.fieldLabelNot}>Is HerbalBase</Typography>
                      <Checkbox
                        checked={product?.oralCavityProduct?.isHerbalBase}
                        onChange={(event) =>
                          setProduct({
                            ...product,
                            oralCavityProduct: {
                              ...product.oralCavityProduct,
                              isHerbalBase: event.target.checked,
                            },
                          })
                        }></Checkbox>
                    </Grid>
                  </>
                )}
                <Grid item>
                  <Button
                    variant="contained"
                    onClick={onSave}
                    style={{ marginTop: "20px", marginLeft: "50px" }}>
                    Save
                  </Button>
                  {!isNil(productId) && <Button
                  variant="contained"
                    onClick={onDelete}
                    style={{ marginTop: "20px", marginLeft: "50px" }}>
                      Delete
                  </Button> }
                  
                </Grid>
              </Box>
            </div>
          )}
        </div>
      </div>
    </>
  );
}
