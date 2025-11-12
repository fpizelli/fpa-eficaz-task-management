
import { useState, useEffect } from 'react'
import type { TaskDto, TaskStatus } from '@/api/types'
import { getTasks } from '@/api/taskApi'
import { moveTask as moveTaskApi } from '@/api/taskApi'

export function useKanban() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [activeId, setActiveId] = useState<string | null>(null)

  const loadTasks = async () => {
    try {
      setLoading(true)
      setError(null)
      
      const taskList = await getTasks()
      setTasks(taskList)
    } catch (err) {
      setError('Erro ao carregar tarefas')
    } finally {
      setLoading(false)
    }
  }

  const moveTask = async (taskId: string, newStatus: TaskStatus) => {
    const task = tasks.find(t => t.id === taskId)
    
    if (!task || task.status === newStatus) {
      return
    }

    try {
      setTasks(prev => prev.map(t => 
        t.id === taskId ? { ...t, status: newStatus } : t
      ))

      const result = await moveTaskApi(taskId, newStatus)
      
      setTasks(prev => prev.map(t => 
        t.id === taskId ? result : t
      ))
    } catch (error) {
      loadTasks()
    }
  }

  const handleDragStart = (taskId: string) => {
    setActiveId(taskId)
  }

  const handleDragEnd = () => {
    setActiveId(null)
  }

  useEffect(() => {
    loadTasks()
    
    ;(window as any).reloadTasks = loadTasks
    
    return () => {
      delete (window as any).reloadTasks
    }
  }, [])

  return {
    tasks,
    loading,
    error,
    activeId,
    loadTasks,
    moveTask,
    handleDragStart,
    handleDragEnd
  }
}
