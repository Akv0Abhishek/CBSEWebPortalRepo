import axios from 'axios';
import { API_URL } from './endPoints';

const authService = {
  login: async (email, password, role) => {
    const endpoint = role === 'admin' ? '/Login/admin' : '/Login/principal';
    const response = await axios.post(`${API_URL}${endpoint}`, { email, password });
    const token = response.data.token;
    const PrincipalId = response.data.principalId;

    console.log('Response:', response);
    console.log('PrincipalId:', PrincipalId);
    if(token){
      localStorage.setItem('authToken', token);
      localStorage.setItem('principalId',PrincipalId);
    }
    return response.data;
  },
  getToken: () =>{
    return localStorage.getItem('authToken');
  },
  logout: () =>{
    localStorage.removeItem('authToken');
  },
};

export default authService;
