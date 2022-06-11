import { AxiosError, AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";

const onRequest = (config: AxiosRequestConfig): AxiosRequestConfig => {
  const token = window.localStorage.getItem("token");
  config.headers["Authorization"] = `Bearer ${token}`;

  return config;
};

const onRequestError = (error: AxiosError): Promise<AxiosError> => {
  return Promise.reject(error);
};

const onResponse = (response: AxiosResponse): AxiosResponse => {
  return response;
};

const onResponseError = async (error: AxiosError): Promise<AxiosError> => {
  return Promise.reject(error);
};

export const makeRequest = (axiosInstance: AxiosInstance): AxiosInstance => {
  axiosInstance.interceptors.request.use(onRequest, onRequestError);
  axiosInstance.interceptors.response.use(onResponse, onResponseError);
  return axiosInstance;
};
