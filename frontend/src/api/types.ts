// Tipos para integração com API
export type TaskStatus = 'Todo' | 'InProgress' | 'Done' | 'Archived'

export interface TaskDto {
  id: string
  title: string
  description?: string
  effort: number
  impact: number
  urgency: number
  priorityScore: number
  status: TaskStatus
  projectId?: string
  userId?: string
  userName?: string
  projectName?: string
  createdAt: string
}

export interface CreateTaskDto {
  Title: string
  Description?: string
  Impact: number
  Effort: number
  Urgency: number
  UserId?: string
}

export interface UpdateTaskDto extends CreateTaskDto {
  id: string
  status: TaskStatus
}

export interface UserDto {
  id: string
  name: string
  email: string
  avatar?: string
}

export interface UpdateTaskStatusDto {
  status: TaskStatus
}

export interface CommentDto {
  id: string
  taskId: string
  userId: string
  userName: string
  content: string
  createdAt: string
}

export interface CreateCommentDto {
  taskId: string
  userId: string
  content: string
}

export interface AuditLogDto {
  id: string
  taskId: string
  userId: string
  userName: string
  userAvatar: string
  action: string
  details: string
  createdAt: string
}
