import * as React from "react"
import { useState } from "react"
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs"
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
import { Role, RoleLabels } from "@/types/role"
import type { CreateTaskDto } from "@/api/types"

interface QuickActionModalProps {
  isOpen: boolean
  onClose: () => void
}

const availableUsers = [
  { name: "João Silva", avatar: "JS" },
  { name: "Maria Santos", avatar: "MS" }
]

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

export function QuickActionModal({ isOpen, onClose }: QuickActionModalProps) {
  const [activeTab, setActiveTab] = useState("task")

  const [taskForm, setTaskForm] = useState({
    title: "",
    description: "",
    impact: "",
    effort: "",
    urgency: "",
    assignee: "",
    dueDate: undefined as Date | undefined
  })
  const [taskLoading, setTaskLoading] = useState(false)

  const [userForm, setUserForm] = useState({
    name: "",
    email: "",
    avatar: ""
  })
  const [userLoading, setUserLoading] = useState(false)

  React.useEffect(() => {
    if (isOpen) {
      setActiveTab("task")
      setTaskForm({ title: "", description: "", impact: "", effort: "", urgency: "", assignee: "", dueDate: undefined })
      setUserForm({ name: "", email: "", avatar: "" })
    }
  }, [isOpen])

  const handleCreateTask = async () => {
    if (!taskForm.title || !taskForm.description || !taskForm.impact || !taskForm.effort || !taskForm.urgency || !taskForm.assignee) {
      return
    }

    try {
      setTaskLoading(true)
      
      const createTaskDto: CreateTaskDto = {
        Title: taskForm.title,
        Description: taskForm.description,
        Impact: mapValueToNumber(taskForm.impact),
        Effort: mapValueToNumber(taskForm.effort),
        Urgency: mapValueToNumber(taskForm.urgency)
      }

      await createTask(createTaskDto)
      
      if ((window as any).reloadTasks) {
        (window as any).reloadTasks()
      }
      
      onClose()
    } catch (error) {
      console.error("Erro ao criar tarefa:", error)
      alert("Erro ao criar tarefa. Tente novamente.")
    } finally {
      setTaskLoading(false)
    }
  }

  const handleCreateUser = async () => {
    if (!userForm.name.trim() || !userForm.email.trim()) return

    try {
      setUserLoading(true)
      await userApi.createUser({
        name: userForm.name.trim(),
        email: userForm.email.trim(),
        password: "senhapadrao",
        role: RoleLabels[Role.Desenvolvedor]
      })
      
      if (window.location.pathname === '/equipe') {
        window.location.reload()
      }
      
      onClose()
    } catch (error) {
      console.error("Erro ao criar usuário:", error)
      alert("Erro ao criar usuário. Tente novamente.")
    } finally {
      setUserLoading(false)
    }
  }

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="max-w-4xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Ação rápida</DialogTitle>
        </DialogHeader>
        
        <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="task">Nova tarefa</TabsTrigger>
            <TabsTrigger value="user">Novo usuário</TabsTrigger>
          </TabsList>
          
          <TabsContent value="task" className="mt-6 space-y-6">
            <div className="space-y-2">
              <Label htmlFor="task-title">Título *</Label>
              <Input
                id="task-title"
                placeholder="Digite o título da tarefa..."
                value={taskForm.title}
                onChange={(e) => setTaskForm(prev => ({ ...prev, title: e.target.value }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="task-description">Descrição *</Label>
              <Textarea
                id="task-description"
                placeholder="Descreva a tarefa em detalhes..."
                className="min-h-[100px] resize-none"
                style={{ 
                  wordWrap: 'break-word', 
                  overflowWrap: 'break-word', 
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word'
                }}
                value={taskForm.description}
                onChange={(e) => setTaskForm(prev => ({ ...prev, description: e.target.value }))}
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
                    <RadioGroup value={taskForm.impact} onValueChange={(value) => setTaskForm(prev => ({ ...prev, impact: value }))}>
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
                    <RadioGroup value={taskForm.effort} onValueChange={(value) => setTaskForm(prev => ({ ...prev, effort: value }))}>
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
                    <RadioGroup value={taskForm.urgency} onValueChange={(value) => setTaskForm(prev => ({ ...prev, urgency: value }))}>
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
                <Select value={taskForm.assignee} onValueChange={(value) => setTaskForm(prev => ({ ...prev, assignee: value }))}>
                  <SelectTrigger id="assignee">
                    <SelectValue placeholder="Selecione o responsável" />
                  </SelectTrigger>
                  <SelectContent>
                    {availableUsers.map((user) => (
                      <SelectItem key={user.name} value={user.name}>
                        <div className="flex items-center gap-3">
                          <Avatar className="h-6 w-6">
                            <AvatarImage src="" />
                            <AvatarFallback className={`text-xs ${getAvatarColor(user.avatar)}`}>
                              {user.avatar}
                            </AvatarFallback>
                          </Avatar>
                          <span>{user.name}</span>
                        </div>
                      </SelectItem>
                    ))}
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
                        !taskForm.dueDate && "text-muted-foreground"
                      }`}
                    >
                      <CalendarIcon className="mr-2 h-4 w-4" />
                      {taskForm.dueDate ? (
                        format(taskForm.dueDate, "PPP", { locale: ptBR })
                      ) : (
                        <span>Selecione uma data</span>
                      )}
                    </Button>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar01 
                      selected={taskForm.dueDate}
                      onSelect={(date) => setTaskForm(prev => ({ ...prev, dueDate: date }))}
                    />
                  </PopoverContent>
                </Popover>
              </div>
            </div>

            <div className="flex justify-end pt-6">
              <Button 
                onClick={handleCreateTask}
                disabled={taskLoading || !taskForm.title || !taskForm.description || !taskForm.impact || !taskForm.effort || !taskForm.urgency || !taskForm.assignee}
                size="sm"
                className="h-8 min-w-[100px]"
              >
                {taskLoading ? "Criando..." : "Criar"}
              </Button>
            </div>
          </TabsContent>
          
          <TabsContent value="user" className="mt-6 space-y-6">
            <div className="space-y-2">
              <Label htmlFor="user-name">Nome *</Label>
              <Input
                id="user-name"
                placeholder="Digite o nome completo"
                value={userForm.name}
                onChange={(e) => setUserForm(prev => ({ ...prev, name: e.target.value }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="user-email">E-mail *</Label>
              <Input
                id="user-email"
                type="email"
                placeholder="Digite o email"
                value={userForm.email}
                onChange={(e) => setUserForm(prev => ({ ...prev, email: e.target.value }))}
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="user-avatar">URL do avatar</Label>
              <Input
                id="user-avatar"
                placeholder="https://exemplo.com/avatar.jpg"
                value={userForm.avatar}
                onChange={(e) => setUserForm(prev => ({ ...prev, avatar: e.target.value }))}
              />
            </div>

            <div className="flex justify-end pt-6">
              <Button 
                onClick={handleCreateUser}
                disabled={userLoading || !userForm.name.trim() || !userForm.email.trim()}
                size="sm"
                className="h-8 min-w-[100px]"
              >
                {userLoading ? "Criando..." : "Criar"}
              </Button>
            </div>
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  )
}
