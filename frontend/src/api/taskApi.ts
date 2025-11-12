import { api } from './axios'
import type { TaskDto, CreateTaskDto, TaskStatus } from './types'

export const taskApi = {
  async getAll(): Promise<TaskDto[]> {
    const response = await api.get<TaskDto[]>('/tasks')
    return response.data
  },

  async getById(id: string): Promise<TaskDto> {
    const response = await api.get<TaskDto>(`/tasks/${id}`)
    return response.data
  },

  async create(task: CreateTaskDto): Promise<TaskDto> {
    const response = await api.post<TaskDto>('/tasks', task)
    return response.data
  },

  async update(id: string, task: Partial<TaskDto>): Promise<TaskDto> {
    const response = await api.put<TaskDto>(`/tasks/${id}`, task)
    return response.data
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/tasks/${id}`)
  },

  async moveStatus(id: string, status: TaskStatus): Promise<TaskDto> {
    const statusPayload = {
      Status: status
    }
    
    const response = await api.put<TaskDto>(`/tasks/${id}/status`, statusPayload)
    return response.data
  },

  async recalculatePriorities(): Promise<{ message: string; count: number }> {
    const response = await api.post<{ message: string; count: number }>('/tasks/recalculate')
    return response.data
  }
}

export const getTasks = taskApi.getAll
export const getTask = taskApi.getById
export const createTask = taskApi.create
export const updateTask = taskApi.update
export const deleteTask = taskApi.delete
export const moveTask = taskApi.moveStatus
export const recalculatePriorities = taskApi.recalculatePriorities
