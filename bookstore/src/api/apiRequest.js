import axios from "axios";

axios.interceptors.request.use((config) => {
  config.headers["Authorization"] = `Bearer ${window.localStorage.getItem("token")}`;
  return config;
});

export { axios };
