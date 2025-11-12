import { Plus, Inbox } from "lucide-react"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"

interface EmptyStateProps {
  title?: string
  description?: string
  actionText?: string
  onAction?: () => void
  icon?: React.ComponentType<{ className?: string }>
  className?: string
}

export function EmptyState({ 
  title = "Nenhum item encontrado",
  description = "Não há dados para exibir no momento.",
  actionText,
  onAction,
  icon: Icon = Inbox,
  className
}: EmptyStateProps) {
  return (
    <div className={cn("flex items-center justify-center h-64", className)}>
      <div className="text-center space-y-4 max-w-md">
        <Icon className="h-12 w-12 text-muted-foreground/50 mx-auto" />
        <div className="space-y-2">
          <h3 className="text-lg font-medium text-muted-foreground">{title}</h3>
          <p className="text-sm text-muted-foreground/70">{description}</p>
        </div>
        {actionText && onAction && (
          <Button onClick={onAction} variant="outline" size="sm">
            <Plus className="h-4 w-4 mr-2" />
            {actionText}
          </Button>
        )}
      </div>
    </div>
  )
}
