"use client"

import React from "react"
import {
  DndContext,
  DragOverlay,
  PointerSensor,
  TouchSensor,
  useSensor,
  useSensors,
  useDroppable,
  closestCenter
} from "@dnd-kit/core"
import type { DragEndEvent, DragStartEvent } from "@dnd-kit/core"
import { useDraggable } from "@dnd-kit/core"
import { CSS } from "@dnd-kit/utilities"
import { Card } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Clock, MessageCircle, Pencil, Play, CheckCircle, Archive } from "lucide-react"
import { AvatarGroup } from "@/components/ui/avatar-group"
import type { TaskStatus, TaskDto } from "@/api/types"
import { useKanban } from "@/features/kanban/hooks/useKanban"
import { TaskEditModal } from "@/features/kanban/modals/TaskEditModal"
import { useDiscussionUsers } from "@/hooks/useDiscussionUsers"
import { getUserInitials, getUIAvatarUrl } from "@/utils/avatars"

const columns = [
  { id: "todo", title: "Pendências", status: 'Todo' as TaskStatus },
  { id: "progress", title: "Em andamento", status: 'InProgress' as TaskStatus },
  { id: "done", title: "Finalizados", status: 'Done' as TaskStatus },
  { id: "archived", title: "Arquivo", status: 'Archived' as TaskStatus }
]

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

const getEmptyStateForColumn = (status: TaskStatus) => {
  switch (status) {
    case 'Todo':
      return {
        title: "Nenhuma tarefa pendente",
        description: "Arraste tarefas aqui ou crie uma nova",
        icon: Clock
      }
    case 'InProgress':
      return {
        title: "Nenhuma tarefa em andamento", 
        description: "Arraste tarefas para iniciar o trabalho",
        icon: Play
      }
    case 'Done':
      return {
        title: "Nenhuma tarefa finalizada",
        description: "Tarefas concluídas aparecerão aqui",
        icon: CheckCircle
      }
    case 'Archived':
      return {
        title: "Nenhuma tarefa arquivada",
        description: "Tarefas antigas aparecerão aqui quando arquivadas",
        icon: Archive
      }
    default:
      return {
        title: "Nenhuma tarefa",
        description: "Arraste tarefas para esta coluna",
        icon: Clock
      }
  }
}

const truncateText = (text: string, maxLength: number): string => {
  if (text.length <= maxLength) return text
  return text.substring(0, maxLength) + "..."
}


function TaskCardContent({ 
  task, 
  onEditClick, 
  onCommentsClick,
  isLoading 
}: { 
  task: TaskDto
  onEditClick?: (task: TaskDto) => void
  onCommentsClick?: (task: TaskDto) => void
  isLoading?: boolean
}) {
  const initials = getUserInitials(task.userName || "")
  const priorityLabel = getPriorityLabel(task.priorityScore)
  const { users: discussionUsers } = useDiscussionUsers(task.id)

  return (
    <div className="space-y-3 relative">
      <button 
        className={`absolute top-0 right-0 p-1 hover:bg-muted rounded-sm transition-colors hover:scale-110 ${isLoading ? 'opacity-50 cursor-not-allowed' : ''}`}
        title="Editar tarefa"
        disabled={isLoading}
        onClick={(e) => {
          e.stopPropagation()
          onEditClick?.(task)
        }}
      >
        <Pencil className={`h-3 w-3 text-muted-foreground hover:text-foreground transition-colors ${isLoading ? 'animate-pulse' : ''}`} />
      </button>
      
      <h3 className="text-sm font-medium leading-tight line-clamp-2 pr-6">
        {truncateText(task.title, 50)}
      </h3>
      
      {task.description && (
        <p className="text-xs text-muted-foreground line-clamp-1">
          {truncateText(task.description, 80)}
        </p>
      )}
      
      <div className="flex items-center justify-between">
        <Badge 
          className={`text-xs ${getPriorityColor(task.priorityScore)}`}
        >
          {priorityLabel}
        </Badge>
        
        <div className="flex items-center gap-1 text-xs text-muted-foreground">
          <Clock className="h-3 w-3" />
          <span>
            {new Intl.DateTimeFormat('pt-BR', { 
              day: '2-digit', 
              month: 'short' 
            }).format(new Date(task.createdAt))}
          </span>
        </div>
      </div>
      
      <div className="flex items-center gap-2 pt-2 border-t">
        <Avatar className="h-6 w-6">
          <AvatarImage src={getUIAvatarUrl(task.userName, 64)} alt={task.userName || "Não atribuído"} />
          <AvatarFallback className="text-xs">
            {initials}
          </AvatarFallback>
        </Avatar>
        <span className="text-xs text-muted-foreground truncate">
          {task.userName || "Não atribuído"}
        </span>
        
        <div className="flex items-center gap-1 ml-auto">
          {discussionUsers.length > 0 && (
            <AvatarGroup 
              users={discussionUsers} 
              maxVisible={3} 
              size="md" 
              className="mr-1"
            />
          )}
          <button 
            className={`p-1 hover:bg-muted rounded-sm transition-colors hover:scale-110 ${isLoading ? 'opacity-50 cursor-not-allowed' : ''}`}
            title="Ver comentários"
            disabled={isLoading}
            onClick={(e) => {
              e.stopPropagation()
              onCommentsClick?.(task)
            }}
          >
            <MessageCircle className={`h-3 w-3 text-muted-foreground hover:text-foreground transition-colors ${isLoading ? 'animate-pulse' : ''}`} />
          </button>
        </div>
      </div>
    </div>
  )
}

function TaskCard({ 
  task, 
  onClick, 
  onEditClick, 
  onCommentsClick,
  isLoading 
}: { 
  task: TaskDto
  onClick?: (task: TaskDto) => void
  onEditClick?: (task: TaskDto) => void
  onCommentsClick?: (task: TaskDto) => void
  isLoading?: boolean
}) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    isDragging,
  } = useDraggable({ id: task.id })

  const style = {
    transform: CSS.Transform.toString(transform),
  }

  const handleCardClick = (e: React.MouseEvent) => {
    if (!isDragging && onClick && !(e.target as HTMLElement).closest('button')) {
      onClick(task)
    }
  }

  return (
    <Card 
      ref={setNodeRef}
      style={style}
      {...attributes}
      {...listeners}
      onClick={handleCardClick}
      className={`cursor-grab active:cursor-grabbing hover:shadow-md transition-shadow p-3 touch-none relative z-10 w-full min-h-[140px] max-h-[200px] ${
        isDragging ? 'opacity-0' : ''
      }`}
    >
      <TaskCardContent 
        task={task} 
        onEditClick={onEditClick}
        onCommentsClick={onCommentsClick}
        isLoading={isLoading}
      />
    </Card>
  )
}

function DraggingTaskCard({ task }: { task: TaskDto }) {
  const [pulse, setPulse] = React.useState(1)
  
  React.useEffect(() => {
    const interval = setInterval(() => {
      setPulse(prev => prev === 1 ? 1.05 : 1)
    }, 800)
    
    return () => clearInterval(interval)
  }, [])

  return (
    <div className="relative">
      <div 
        className="absolute inset-0 bg-black/8 rounded-lg blur-sm transform translate-y-2"
        style={{
          transform: `translateY(4px) scale(${pulse * 1.05})`,
          transition: 'all 0.8s ease-in-out'
        }}
      />
      
      <Card 
        className="cursor-grabbing shadow-lg p-3 relative z-10 border border-primary/10"
        style={{ 
          transform: `scale(${pulse}) translateY(-2px)`,
          transition: 'all 0.8s ease-in-out',
          boxShadow: '0 8px 20px rgba(0,0,0,0.08), 0 0 10px rgba(59,130,246,0.05)'
        }}
      >
        <TaskCardContent task={task} />
      </Card>
    </div>
  )
}

function Column({ 
  column, 
  tasks, 
  onTaskClick, 
  onEditClick, 
  onCommentsClick,
  isLoading 
}: { 
  column: typeof columns[0]
  tasks: TaskDto[]
  onTaskClick?: (task: TaskDto) => void
  onEditClick?: (task: TaskDto) => void
  onCommentsClick?: (task: TaskDto) => void
  isLoading?: boolean
}) {
  const { setNodeRef, isOver } = useDroppable({ id: column.id })

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h3 className="font-semibold text-sm">{column.title}</h3>
        <Badge variant="secondary" className="text-xs">
          {tasks.length}
        </Badge>
      </div>
      
      <div className={`rounded-lg bg-muted/50 min-h-[120px] transition-colors relative ${
          isOver ? 'bg-muted/80 ring-2 ring-primary/50' : ''
        }`}>
        <div 
          ref={setNodeRef}
          className="absolute inset-0 z-0"
        />
        <div className="relative z-10 p-3 space-y-3">
          {tasks.map((task) => (
            <TaskCard 
              key={task.id} 
              task={task} 
              onClick={onTaskClick}
              onEditClick={onEditClick}
              onCommentsClick={onCommentsClick}
              isLoading={isLoading}
            />
          ))}
          {tasks.length === 0 && (
            <div className="flex flex-col items-center justify-center py-6 text-center">
              {(() => {
                const emptyState = getEmptyStateForColumn(column.status);
                const Icon = emptyState.icon;
                return (
                  <>
                    <Icon className="h-6 w-6 text-muted-foreground/30 mb-2" />
                    <p className="text-xs text-muted-foreground/50 font-medium mb-1">
                      {emptyState.title}
                    </p>
                    <p className="text-xs text-muted-foreground/30 max-w-[180px] leading-snug">
                      {emptyState.description}
                    </p>
                  </>
                );
              })()}
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

interface KanbanBoardProps {
  viewFilter?: string
  onTaskMoved?: () => void
}

export function KanbanBoard({ viewFilter = "visao-geral", onTaskMoved }: KanbanBoardProps) {
  const {
    tasks,
    activeId,
    loadTasks,
    moveTask,
    handleDragStart: startDrag,
    handleDragEnd: endDrag
  } = useKanban()

  const [selectedTask, setSelectedTask] = React.useState<TaskDto | null>(null)
  const [isModalOpen, setIsModalOpen] = React.useState(false)
  const [modalTab, setModalTab] = React.useState<'details' | 'comments' | 'audit'>('details')
  const [isModalLoading, setIsModalLoading] = React.useState(false)
  const [modalEditMode, setModalEditMode] = React.useState(false)

  const handleTaskClick = async (task: TaskDto) => {
    setIsModalLoading(true)
    setSelectedTask(task)
    setModalTab('details')
    setModalEditMode(false)
    setIsModalOpen(true)
    setTimeout(() => setIsModalLoading(false), 200)
  }

  const handleEditClick = async (task: TaskDto) => {
    setIsModalLoading(true)
    setSelectedTask(task)
    setModalTab('details')
    setModalEditMode(true)
    setIsModalOpen(true)
    setTimeout(() => setIsModalLoading(false), 200)
  }

  const handleCommentsClick = async (task: TaskDto) => {
    setIsModalLoading(true)
    setSelectedTask(task)
    setModalTab('comments')
    setModalEditMode(false)
    setIsModalOpen(true)
    setTimeout(() => setIsModalLoading(false), 200)
  }

  const handleTaskUpdated = () => {
    loadTasks()
  }

  const handleCommentAdded = (taskId: string) => {
    window.dispatchEvent(new CustomEvent('discussionUsersChanged', { detail: { taskId } }))
  }

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: { distance: 8 },
    }),
    useSensor(TouchSensor, {
      activationConstraint: { delay: 250, tolerance: 8 },
    })
  )

  const handleDragStart = (event: DragStartEvent) => {
    const taskId = event.active.id as string
    startDrag(taskId)
  }

  const handleDragEnd = async (event: DragEndEvent) => {
    const { active, over } = event
    endDrag()

    if (!over) {
      return
    }

    const taskId = active.id as string
    const newColumnId = over.id as string
    const newStatus = columns.find(col => col.id === newColumnId)?.status

    if (!newStatus) {
      return
    }

    await moveTask(taskId, newStatus)
    onTaskMoved?.()
  }

  const getTasksByColumn = (columnId: string) => {
    const column = columns.find(col => col.id === columnId)
    return column ? tasks.filter(task => task.status === column.status) : []
  }

  const getVisibleColumns = () => {
    if (viewFilter === "visao-geral") {
      return columns
    }
    
    const statusMap: Record<string, string> = {
      "pendencias": "Todo",
      "em-andamento": "InProgress", 
      "finalizados": "Done",
      "arquivo": "Archived"
    }
    
    const targetStatus = statusMap[viewFilter]
    if (targetStatus) {
      return columns.filter(col => col.status === targetStatus)
    }
    
    return columns
  }

  const visibleColumns = getVisibleColumns()

  const activeTask = activeId ? tasks.find(task => task.id === activeId) : null


  return (
    <div className="px-2 lg:px-3">
      <DndContext
        sensors={sensors}
        collisionDetection={closestCenter}
        onDragStart={handleDragStart}
        onDragEnd={handleDragEnd}
      >
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-3">
          {visibleColumns.map((column) => (
            <Column 
              key={column.id} 
              column={column} 
              tasks={getTasksByColumn(column.id)}
              onTaskClick={handleTaskClick}
              onEditClick={handleEditClick}
              onCommentsClick={handleCommentsClick}
              isLoading={isModalLoading}
            />
          ))}
        </div>
        
        <DragOverlay>
          {activeTask ? (
            <div className="scale-105">
              <DraggingTaskCard task={activeTask} />
            </div>
          ) : null}
        </DragOverlay>
      </DndContext>

      <TaskEditModal
        task={selectedTask}
        open={isModalOpen}
        onOpenChange={setIsModalOpen}
        onTaskUpdated={handleTaskUpdated}
        onCommentAdded={handleCommentAdded}
        defaultTab={modalTab}
        initialEditMode={modalEditMode}
      />
    </div>
  )
}
