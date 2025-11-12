import * as React from "react"
import { useLocation } from "react-router-dom"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"
import { Button } from "@/components/ui/button"
import { NewTaskModal } from "@features/kanban"
import { UserCreateModal } from "@/features/users/components/UserCreateModal"

export function SiteHeader() {
  const location = useLocation()
  const [isTaskModalOpen, setIsTaskModalOpen] = React.useState(false)
  const [isUserModalOpen, setIsUserModalOpen] = React.useState(false)

  const getHeaderContent = () => {
    switch (location.pathname) {
      case '/equipe':
        return {
          title: 'Equipe',
          buttonText: null,
          buttonIcon: null,
          onButtonClick: null
        }
      default:
        return {
          title: 'Tarefas',
          buttonText: null,
          buttonIcon: null,
          onButtonClick: null
        }
    }
  }

  const headerContent = getHeaderContent()

  const handleTaskSaved = (taskData: any) => {
    console.log("Tarefa criada:", taskData)
  }

  const handleUserModalClose = () => {
    setIsUserModalOpen(false)
    window.location.reload()
  }

  return (
    <>
      <header className="flex h-(--header-height) shrink-0 items-center gap-2 border-b transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-(--header-height)">
        <div className="flex w-full items-center gap-1 px-4 lg:gap-2 lg:px-6">
          <SidebarTrigger className="-ml-1" />
          <Separator
            orientation="vertical"
            className="mx-2 data-[orientation=vertical]:h-4"
          />
          <h1 className="text-base font-medium">{headerContent.title}</h1>
          <div className="ml-auto flex items-center gap-2">
            {headerContent.buttonText && headerContent.buttonIcon && headerContent.onButtonClick && (
              <Button
                size="sm"
                onClick={headerContent.onButtonClick}
                className="gap-2"
              >
                {React.createElement(headerContent.buttonIcon, { className: "h-4 w-4" })}
                <span>{headerContent.buttonText}</span>
              </Button>
            )}
          </div>
        </div>
      </header>

      <NewTaskModal
        isOpen={isTaskModalOpen}
        onClose={() => setIsTaskModalOpen(false)}
        onSave={handleTaskSaved}
      />

      <UserCreateModal
        isOpen={isUserModalOpen}
        onClose={handleUserModalClose}
      />
    </>
  )
}
