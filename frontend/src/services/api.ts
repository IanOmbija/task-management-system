import axios from 'axios';

// Base API URL from env
const API_BASE = process.env.REACT_APP_API_URL;
console.log("API_URL:", API_BASE);

export const api = axios.create({
  baseURL: `${API_BASE}/api`,
  headers: { 'Content-Type': 'application/json' },
});


export function attachInterceptors(
  getToken: () => string | null,
  onUnauthorized: () => void
) {
  
  api.interceptors.request.use((config) => {
    const token = getToken();
    console.log("Attached the token here!");
    if (token) {
      config.headers = config.headers || {};
      (config.headers as any).Authorization = `Bearer ${token}`;
    }
    return config;
  });

  
  api.interceptors.response.use(
    (res) => res,
    (err) => {
      if (err?.response?.status === 401) {
        onUnauthorized();
      }
      return Promise.reject(err);
    }
  );
}
