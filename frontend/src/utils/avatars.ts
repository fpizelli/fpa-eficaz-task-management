export const getUserInitials = (name?: string): string => {
  if (!name) return "U"
  return name
    .split(' ')
    .map(word => word.charAt(0))
    .join('')
    .toUpperCase()
    .slice(0, 2)
}

export const getUIAvatarUrl = (name?: string, size: number = 128): string => {
  if (!name) return `https://ui-avatars.com/api/?name=U&background=6b7280&color=fff&size=${size}`
  
  const colors = [
    '6366f1',
    'ef4444',
    'f59e0b',
    '10b981',
    '8b5cf6',
    'f97316',
    '06b6d4',
    'ec4899',
    '84cc16',
    '6b7280',
    '3b82f6',
    '14b8a6',
    'a855f7',
    'f43f5e',
    'eab308',
    '22c55e',
    'fb7185',
    '60a5fa',
    '34d399',
    'fbbf24',
    'c084fc',
    'fb923c',
    '4ade80',
    'f87171',
    '38bdf8',
    'a78bfa',
    'fcd34d',
    '6ee7b7',
    'f9a8d4',
    '93c5fd'
  ]
  
  let hash = 0
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash)
  }
  const colorIndex = Math.abs(hash) % colors.length
  const backgroundColor = colors[colorIndex]
  
  const initials = getUserInitials(name)
  return `https://ui-avatars.com/api/?name=${encodeURIComponent(initials)}&background=${backgroundColor}&color=fff&size=${size}&font-size=0.4`
}
