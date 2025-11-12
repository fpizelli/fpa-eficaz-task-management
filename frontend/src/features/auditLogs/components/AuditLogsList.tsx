import { useState, useEffect } from 'react';
import { History, ChevronLeft, ChevronRight, Clock } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { AuditLogService } from '@/services/auditLogService';
import { getUserInitials, getUIAvatarUrl } from '@/utils/avatars';
import { getRelativeTimeWithTooltip } from '@/utils/timeUtils';
import type { AuditLog } from '@/types/auditLog';
import type { PagedResult, PaginationParams } from '@/types/comment';

interface AuditLogsListProps {
  taskId: string;
  refreshTrigger?: number;
}

export function AuditLogsList({ taskId, refreshTrigger }: AuditLogsListProps) {
  const [auditLogs, setAuditLogs] = useState<PagedResult<AuditLog>>({
    Items: [],
    TotalCount: 0,
    Page: 1,
    PageSize: 10,
    TotalPages: 0,
    HasNextPage: false,
    HasPreviousPage: false
  });
  const [loading, setLoading] = useState(false);
  const [pagination, setPagination] = useState<PaginationParams>({ page: 1, pageSize: 10 });

  const loadAuditLogs = async () => {
    setLoading(true);
    try {
      const result = await AuditLogService.getAuditLogsByTaskPaged(taskId, pagination);
      setAuditLogs(result);
    } catch (error) {
      console.error('Erro ao carregar logs de auditoria:', error);
      
      setAuditLogs({
        Items: [],
        TotalCount: 0,
        Page: 1,
        PageSize: 10,
        TotalPages: 0,
        HasNextPage: false,
        HasPreviousPage: false
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAuditLogs();
  }, [taskId, pagination, refreshTrigger]);

  const handlePageChange = (newPage: number) => {
    setPagination({ ...pagination, page: newPage });
  };


  const getActionBadgeVariant = () => {
    return 'secondary' as const;
  };

  const formatActionText = (action: string) => {
    const actionMap: { [key: string]: string } = {
      'Tarefa criada': 'Tarefa criada',
      'Tarefa atualizada': 'Tarefa atualizada',
      'Tarefa excluída': 'Tarefa excluída',
      'Status da tarefa alterado': 'Status da tarefa alterado',
      'Prioridade da tarefa alterada': 'Prioridade da tarefa alterada',
      'Tarefa atribuída': 'Tarefa atribuída',
      'Tarefa desatribuída': 'Tarefa desatribuída',
      'Comentário adicionado': 'Comentário adicionado',
      'Comentário atualizado': 'Comentário atualizado',
      'Comentário excluído': 'Comentário excluído',
      
      'Task Created': 'Tarefa criada',
      'Task Updated': 'Tarefa atualizada', 
      'Task Deleted': 'Tarefa excluída',
      'Task Status Changed': 'Status da tarefa alterado',
      'Task Assigned': 'Tarefa atribuída',
      'Task Unassigned': 'Tarefa desatribuída',
      'task created': 'Tarefa criada',
      'task updated': 'Tarefa atualizada',
      'task deleted': 'Tarefa excluída',
      'created': 'Tarefa criada',
      'updated': 'Tarefa atualizada',
      'deleted': 'Tarefa excluída',
      
      'Status Changed': 'Status alterado',
      'Task Moved': 'Tarefa movida',
      'Task Started': 'Tarefa iniciada',
      'Task Completed': 'Tarefa concluída',
      'Task Reopened': 'Tarefa reaberta',
      'Task Archived': 'Tarefa arquivada',
      'status changed': 'Status alterado',
      'task started': 'Tarefa iniciada',
      'task completed': 'Tarefa concluída',
      'task reopened': 'Tarefa reaberta',
      'task archived': 'Tarefa arquivada',
      'task moved': 'Tarefa movida',
      
      'status_changed': 'Status alterado',
      'priority_changed': 'Prioridade alterada'
    };
    return actionMap[action.toLowerCase()] || action;
  };

  const translateStatus = (status: string) => {
    const statusMap: { [key: string]: string } = {
      'Todo': 'Pendente',
      'InProgress': 'Em andamento',
      'Done': 'Finalizado',
      'Archived': 'Arquivado'
    };
    return statusMap[status] || status;
  };

  return (
    <div className="space-y-4">
      {/* Lista de logs de auditoria */}
      <div className="space-y-3">
        {loading ? (
          <div className="text-center py-8 text-muted-foreground">
            <History className="h-6 w-6 mx-auto mb-2 animate-pulse" />
            <p className="text-sm">Carregando histórico...</p>
          </div>
        ) : !auditLogs.Items || auditLogs.Items.length === 0 ? (
          <div className="text-center py-12 text-muted-foreground">
            <Clock className="h-8 w-8 mx-auto mb-3 opacity-40" />
            <p className="text-sm font-medium mb-1">Nenhuma alteração registrada</p>
            <p className="text-xs">O histórico de mudanças aparecerá aqui</p>
          </div>
        ) : (
          auditLogs.Items.map((log) => (
            <div key={log.id} className="flex items-start gap-3 py-3 border-b border-border/50 last:border-0">
              <div className="flex-1 min-w-0">
                <div className="flex items-center justify-between mb-1">
                  <div className="flex items-center gap-2">
                    <Avatar className="h-6 w-6 flex-shrink-0">
                      <AvatarImage src={getUIAvatarUrl(log.userName || "Sistema")} alt="" />
                      <AvatarFallback className="text-xs bg-muted">
                        {getUserInitials(log.userName || "Sistema")}
                      </AvatarFallback>
                    </Avatar>
                    <Badge variant={getActionBadgeVariant()} className="text-xs">
                      {formatActionText(log.action)}
                    </Badge>
                  </div>
                  <span className="text-xs text-muted-foreground" title={getRelativeTimeWithTooltip(log.createdAt).absolute}>
                    {getRelativeTimeWithTooltip(log.createdAt).relative}
                  </span>
                </div>
                
                {(log.oldValue || log.newValue) && log.action !== 'Tarefa criada' && (
                  <div className="text-sm space-y-1 mt-2">
                    {log.oldValue && log.newValue ? (
                      <div className="space-y-1">
                        <div className="flex items-center gap-2">
                          <span className="text-xs text-muted-foreground uppercase tracking-wide">De:</span>
                          <span className="text-sm text-muted-foreground line-through">{translateStatus(log.oldValue.replace(/^(VALOR:\s*)?Título:\s*/, ''))}</span>
                        </div>
                        <div className="flex items-center gap-2">
                          <span className="text-xs text-muted-foreground uppercase tracking-wide">Para:</span>
                          <span className="text-sm font-medium text-foreground">{translateStatus(log.newValue.replace(/^(VALOR:\s*)?Título:\s*/, ''))}</span>
                        </div>
                      </div>
                    ) : log.newValue ? (
                      <div className="text-sm font-medium text-foreground">
                        {translateStatus(log.newValue.replace(/^(VALOR:\s*)?Título:\s*/, ''))}
                      </div>
                    ) : null}
                  </div>
                )}
              </div>
            </div>
          ))
        )}
      </div>

      {/* Paginação */}
      {auditLogs.TotalPages > 1 && (
        <div className="flex items-center justify-between py-3 border-t border-border/50">
          <div className="text-xs text-muted-foreground">
            {auditLogs.TotalCount} registro{auditLogs.TotalCount !== 1 ? 's' : ''}
          </div>
          <div className="flex gap-1">
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handlePageChange(auditLogs.Page - 1)}
              disabled={!auditLogs.HasPreviousPage}
              className="h-8 px-2"
            >
              <ChevronLeft className="h-3 w-3" />
            </Button>
            <span className="text-xs text-muted-foreground px-2 py-1">
              {auditLogs.Page}/{auditLogs.TotalPages}
            </span>
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handlePageChange(auditLogs.Page + 1)}
              disabled={!auditLogs.HasNextPage}
              className="h-8 px-2"
            >
              <ChevronRight className="h-3 w-3" />
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
