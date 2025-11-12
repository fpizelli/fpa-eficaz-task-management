import { RefreshCw } from "lucide-react"
import { cn } from "@/lib/utils"

interface LoadingSpinnerProps {
  message?: string
  size?: "sm" | "md" | "lg"
  className?: string
}

export function LoadingSpinner({ 
  message = "Carregando...", 
  size = "md",
  className 
}: LoadingSpinnerProps) {
  const sizeClasses = {
    sm: "h-4 w-4",
    md: "h-6 w-6", 
    lg: "h-8 w-8"
  }

  return (
    <div className={cn("flex items-center justify-center h-64", className)}>
      <div className="flex items-center gap-2 text-muted-foreground">
        <RefreshCw className={cn("animate-spin", sizeClasses[size])} />
        <span className="text-sm">{message}</span>
      </div>
    </div>
  )
}
