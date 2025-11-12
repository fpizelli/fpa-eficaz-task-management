
import { useState, useEffect } from 'react'
import { LoadingSpinner } from '@/components/ui/loading-spinner'
import { ErrorState } from '@/components/ui/error-state'
import { userApi } from '@/api/userApi'
import type { User } from '@/types/user'
import { UserCreateModal } from '@/features/users/components/UserCreateModal'
import { UserEditModal } from '@/features/users/components/UserEditModal'
import { usePermissions } from '@/hooks/usePermissions'
import { useAuth } from '@/hooks/useAuth'
import { UserControlButtons } from '@/features/users/components/UserControlButtons'
import { UserDataTable } from '@/features/users/components/UserDataTable'
import { usePageTitle } from '@/hooks/usePageTitle'

export function TeamView() {
  usePageTitle("Equipe | Eficaz")
  
  const [users, setUsers] = useState<User[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [selectedUser, setSelectedUser] = useState<User | null>(null)
  const [selectedView, setSelectedView] = useState("visao-geral")
  
  const { user, loading: authLoading } = useAuth()
  const { canEditUsers, canDeleteUsers } = usePermissions()

  const fetchUsers = async () => {
    try {
      setLoading(true)
      const data = await userApi.getUsers()
      setUsers(data)
      setError(null)
    } catch (err: any) {
      console.error('Erro ao buscar usuários:', err)
      if (err.response?.status === 401) {
        setError('Você precisa fazer login para visualizar usuários.')
      } else if (err.response?.status === 403) {
        setError('Acesso negado. Verifique suas permissões.')
      } else {
        setError('Erro ao carregar usuários. Tente novamente.')
      }
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (!authLoading && user) {
      fetchUsers()
    }
  }, [authLoading, user])


  const handleDeleteUser = async (userId: string) => {
    if (window.confirm('Tem certeza que deseja excluir este usuário?')) {
      try {
        await userApi.deleteUser(userId)
        await fetchUsers()
      } catch (err) {
        console.error('Erro ao deletar usuário:', err)
        alert('Erro ao deletar usuário')
      }
    }
  }

  const handleEditUser = (user: User) => {
    setSelectedUser(user)
    setIsEditModalOpen(true)
  }

  const handleModalSuccess = () => {
    setIsCreateModalOpen(false)
    setIsEditModalOpen(false)
    setSelectedUser(null)
    fetchUsers()
  }

  const handleModalCancel = () => {
    setIsCreateModalOpen(false)
    setIsEditModalOpen(false)
    setSelectedUser(null)
  }

  const handleAddSection = () => {
    setIsCreateModalOpen(true)
  }

  const handleViewChange = (view: string) => {
    setSelectedView(view)
    console.log(`Visualização alterada para: ${view}`, { currentView: selectedView, newView: view })
  }

  if (authLoading || loading) {
    return <LoadingSpinner message="Carregando usuários..." />
  }

  if (error) {
    return <ErrorState message={error} onRetry={fetchUsers} />
  }

  return (
    <>
      <div className="@container/main flex flex-1 flex-col gap-2">
        <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6 px-4 lg:px-6">
          <UserControlButtons 
            onAddUser={handleAddSection}
            users={users}
            onViewChange={handleViewChange}
          />
          <UserDataTable 
            data={users}
            onEditUser={handleEditUser}
            onDeleteUser={handleDeleteUser}
            canEdit={canEditUsers()}
            canDelete={canDeleteUsers()}
            roleFilter={selectedView}
          />
        </div>
      </div>

      <UserCreateModal 
        isOpen={isCreateModalOpen} 
        onClose={handleModalCancel}
        onSuccess={handleModalSuccess}
      />
      
      {selectedUser && (
        <UserEditModal 
          isOpen={isEditModalOpen} 
          onClose={handleModalCancel}
          onSuccess={handleModalSuccess}
          user={selectedUser}
        />
      )}
    </>
  )
}
