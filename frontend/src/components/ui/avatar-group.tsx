import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { getUserInitials, getUIAvatarUrl } from "@/utils/avatars"

interface User {
  id: string
  name: string
  email?: string
}

interface AvatarGroupProps {
  users: User[]
  maxVisible?: number
  size?: "sm" | "md" | "lg"
  className?: string
}

const sizeClasses = {
  sm: "h-5 w-5 text-xs",
  md: "h-6 w-6 text-xs", 
  lg: "h-8 w-8 text-sm"
}

// Classes responsivas para diferentes tamanhos de tela
const responsiveClasses = "flex-shrink-0 sm:flex md:flex lg:flex"

const getAvatarColor = (name: string): string => {
  const colors = [
    "bg-purple-500 text-white",
    "bg-orange-500 text-white", 
    "bg-pink-500 text-white",
    "bg-blue-500 text-white",
    "bg-green-500 text-white",
    "bg-red-500 text-white"
  ]
  return colors[name.charCodeAt(0) % colors.length]
}

export function AvatarGroup({ 
  users, 
  maxVisible = 3, 
  size = "sm",
  className = "" 
}: AvatarGroupProps) {
  if (!users || users.length === 0) return null

  const visibleUsers = users.slice(0, maxVisible)
  const remainingCount = users.length - maxVisible

  return (
    <div className={`flex -space-x-2 ${responsiveClasses} ${className}`}>
        {visibleUsers.map((user, index) => {
          const initials = getUserInitials(user.name)
          
          return (
            <Avatar 
              key={user.id}
              className={`${sizeClasses[size]} border-2 border-background relative z-${10 + index} hover:z-50 transition-all hover:scale-110 cursor-pointer`}
              title={user.email ? `${user.name}\n${user.email}` : user.name}
            >
              <AvatarImage 
                src={getUIAvatarUrl(user.name, 32)} 
                alt="" 
              />
              <AvatarFallback className={getAvatarColor(user.name)}>
                {initials}
              </AvatarFallback>
            </Avatar>
          )
        })}
        
        {remainingCount > 0 && (
          <Avatar 
            className={`${sizeClasses[size]} border-2 border-background bg-muted text-muted-foreground relative z-${10 + visibleUsers.length} hover:z-50 transition-all hover:scale-110 cursor-pointer`}
            title={`${remainingCount} usuário${remainingCount > 1 ? 's' : ''} adiciona${remainingCount > 1 ? 'is' : 'l'}: ${users.slice(maxVisible).map(u => u.name).join(', ')}`}
          >
            <AvatarFallback className="bg-muted text-muted-foreground">
              +{remainingCount}
            </AvatarFallback>
          </Avatar>
        )}
      </div>
  )
}
