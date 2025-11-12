import type { User, CreateUserDto, UpdateUserDto } from '@/types/user'
import { api } from './axios'

export const userApi = {
  async getUsers(): Promise<User[]> {
    const timestamp = new Date().getTime()
    const response = await api.get<User[]>(`/tasks/users?_t=${timestamp}`)
    return response.data
  },

  async getUserById(id: string): Promise<User> {
    const response = await api.get<User>(`/tasks/users/${id}`)
    return response.data
  },

  async createUser(userData: CreateUserDto): Promise<any> {
    const response = await api.post<any>('/tasks/users', userData)
    return response.data
  },

  async updateUser(userData: UpdateUserDto): Promise<any> {
    const response = await api.put<any>(`/tasks/users/${userData.id}`, userData)
    return response.data
  },

  async deleteUser(id: string): Promise<void> {
    await api.delete(`/tasks/users/${id}`)
  },
}
