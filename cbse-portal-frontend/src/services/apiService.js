import axios from 'axios';
import { API_URL } from './endPoints';

const apiService = {
  registerAdmin: async (data) => {
    const response = await axios.post(`${API_URL}/Signup/register-admin`, data);
    return response.data;
  },

  registerPrincipal: async (data) => {
    const response = await axios.post(`${API_URL}/Signup/register-principal`, data);
    return response.data;
  },
};

export default apiService;
