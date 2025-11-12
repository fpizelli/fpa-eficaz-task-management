import { useState } from "react"
import { KanbanBoard, NewTaskModal, useKanban } from "@features/kanban"
import { TaskControlButtons } from "@/features/kanban/components/TaskControlButtons"
import { LoadingSpinner } from "@/components/ui/loading-spinner"
import { ErrorState } from "@/components/ui/error-state"
import { usePageTitle } from "@/hooks/usePageTitle"

export function Home() {
  usePageTitle("Início | Eficaz")
  
  const [isTaskModalOpen, setIsTaskModalOpen] = useState(false)
  const [selectedView, setSelectedView] = useState("visao-geral")
  
  const { tasks, loading, error, loadTasks } = useKanban()

  const handleAddSection = () => {
    setIsTaskModalOpen(true)
  }

  const handleTaskSaved = () => {
    setIsTaskModalOpen(false)
    loadTasks()
  }

  const handleViewChange = (view: string) => {
    setSelectedView(view)
  }

  const handleTaskMoved = () => {
    loadTasks()
  }

  if (loading) {
    return <LoadingSpinner message="Carregando tarefas..." />
  }

  if (error) {
    return <ErrorState message={error} onRetry={loadTasks} />
  }

  return (
    <>
      <div className="@container/main flex flex-1 flex-col gap-2">
        <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6">
          <TaskControlButtons 
            onAddSection={handleAddSection}
            tasks={tasks}
            onViewChange={handleViewChange}
          />
          <div className="px-4 lg:px-6">
            <KanbanBoard 
              viewFilter={selectedView} 
              onTaskMoved={handleTaskMoved}
            />
          </div>
        </div>
      </div>

      <NewTaskModal
        isOpen={isTaskModalOpen}
        onClose={() => setIsTaskModalOpen(false)}
        onSave={handleTaskSaved}
      />
    </>
  )
}
