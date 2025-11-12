
import { apiClient } from './apiClient'

export const api = {
  get: async <T>(endpoint: string): Promise<{ data: T }> => {
    const result = await apiClient.get<T>(endpoint)
    return { data: result }
  },
  post: async <T>(endpoint: string, data?: any): Promise<{ data: T }> => {
    const result = await apiClient.post<T>(endpoint, data)
    return { data: result }
  },
  put: async <T>(endpoint: string, data?: any): Promise<{ data: T }> => {
    const result = await apiClient.put<T>(endpoint, data)
    return { data: result }
  },
  delete: async (endpoint: string): Promise<void> => {
    await apiClient.delete(endpoint)
  },
  patch: async <T>(endpoint: string, data?: any): Promise<{ data: T }> => {
    const result = await apiClient.patch<T>(endpoint, data)
    return { data: result }
  }
}

export default api
