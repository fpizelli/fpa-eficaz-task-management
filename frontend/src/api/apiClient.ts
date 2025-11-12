
const API_BASE_URL = 'http://localhost:5077/api'

function getAuthToken(): string | null {
  return localStorage.getItem('token')
}

function createAuthHeaders(): HeadersInit {
  const token = getAuthToken()
  const headers: HeadersInit = {
    'Content-Type': 'application/json',
  }
  
  if (token) {
    headers['Authorization'] = `Bearer ${token}`
  }
  
  return headers
}

export const apiClient = {
  async get<T>(endpoint: string): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'GET',
      headers: createAuthHeaders(),
    })
    
    if (!response.ok) {
      throw new Error(`Erro ${response.status}: ${response.statusText}`)
    }
    
    return response.json()
  },

  async post<T>(endpoint: string, data?: any): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      headers: createAuthHeaders(),
      body: data ? JSON.stringify(data) : undefined,
    })
    
    if (!response.ok) {
      throw new Error(`Erro ${response.status}: ${response.statusText}`)
    }
    
    return response.json()
  },

  async put<T>(endpoint: string, data?: any): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'PUT',
      headers: createAuthHeaders(),
      body: data ? JSON.stringify(data) : undefined,
    })
    
    if (!response.ok) {
      throw new Error(`Erro ${response.status}: ${response.statusText}`)
    }
    
    return response.json()
  },

  async delete(endpoint: string): Promise<void> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'DELETE',
      headers: createAuthHeaders(),
    })
    
    if (!response.ok) {
      throw new Error(`Erro ${response.status}: ${response.statusText}`)
    }
  },

  async patch<T>(endpoint: string, data?: any): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      method: 'PATCH',
      headers: createAuthHeaders(),
      body: data ? JSON.stringify(data) : undefined,
    })
    
    if (!response.ok) {
      throw new Error(`Erro ${response.status}: ${response.statusText}`)
    }
    
    return response.json()
  }
}
