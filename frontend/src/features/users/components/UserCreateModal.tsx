import { useState } from 'react'
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
import type { UserFormData } from '@/types/user'
import { Role, RoleLabels } from '@/types/role'

interface UserCreateModalProps {
  isOpen: boolean
  onClose: () => void
  onSuccess?: () => void
}

export function UserCreateModal({ isOpen, onClose, onSuccess }: UserCreateModalProps) {
  const [formData, setFormData] = useState<UserFormData>({
    name: '',
    email: '',
    avatar: undefined,
    role: Role.Desenvolvedor
  })
  const [isLoading, setIsLoading] = useState(false)
  const [errors, setErrors] = useState<Record<string, string>>({})

  const validateForm = (): boolean => {
    const newErrors: Record<string, string> = {}

    if (!formData.name.trim()) {
      newErrors.name = 'Nome é obrigatório'
    }

    if (!formData.email.trim()) {
      newErrors.email = 'E-mail é obrigatório'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'E-mail inválido'
    }

    setErrors(newErrors)
    return Object.keys(newErrors).length === 0
  }

  const handleSave = async () => {
    if (!validateForm()) return

    try {
      setIsLoading(true)
      await userApi.createUser({
        name: formData.name.trim(),
        email: formData.email.trim(),
        password: "senhapadrao",
        role: RoleLabels[formData.role]
      })
      
      setFormData({
        name: '',
        email: '',
        avatar: undefined,
        role: Role.Desenvolvedor
      })
      setErrors({})
      onSuccess?.()
      onClose()
    } catch (error: any) {
      console.error('Erro ao criar usuário:', error)
      let errorMessage = 'Erro ao criar usuário. Tente novamente.'
      
      if (error?.response?.data?.message) {
        errorMessage = error.response.data.message
      } else if (error?.message) {
        errorMessage = error.message
      }
      
      setErrors({ general: errorMessage })
    } finally {
      setIsLoading(false)
    }
  }

  const handleClose = () => {
    setFormData({
      name: '',
      email: '',
      avatar: undefined,
      role: Role.Desenvolvedor
    })
    setErrors({})
    onClose()
  }

  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-md">
        <DialogHeader>
          <DialogTitle>Adicionar usuário</DialogTitle>
        </DialogHeader>
        
        <div className="space-y-4 py-4">
          {errors.general && (
            <div className="text-sm text-destructive bg-destructive/10 p-3 rounded-md">
              {errors.general}
            </div>
          )}

          <div className="space-y-2">
            <Label htmlFor="name">Nome *</Label>
            <Input
              id="name"
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
            <Label htmlFor="email">E-mail *</Label>
            <Input
              id="email"
              type="email"
              placeholder="Digite o e-mail"
              value={formData.email}
              onChange={(e) => setFormData(prev => ({ ...prev, email: e.target.value }))}
              className={errors.email ? 'border-destructive' : ''}
            />
            {errors.email && (
              <p className="text-sm text-destructive">{errors.email}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="role">Atribuição *</Label>
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
            <Label htmlFor="avatar">URL do avatar</Label>
            <Input
              id="avatar"
              placeholder="https://exemplo.com/avatar.jpg"
              value={formData.avatar || ''}
              onChange={(e) => setFormData(prev => ({ ...prev, avatar: e.target.value }))}
            />
          </div>
        </div>

        <div className="flex justify-end pt-4">
          <Button 
            onClick={handleSave}
            disabled={isLoading}
            size="sm"
            className="h-8 min-w-[100px]"
          >
            {isLoading ? 'Criando...' : 'Criar'}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  )
}
