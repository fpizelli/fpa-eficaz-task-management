import { useAuth } from './useAuth'
import { Role } from '@/types/role'

export function usePermissions() {
  const { user } = useAuth()

  const userRole = user?.role

  const canViewUsers = () => {
    return true
  }

  const canCreateUsers = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }

  const canEditUsers = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }

  const canDeleteUsers = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }

  const canEditUserRoles = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }

  const canCreateTasks = () => {
    return true
  }

  const canEditTasks = () => {
    return true
  }

  const canDeleteTasks = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }


  const isAdmin = () => {
    return userRole === Role.Gerente || userRole === Role.Admin
  }

  return {
    canViewUsers,
    canCreateUsers,
    canEditUsers,
    canDeleteUsers,
    canEditUserRoles,
    canCreateTasks,
    canEditTasks,
    canDeleteTasks,
    isAdmin,
    userRole
  }
}
