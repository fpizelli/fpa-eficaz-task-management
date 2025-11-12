
export function getRelativeTime(dateString: string): string {
  const now = new Date()
  
  let normalizedDateString = dateString
  if (dateString && !dateString.endsWith('Z') && !dateString.includes('+')) {
    normalizedDateString = dateString + 'Z'
  }
  
  const date = new Date(normalizedDateString)
  const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000)

  if (isNaN(date.getTime())) {
    console.warn('Data inválida recebida:', dateString)
    return 'data inválida'
  }

  if (diffInSeconds < 0) {
    return 'agora'
  }

  if (diffInSeconds < 60) {
    return 'agora'
  }

  const diffInMinutes = Math.floor(diffInSeconds / 60)
  if (diffInMinutes < 60) {
    return `${diffInMinutes}m atrás`
  }

  const diffInHours = Math.floor(diffInMinutes / 60)
  if (diffInHours < 24) {
    return `${diffInHours}h atrás`
  }

  const diffInDays = Math.floor(diffInHours / 24)
  if (diffInDays < 30) {
    return `${diffInDays}d atrás`
  }

  const diffInMonths = Math.floor(diffInDays / 30)
  if (diffInMonths < 12) {
    return `${diffInMonths} mês${diffInMonths > 1 ? 'es' : ''} atrás`
  }

  const diffInYears = Math.floor(diffInMonths / 12)
  return `${diffInYears} ano${diffInYears > 1 ? 's' : ''} atrás`
}

export function getRelativeTimeWithTooltip(dateString: string): { relative: string; absolute: string } {
  const relative = getRelativeTime(dateString)
  
  let normalizedDateString = dateString
  if (dateString && !dateString.endsWith('Z') && !dateString.includes('+')) {
    normalizedDateString = dateString + 'Z'
  }
  
  const absolute = new Date(normalizedDateString).toLocaleString('pt-BR', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
  
  return { relative, absolute }
}
