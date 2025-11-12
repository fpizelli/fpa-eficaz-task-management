"use client"

import { useState, useEffect } from "react"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import { Calendar01 } from "@/components/calendar-01"
import { CalendarIcon } from "lucide-react"
import { format } from "date-fns"
import { ptBR } from "date-fns/locale"
import { createTask } from "@/api/taskApi"
import { userApi } from "@/api/userApi"
import type { CreateTaskDto } from "@/api/types"
import type { User } from "@/types/user"

interface NewTaskModalProps {
  isOpen: boolean
  onClose: () => void
  onSave?: (task: NewTaskData) => void
}

interface NewTaskData {
  title: string
  description: string
  impact: string
  effort: string
  urgency: string
  assignee: string
  dueDate: Date | undefined
}

const getAvatarColor = (initials: string) => {
  const colors = [
    "bg-red-500 text-white",
    "bg-blue-500 text-white", 
    "bg-green-500 text-white",
    "bg-purple-500 text-white",
    "bg-orange-500 text-white",
    "bg-pink-500 text-white",
    "bg-indigo-500 text-white",
    "bg-teal-500 text-white"
  ]
  
  const index = initials.charCodeAt(0) % colors.length
  return colors[index]
}

const getInitials = (name: string | undefined | null) => {
  if (!name || typeof name !== 'string') return '??'
  return name.split(' ').map(word => word[0]).join('').toUpperCase().slice(0, 2)
}

const mapValueToNumber = (value: string): number => {
  switch (value) {
    case "Alto": return 10
    case "Médio": return 5
    case "Baixo": return 1
    case "Alta": return 10
    case "Média": return 5
    case "Baixa": return 1
    default: return 5
  }
}

export function NewTaskModal({ isOpen, onClose, onSave }: NewTaskModalProps) {
  const [formData, setFormData] = useState<NewTaskData>({
    title: "",
    description: "",
    impact: "",
    effort: "",
    urgency: "",
    assignee: "",
    dueDate: undefined
  })
  const [isLoading, setIsLoading] = useState(false)
  const [users, setUsers] = useState<User[]>([])
  const [loadingUsers, setLoadingUsers] = useState(false)

  useEffect(() => {
    if (isOpen) {
      loadUsers()
    }
  }, [isOpen])

  const loadUsers = async () => {
    try {
      setLoadingUsers(true)
      const usersData = await userApi.getUsers()
      setUsers(usersData)
    } catch (error) {
      console.error('Erro ao carregar usuários:', error)
    } finally {
      setLoadingUsers(false)
    }
  }

  const handleSave = async () => {
    if (formData.title && formData.description && formData.impact && formData.effort && formData.urgency && formData.assignee) {
      try {
        setIsLoading(true)
        
        const selectedUser = users.find(user => user.name === formData.assignee)
        
        const createTaskDto: CreateTaskDto = {
          Title: formData.title,
          Description: formData.description,
          Impact: mapValueToNumber(formData.impact),
          Effort: mapValueToNumber(formData.effort),
          Urgency: mapValueToNumber(formData.urgency),
          UserId: selectedUser?.id
        }

        await createTask(createTaskDto)
        
        if (onSave) {
          onSave(formData)
        }
        
        if ((window as any).reloadTasks) {
          (window as any).reloadTasks()
        }
        
        handleClose()
      } catch (error) {
        console.error("Erro ao criar tarefa:", error)
        alert("Erro ao criar tarefa. Tente novamente.")
      } finally {
        setIsLoading(false)
      }
    }
  }

  const handleClose = () => {
    setFormData({
      title: "",
      description: "",
      impact: "",
      effort: "",
      urgency: "",
      assignee: "",
      dueDate: undefined
    })
    onClose()
  }


  return (
    <Dialog open={isOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-6xl w-full max-h-[90vh] overflow-y-auto p-8">
        <DialogHeader>
          <DialogTitle className="text-lg font-semibold">Nova tarefa</DialogTitle>
        </DialogHeader>
        
        <div className="space-y-6 py-4">
          <div className="space-y-2">
            <Label htmlFor="title">Título *</Label>
            <Input
              id="title"
              placeholder="Digite o título da tarefa..."
              value={formData.title}
              onChange={(e) => setFormData(prev => ({ ...prev, title: e.target.value }))}
              className="break-words"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="description">Descrição *</Label>
            <Textarea
              id="description"
              value={formData.description}
              onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
              placeholder="Descreva a tarefa..."
              className="min-h-[120px] resize-none"
              style={{ 
                wordWrap: 'break-word', 
                overflowWrap: 'break-word', 
                whiteSpace: 'pre-wrap',
                wordBreak: 'break-word'
              }}
            />
          </div>

          <div className="space-y-6">
            <Label>Prioridade *</Label>
            <div className="w-full max-w-3xl">
              <div className="flex justify-between mb-4">
                <Label className="text-xs font-normal text-muted-foreground">Impacto</Label>
                <Label className="text-xs font-normal text-muted-foreground">Esforço</Label>
                <Label className="text-xs font-normal text-muted-foreground">Urgência</Label>
              </div>
              
              <div className="flex justify-between">
                <div className="space-y-2">
                  <RadioGroup value={formData.impact} onValueChange={(value) => setFormData(prev => ({ ...prev, impact: value }))}>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Alto" id="impact-alto" />
                      <Label htmlFor="impact-alto" className="text-sm font-normal">Alto</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Médio" id="impact-medio" />
                      <Label htmlFor="impact-medio" className="text-sm font-normal">Médio</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Baixo" id="impact-baixo" />
                      <Label htmlFor="impact-baixo" className="text-sm font-normal">Baixo</Label>
                    </div>
                  </RadioGroup>
                </div>

                <div className="space-y-2">
                  <RadioGroup value={formData.effort} onValueChange={(value) => setFormData(prev => ({ ...prev, effort: value }))}>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Alto" id="effort-alto" />
                      <Label htmlFor="effort-alto" className="text-sm font-normal">Alto</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Médio" id="effort-medio" />
                      <Label htmlFor="effort-medio" className="text-sm font-normal">Médio</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Baixo" id="effort-baixo" />
                      <Label htmlFor="effort-baixo" className="text-sm font-normal">Baixo</Label>
                    </div>
                  </RadioGroup>
                </div>

                <div className="space-y-2">
                  <RadioGroup value={formData.urgency} onValueChange={(value) => setFormData(prev => ({ ...prev, urgency: value }))}>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Alta" id="urgency-alta" />
                      <Label htmlFor="urgency-alta" className="text-sm font-normal">Alta</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Média" id="urgency-media" />
                      <Label htmlFor="urgency-media" className="text-sm font-normal">Média</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="Baixa" id="urgency-baixa" />
                      <Label htmlFor="urgency-baixa" className="text-sm font-normal">Baixa</Label>
                    </div>
                  </RadioGroup>
                </div>
              </div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div className="space-y-2">
              <Label htmlFor="assignee" className="text-sm font-medium">Responsável *</Label>
              <Select value={formData.assignee} onValueChange={(value) => setFormData(prev => ({ ...prev, assignee: value }))}>
                <SelectTrigger id="assignee">
                  <SelectValue placeholder="Selecione o responsável" />
                </SelectTrigger>
                <SelectContent>
                  {loadingUsers ? (
                    <div className="p-2 text-center text-sm text-muted-foreground">
                      Carregando usuários...
                    </div>
                  ) : users.length === 0 ? (
                    <div className="p-2 text-center text-sm text-muted-foreground">
                      Nenhum usuário encontrado
                    </div>
                  ) : (
                    users.map((user) => (
                      <SelectItem key={user.id} value={user.name}>
                        <div className="flex items-center gap-3">
                          <Avatar className="h-6 w-6">
                            <AvatarImage src="" />
                            <AvatarFallback className={`text-xs ${getAvatarColor(getInitials(user.name))}`}>
                              {getInitials(user.name)}
                            </AvatarFallback>
                          </Avatar>
                          <span>{user.name}</span>
                        </div>
                      </SelectItem>
                    ))
                  )}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="dueDate" className="text-sm font-medium">Prazo</Label>
              <Popover>
                <PopoverTrigger asChild>
                  <Button
                    id="dueDate"
                    variant="outline"
                    className={`w-full justify-start text-left font-normal ${
                      !formData.dueDate && "text-muted-foreground"
                    }`}
                  >
                    <CalendarIcon className="mr-2 h-4 w-4" />
                    {formData.dueDate ? (
                      format(formData.dueDate, "PPP", { locale: ptBR })
                    ) : (
                      <span>Selecione uma data</span>
                    )}
                  </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                  <Calendar01 
                    selected={formData.dueDate}
                    onSelect={(date) => setFormData(prev => ({ ...prev, dueDate: date }))}
                  />
                </PopoverContent>
              </Popover>
            </div>
          </div>

        </div>

        <div className="flex justify-end pt-4">
          <Button 
            onClick={handleSave}
            disabled={isLoading || !formData.title || !formData.description || !formData.impact || !formData.effort || !formData.urgency || !formData.assignee}
            size="sm"
            className="h-8 min-w-[100px]"
          >
            {isLoading ? "Criando..." : "Criar"}
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  )
}
