import { useState, useEffect } from 'react';
import { authService } from '@/services/authService';
import type { LoginRequest, RegisterRequest, AuthResponse } from '@/services/authService';
import { parseRole } from '@/types/role';

export interface AuthState {
  isAuthenticated: boolean;
  user: any | null;
  loading: boolean;
}

export function useAuth() {
  const [authState, setAuthState] = useState<AuthState>({
    isAuthenticated: false,
    user: null,
    loading: true
  });

  useEffect(() => {
    const checkAuth = () => {
      const token = authService.getToken();
      const savedUser = authService.getUser();
      
      if (token && savedUser) {
        setAuthState({
          isAuthenticated: true,
          user: savedUser,
          loading: false
        });
      } else {
        const isFirstTime = !localStorage.getItem('app_initialized');
        
        if (isFirstTime) {
          localStorage.setItem('app_initialized', 'true');
          
          const tempUser = {
            id: "135732ff-9500-4468-b406-1350ee0c3e81",
            name: "Carlos Silva",
            email: "carlos.admin@eficaz.com",
            role: parseRole("Admin"),
            createdAt: new Date().toISOString()
          };
          
          localStorage.setItem('user', JSON.stringify(tempUser));
          localStorage.setItem('token', 'temp-dev-token');
          
          setAuthState({
            isAuthenticated: true,
            user: tempUser,
            loading: false
          });
        } else {
          setAuthState({
            isAuthenticated: false,
            user: null,
            loading: false
          });
        }
      }
    };

    checkAuth();
    authService.setupInterceptors();
  }, []);

  const login = async (credentials: LoginRequest): Promise<AuthResponse> => {
    setAuthState(prev => ({ ...prev, loading: true }));
    
    await new Promise(resolve => setTimeout(resolve, 500));
    
    if (credentials.email && credentials.password) {
      const fakeUser = {
        id: "135732ff-9500-4468-b406-1350ee0c3e81",
        name: "Carlos Silva",
        email: credentials.email,
        role: parseRole("Admin"),
        createdAt: new Date().toISOString()
      };
      
      localStorage.setItem('user', JSON.stringify(fakeUser));
      localStorage.setItem('token', 'fake-auth-token');
      
      setAuthState({
        isAuthenticated: true,
        user: fakeUser,
        loading: false
      });
      
      window.location.href = '/';
      
      return {
        success: true,
        user: fakeUser,
        message: 'Login realizado com sucesso'
      };
    } else {
      setAuthState(prev => ({ ...prev, loading: false }));
      return {
        success: false,
        message: 'Email e senha são obrigatórios'
      };
    }
  };

  const register = async (userData: RegisterRequest): Promise<AuthResponse> => {
    setAuthState(prev => ({ ...prev, loading: true }));
    
    try {
      const response = await authService.register(userData);
      
      if (response.success) {
        setAuthState({
          isAuthenticated: true,
          user: response.user || null,
          loading: false
        });
      } else {
        setAuthState(prev => ({ ...prev, loading: false }));
      }
      
      return response;
    } catch (error) {
      setAuthState(prev => ({ ...prev, loading: false }));
      return {
        success: false,
        message: 'Erro interno do sistema'
      };
    }
  };

  const logout = () => {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    
    setAuthState({
      isAuthenticated: false,
      user: null,
      loading: false
    });
    
    window.location.href = '/login';
  };

  const checkEmail = async (email: string): Promise<boolean> => {
    return await authService.checkEmail(email);
  };

  return {
    ...authState,
    login,
    register,
    logout,
    checkEmail
  };
}
