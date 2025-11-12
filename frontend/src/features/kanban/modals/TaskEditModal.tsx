import * as React from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Badge } from "@/components/ui/badge"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { CommentsList } from "@/features/comments/components/CommentsList"
import { AuditLogsList } from "@/features/auditLogs/components/AuditLogsList"
import type { TaskDto } from "@/api/types"
import { updateTask, deleteTask } from "@/api/taskApi"
import { userApi } from "@/api/userApi"
import type { User } from "@/types/user"
import { getUserInitials, getUIAvatarUrl } from "@/utils/avatars"

interface TaskEditModalProps {
  task: TaskDto | null
  open: boolean
  onOpenChange: (open: boolean) => void
  onTaskUpdated?: (task: TaskDto) => void
  onTaskDeleted?: (taskId: string) => void
  onCommentAdded?: (taskId: string) => void
  defaultTab?: 'details' | 'comments' | 'audit'
  initialEditMode?: boolean
}

export function TaskEditModal({ task, open, onOpenChange, onTaskUpdated, onTaskDeleted, onCommentAdded, defaultTab = 'details', initialEditMode = false }: TaskEditModalProps) {
  const [formData, setFormData] = React.useState({
    title: "",
    description: "",
    priority: 0,
    impact: 1,
    effort: 1,
    urgency: 1,
    assigneeId: "",
    assigneeName: ""
  })
  const [isLoading, setIsLoading] = React.useState(false)
  const [isEditing, setIsEditing] = React.useState(false)
  const [users, setUsers] = React.useState<User[]>([])
  const [loadingUsers, setLoadingUsers] = React.useState(false)

  React.useEffect(() => {
    if (open) {
      loadUsers()
    }
  }, [open])

  React.useEffect(() => {
    if (task) {
      setFormData({
        title: task.title,
        description: task.description || "",
        priority: task.priorityScore,
        impact: task.impact || 1,
        effort: task.effort || 1,
        urgency: task.urgency || 1,
        assigneeId: task.userId || "",
        assigneeName: task.userName || ""
      })
    }
  }, [task])

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

  React.useEffect(() => {
    if (open) {
      setIsEditing(initialEditMode)
    }
  }, [open, initialEditMode])

  const handleSave = async () => {
    if (!task) return

    try {
      setIsLoading(true)
      
      const updateData = {
        title: formData.title,
        description: formData.description,
        impact: formData.impact,
        effort: formData.effort,
        urgency: formData.urgency,
        userId: formData.assigneeId
      }

      const updatedTask = await updateTask(task.id, updateData)
      setIsEditing(false)
      onTaskUpdated?.(updatedTask || task)
    } catch (error) {
      console.error('Erro ao atualizar tarefa:', error)
    } finally {
      setIsLoading(false)
    }
  }


  const handleDelete = async () => {
    if (!task) return

    const confirmed = window.confirm('Tem certeza que deseja excluir esta tarefa? Esta ação não pode ser desfeita.')
    if (!confirmed) return

    try {
      setIsLoading(true)
      await deleteTask(task.id)
      onOpenChange(false)
      onTaskDeleted?.(task.id)
    } catch (error) {
      console.error('Erro ao excluir tarefa:', error)
      alert('Erro ao excluir a tarefa. Tente novamente.')
    } finally {
      setIsLoading(false)
    }
  }

  const getInitials = (name: string): string => {
    return name?.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2) || "NA"
  }

  const getAvatarColor = (initials: string): string => {
    const colors = [
      "bg-purple-500 text-white", "bg-orange-500 text-white", "bg-pink-500 text-white"
    ]
    return colors[initials.charCodeAt(0) % colors.length]
  }

  const getPriorityLabel = (score: number): string => {
    if (score >= 7) return "Alta"
    if (score >= 4) return "Média"
    return "Baixa"
  }

  const getPriorityColor = (score: number): string => {
    if (score >= 7) return "bg-foreground text-background border border-foreground"
    if (score >= 4) return "bg-muted text-muted-foreground border border-muted-foreground/30"
    return "bg-background text-foreground border border-border"
  }

  const handlePriorityChange = (priorityLabel: string) => {
    let impact = 1, effort = 1, urgency = 1
    
    switch (priorityLabel) {
      case "Alta":
        impact = 5
        effort = 1
        urgency = 5
        break
      case "Média":
        impact = 3
        effort = 3
        urgency = 3
        break
      case "Baixa":
        impact = 1
        effort = 5
        urgency = 1
        break
    }
    
    const priorityScore = (impact * urgency) / effort
    
    setFormData(prev => ({
      ...prev,
      impact,
      effort,
      urgency,
      priority: priorityScore
    }))
  }



  if (!task) return null

  const initials = getInitials(task.userName || "")

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="text-xl font-semibold">
            {isEditing ? "Editar tarefa" : "Detalhes da tarefa"}
          </DialogTitle>
        </DialogHeader>

        <Tabs defaultValue={defaultTab} className="w-full">
          <TabsList className="grid w-full grid-cols-3">
            <TabsTrigger value="details">Tarefa</TabsTrigger>
            <TabsTrigger value="comments">Comentários</TabsTrigger>
            <TabsTrigger value="audit">Auditoria</TabsTrigger>
          </TabsList>

          <TabsContent value="details" className="space-y-6">

            <div className="space-y-6">
              <div className="space-y-3">
                <Label className="text-sm font-medium">Título</Label>
                {isEditing ? (
                  <Input
                    id="title"
                    value={formData.title}
                    onChange={(e) => setFormData(prev => ({ ...prev, title: e.target.value }))}
                  />
                ) : (
                  <div className="p-3 bg-muted/30 rounded-lg border">
                    <div className="text-sm">{task.title}</div>
                  </div>
                )}
              </div>

              <div className="space-y-3">
                <Label className="text-sm font-medium">Descrição</Label>
                {isEditing ? (
                  <Textarea
                    id="description"
                    value={formData.description}
                    onChange={(e) => setFormData(prev => ({ ...prev, description: e.target.value }))}
                    className="min-h-[100px] resize-none"
                    style={{ 
                      wordWrap: 'break-word', 
                      overflowWrap: 'break-word', 
                      whiteSpace: 'pre-wrap',
                      wordBreak: 'break-word'
                    }}
                  />
                ) : (
                  <div className="p-3 bg-muted/30 rounded-lg border min-h-[100px]">
                    <div className="text-sm">{task.description || "Sem descrição"}</div>
                  </div>
                )}
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="space-y-2">
                  <Label className="text-sm font-medium">Prioridade</Label>
                  {isEditing ? (
                    <Select value={getPriorityLabel(formData.priority)} onValueChange={handlePriorityChange}>
                      <SelectTrigger className="w-full">
                        <SelectValue />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="Alta">Alta</SelectItem>
                        <SelectItem value="Média">Média</SelectItem>
                        <SelectItem value="Baixa">Baixa</SelectItem>
                      </SelectContent>
                    </Select>
                  ) : (
                    <div>
                      <Badge className={`text-xs ${getPriorityColor(formData.priority)}`}>
                        {getPriorityLabel(formData.priority)}
                      </Badge>
                    </div>
                  )}
                </div>

                <div className="space-y-2">
                  <Label className="text-sm font-medium">Atribuição</Label>
                  {isEditing ? (
                    <Select value={formData.assigneeId || "unassigned"} onValueChange={(value) => {
                      if (value === "unassigned") {
                        setFormData(prev => ({ 
                          ...prev, 
                          assigneeId: "",
                          assigneeName: ""
                        }))
                      } else {
                        const selectedUser = users.find(u => u.id === value)
                        setFormData(prev => ({ 
                          ...prev, 
                          assigneeId: value,
                          assigneeName: selectedUser?.name || ""
                        }))
                      }
                    }}>
                      <SelectTrigger className="w-full">
                        <SelectValue placeholder="Selecionar usuário" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="unassigned">
                          <span>Não atribuído</span>
                        </SelectItem>
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
                            <SelectItem key={user.id} value={user.id}>
                              <div className="flex items-center gap-3">
                                <Avatar className="h-6 w-6">
                                  <AvatarImage src={getUIAvatarUrl(user.name)} alt="" />
                                  <AvatarFallback className="text-xs">
                                    {getUserInitials(user.name)}
                                  </AvatarFallback>
                                </Avatar>
                                <span>{user.name}</span>
                              </div>
                            </SelectItem>
                          ))
                        )}
                      </SelectContent>
                    </Select>
                  ) : (
                    <div className="flex items-center gap-2">
                      <Avatar className="h-6 w-6">
                        <AvatarImage src={getUIAvatarUrl(task.userName || "")} alt="" />
                        <AvatarFallback className={`${getAvatarColor(initials)} text-xs`}>
                          {initials}
                        </AvatarFallback>
                      </Avatar>
                      <span className="text-sm">
                        {task.userName || "Não atribuído"}
                      </span>
                    </div>
                  )}
                </div>

                <div className="space-y-2">
                  <Label className="text-sm font-medium">Data</Label>
                  <div className="flex justify-between items-center">
                    <div className="text-sm">
                      {task.createdAt ? new Date(task.createdAt).toLocaleDateString('pt-BR') : "Não definida"}
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div className="flex justify-end gap-2 pt-4">
              {isEditing ? (
                <>
                  <Button variant="destructive" onClick={handleDelete} disabled={isLoading} size="sm" className="h-8 min-w-[100px]">
                    {isLoading ? "Excluindo..." : "Excluir"}
                  </Button>
                  <Button onClick={handleSave} disabled={isLoading} size="sm" className="h-8 min-w-[100px]">
                    {isLoading ? "Salvando..." : "Salvar"}
                  </Button>
                </>
              ) : (
                <Button onClick={() => setIsEditing(true)} size="sm" className="h-8 min-w-[100px]">
                  Editar
                </Button>
              )}
            </div>
          </TabsContent>

          <TabsContent value="comments">
            <CommentsList taskId={task.id} onCommentAdded={() => onCommentAdded?.(task.id)} />
          </TabsContent>

          <TabsContent value="audit">
            <AuditLogsList taskId={task.id} />
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  )
}
