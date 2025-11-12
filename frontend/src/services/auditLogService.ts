import { apiClient } from '../api/apiClient';
import type { AuditLog } from '../types/auditLog';
import type { PagedResult, PaginationParams } from '../types/comment';

export class AuditLogService {
  private static readonly BASE_URL = '/tasks';

  static async getAuditLogsByTask(taskId: string): Promise<AuditLog[]> {
    try {
      const result = await apiClient.get<AuditLog[]>(`${this.BASE_URL}/${taskId}/audit`);
      return result;
    } catch (error) {
      console.error(`[AUDIT] Erro ao buscar logs:`, error);
      return [];
    }
  }

  static async getAuditLogsByTaskPaged(
    taskId: string, 
    pagination: PaginationParams
  ): Promise<PagedResult<AuditLog>> {
    const allLogs = await this.getAuditLogsByTask(taskId);
    const startIndex = (pagination.page - 1) * pagination.pageSize;
    const endIndex = startIndex + pagination.pageSize;
    const items = allLogs.slice(startIndex, endIndex);
    
    return {
      Items: items,
      TotalCount: allLogs.length,
      Page: pagination.page,
      PageSize: pagination.pageSize,
      TotalPages: Math.ceil(allLogs.length / pagination.pageSize),
      HasNextPage: endIndex < allLogs.length,
      HasPreviousPage: pagination.page > 1
    };
  }
}
