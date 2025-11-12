import { api } from '../api/axios';
import type { Comment, CreateCommentDto, PagedResult, PaginationParams } from '../types/comment';

export class CommentService {
  private static readonly BASE_URL = '/comments';

  static async getCommentsByTask(taskId: string): Promise<Comment[]> {
    const response = await api.get<Comment[]>(`${this.BASE_URL}/task/${taskId}`);
    return response.data;
  }

  static async getCommentsByTaskPaged(
    taskId: string, 
    pagination: PaginationParams
  ): Promise<PagedResult<Comment>> {
    const response = await api.get<PagedResult<Comment>>(`${this.BASE_URL}/task/${taskId}/paged?page=${pagination.page}&pageSize=${pagination.pageSize}`);
    return response.data;
  }

  static async createComment(comment: CreateCommentDto): Promise<Comment> {
    const response = await api.post<Comment>(this.BASE_URL, comment);
    
    this.discussionUsersCache.delete(comment.TaskId);
    
    return response.data;
  }

  static async deleteComment(commentId: string): Promise<void> {
    await api.delete(`${this.BASE_URL}/${commentId}`);
  }

  private static discussionUsersCache = new Map<string, { users: Array<{id: string, name: string, email?: string}>, timestamp: number }>();
  private static readonly CACHE_DURATION = 5 * 60 * 1000;

  static async getDiscussionUsers(taskId: string): Promise<Array<{id: string, name: string, email?: string}>> {
    try {
      const cached = this.discussionUsersCache.get(taskId);
      const now = Date.now();
      
      if (cached && (now - cached.timestamp) < this.CACHE_DURATION) {
        return cached.users;
      }

      const result = await this.getCommentsByTaskPaged(taskId, { page: 1, pageSize: 20 });
      
      const uniqueUsers = new Map();
      
      result.Items.forEach(comment => {
        if (comment.UserId && comment.UserName && !uniqueUsers.has(comment.UserId)) {
          uniqueUsers.set(comment.UserId, {
            id: comment.UserId,
            name: comment.UserName,
          });
        }
      });
      
      const users = Array.from(uniqueUsers.values());
      
      this.discussionUsersCache.set(taskId, {
        users,
        timestamp: now
      });
      
      return users;
    } catch (error) {
      console.error('Erro ao buscar usuários da discussão:', error);
      return [];
    }
  }
}
