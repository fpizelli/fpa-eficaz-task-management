import { RefreshCw, AlertCircle } from "lucide-react"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"

interface ErrorStateProps {
  message?: string
  onRetry?: () => void
  retryText?: string
  className?: string
  showIcon?: boolean
}

export function ErrorState({ 
  message = "Erro ao carregar dados", 
  onRetry,
  retryText = "Tentar novamente",
  className,
  showIcon = true
}: ErrorStateProps) {
  return (
    <div className={cn("flex items-center justify-center h-64", className)}>
      <div className="text-center space-y-4">
        {showIcon && (
          <AlertCircle className="h-8 w-8 text-muted-foreground mx-auto" />
        )}
        <p className="text-muted-foreground text-sm">{message}</p>
        {onRetry && (
          <Button onClick={onRetry} variant="outline" size="sm">
            <RefreshCw className="h-4 w-4 mr-2" />
            {retryText}
          </Button>
        )}
      </div>
    </div>
  )
}
