import { useState, useEffect } from 'react'
import { CommentService } from '@/services/commentService'

interface DiscussionUser {
  id: string
  name: string
  email?: string
}

const discussionUsersCache = new Map<string, { users: DiscussionUser[], timestamp: number }>()
const CACHE_DURATION = 5 * 60 * 1000

export function useDiscussionUsers(taskId: string) {
  const [users, setUsers] = useState<DiscussionUser[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const loadDiscussionUsers = async (forceRefresh = false) => {
    if (!taskId) return

    const cached = discussionUsersCache.get(taskId)
    const now = Date.now()
    
    if (!forceRefresh && cached && (now - cached.timestamp) < CACHE_DURATION) {
      setUsers(cached.users)
      return
    }

    try {
      setLoading(true)
      setError(null)
      const discussionUsers = await CommentService.getDiscussionUsers(taskId)
      setUsers(discussionUsers)
      
      discussionUsersCache.set(taskId, {
        users: discussionUsers,
        timestamp: now
      })
    } catch (err) {
      setError('Erro ao carregar usuários da discussão')
      console.error('Erro ao carregar usuários da discussão:', err)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadDiscussionUsers()
    
    const handleDiscussionUsersChanged = (event: CustomEvent) => {
      if (event.detail.taskId === taskId) {
        loadDiscussionUsers(true)
      }
    }
    
    window.addEventListener('discussionUsersChanged', handleDiscussionUsersChanged as EventListener)
    
    return () => {
      window.removeEventListener('discussionUsersChanged', handleDiscussionUsersChanged as EventListener)
    }
  }, [taskId])

  return {
    users,
    loading,
    error,
    reload: () => loadDiscussionUsers(true)
  }
}
