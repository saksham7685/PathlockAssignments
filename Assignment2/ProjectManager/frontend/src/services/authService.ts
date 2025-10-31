import { apiClient } from '../utils/apiClient';

interface LoginData {
  username: string;
  password: string;
}

interface RegisterData {
  username: string;
  password: string;
}

export const authService = {
  login: async (data: LoginData) => {
    return apiClient.post<{ token: string; username: string }>('/auth/login', data, false);
  },

  register: async (data: RegisterData) => {
    return apiClient.post<{ token: string; username: string }>('/auth/register', data, false);
  },

  getCurrentUser: () => {
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    return { token, username };
  },

  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
  },

  isAuthenticated: (): boolean => {
    return !!localStorage.getItem('token');
  },
};
