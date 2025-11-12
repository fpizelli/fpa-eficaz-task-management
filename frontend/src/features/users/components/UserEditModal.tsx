import { useState, useEffect } from 'react'
import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { userApi } from '@/api/userApi'
import type { User, UserFormData } from '@/types/user'
import { Role, RoleLabels, parseRole } from '@/types/role'

interface UserEditModalProps {
  isOpen: boolean
  onClose: () => void
  onSuccess?: () => void
  user: User
}

export function UserEditModal({ isOpen, onClose, onSuccess, user }: UserEditModalProps) {
  const [formData, setFormData] = useState<UserFormData>({
    name: '',
    email: '',
    avatar: '',
    role: Role.Desenvolvedor
  })
  const [isLoading, setIsLoading] = useState(false)
  const [errors, setErrors] = useState<Record<string, string>>({})

  useEffect(() => {
    if (user) {
      const parsedRole = parseRole(user.role)
      setFormData({
        name: user.name,
        email: user.email,
        role: parsedRole,
        avatar: user.avatar || ''
      })
    }
  }, [user])

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {}

    if (!formData.name.trim()) {
      newErrors.name = 'Nome é obrigatório'
    }

    if (!formData.email.trim()) {
      newErrors.email = 'Email é obrigatório'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Email inválido'
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSave = async () => {
    if (!validateForm()) return

    try {
      setIsLoading(true)
      const roleString = (() => {
        switch (formData.role) {
          case Role.Desenvolvedor: return 'Desenvolvedor'
          case Role.QA: return 'QA'
          case Role.Gerente: return 'Gerente'
          case Role.Admin: return 'Admin'
          default: return 'Desenvolvedor'
        }
      })()

      await userApi.updateUser({
        id: user.id,
        name: formData.name.trim(),
        email: formData.email.trim(),
        role: roleString,
        avatar: formData.avatar?.trim() || undefined
      })
      
      setErrors({})
      onSuccess?.()
      onClose()
    } catch (error) {
      console.error('Erro ao atualizar usuário:', error)
      setErrors({ general: 'Erro ao atualizar usuário. Tente novamente.' })
    } finally {
      setIsLoading(false)
    }
  }

  const handleClose = () => {
    setErrors({})
    onClose()
  }

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-md">
        <DialogHeader>
          <DialogTitle>Editar usuário</DialogTitle>
        </DialogHeader>
        
        <div className="space-y-4 py-4">
          {errors.general && (
            <div className="text-sm text-destructive bg-destructive/10 p-3 rounded-md">
              {errors.general}
            </div>
          )}

          <div className="space-y-2">
            <Label htmlFor="edit-name">Nome *</Label>
            <Input
              id="edit-name"
              placeholder="Digite o nome completo"
              value={formData.name}
              onChange={(e) => setFormData(prev => ({ ...prev, name: e.target.value }))}
              className={errors.name ? 'border-destructive' : ''}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="edit-email">E-mail *</Label>
            <Input
              id="edit-email"
              type="email"
              placeholder="Digite o email"
              value={formData.email}
              onChange={(e) => setFormData(prev => ({ ...prev, email: e.target.value }))}
              className={errors.email ? 'border-destructive' : ''}
            />
            {errors.email && (
              <p className="text-sm text-destructive">{errors.email}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="edit-role">Cargo *</Label>
            <Select
              value={formData.role.toString()}
              onValueChange={(value) => setFormData(prev => ({ ...prev, role: parseInt(value) as Role }))}
            >
              <SelectTrigger>
                <SelectValue placeholder="Selecione o cargo" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={Role.Desenvolvedor.toString()}>
                  {RoleLabels[Role.Desenvolvedor]}
                </SelectItem>
                <SelectItem value={Role.QA.toString()}>
                  {RoleLabels[Role.QA]}
                </SelectItem>
                <SelectItem value={Role.Gerente.toString()}>
                  {RoleLabels[Role.Gerente]}
                </SelectItem>
                <SelectItem value={Role.Admin.toString()}>
                  {RoleLabels[Role.Admin]}
                </SelectItem>
              </SelectContent>
            </Select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="edit-avatar">URL do Avatar (opcional)</Label>
            <Input
              id="edit-avatar"
              placeholder="https://exemplo.com/avatar.jpg"
              value={formData.avatar}
              onChange={(e) => setFormData(prev => ({ ...prev, avatar: e.target.value }))}
            />
          </div>
        </div>

        <div className="flex justify-end pt-4 border-t">
          <Button 
            onClick={handleSave}
            disabled={isLoading}
            size="sm"
            className="h-8 min-w-[100px]"
          >
            {isLoading ? 'Salvando...' : 'Salvar'}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  )
}
