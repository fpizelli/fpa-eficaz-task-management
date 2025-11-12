import { Role } from './role'

export interface User {
  id: string
  name: string
  email: string
  role: Role
  avatar?: string
  createdAt: string
  updatedAt?: string
}

export interface CreateUserDto {
  name: string
  email: string
  password: string
  role: string
}

export interface UpdateUserDto {
  id: string
  name: string
  email: string
  role: Role | string
  avatar?: string
}

export interface UserFormData {
  name: string
  email: string
  password?: string
  role: Role
  avatar?: string
}
