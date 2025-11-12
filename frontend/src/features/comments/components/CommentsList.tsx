import { useState, useEffect } from 'react';
import { MessageCircle, ChevronLeft, ChevronRight } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Textarea } from '@/components/ui/textarea';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';
import { CommentService } from '@/services/commentService';
import { getUserInitials, getUIAvatarUrl } from '@/utils/avatars';
import { getRelativeTimeWithTooltip } from '@/utils/timeUtils';
import { useAuth } from '@/hooks/useAuth';
import type { Comment, CreateCommentDto, PagedResult, PaginationParams } from '@/types/comment';

interface CommentsListProps {
  taskId: string;
  onCommentAdded?: () => void;
  currentUserId?: string;
  currentUserName?: string;
}

export function CommentsList({ taskId, onCommentAdded }: CommentsListProps) {
  const { user } = useAuth();
  const [comments, setComments] = useState<PagedResult<Comment>>({
    Items: [],
    TotalCount: 0,
    Page: 1,
    PageSize: 5,
    TotalPages: 0,
    HasNextPage: false,
    HasPreviousPage: false
  });
  const [loading, setLoading] = useState(false);
  const [newComment, setNewComment] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [pagination, setPagination] = useState<PaginationParams>({ page: 1, pageSize: 5 });

  const loadComments = async () => {
    setLoading(true);
    try {
      const result = await CommentService.getCommentsByTaskPaged(taskId, pagination);
      setComments(result);
    } catch (error) {
      console.error('Erro ao carregar comentários:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadComments();
  }, [taskId, pagination]);

  const handleSubmitComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newComment.trim() || submitting) return;

    if (!user?.id) {
      console.error('Usuário não autenticado');
      alert('Você precisa estar logado para comentar');
      return;
    }

    setSubmitting(true);
    try {
      const createDto: CreateCommentDto = {
        TaskId: taskId,
        UserId: user.id,
        Content: newComment.trim()
      };

      await CommentService.createComment(createDto);
      
      setNewComment('');
      setPagination({ ...pagination, page: 1 });
      await loadComments();
      
      onCommentAdded?.();
    } catch (error) {
      console.error('Erro ao criar comentário:', error);
      alert('Erro ao enviar comentário. Verifique sua conexão e tente novamente.');
    } finally {
      setSubmitting(false);
    }
  };

  const handlePageChange = (newPage: number) => {
    setPagination({ ...pagination, page: newPage });
  };


  return (
    <div className="space-y-4">
      <div className="space-y-3">
        {loading ? (
          <div className="text-center py-8 text-muted-foreground">
            <MessageCircle className="h-6 w-6 mx-auto mb-2 animate-pulse" />
            <p className="text-sm">Carregando comentários...</p>
          </div>
        ) : comments.Items.length === 0 ? (
          <div className="text-center py-12 text-muted-foreground">
            <MessageCircle className="h-8 w-8 mx-auto mb-3 opacity-40" />
            <p className="text-sm font-medium mb-1">Nenhum comentário ainda</p>
            <p className="text-xs">Seja o primeiro a comentar nesta tarefa!</p>
          </div>
        ) : (
          comments.Items.map((comment) => (
            <div key={comment.Id} className="flex items-start gap-3 py-3 border-b border-border/50 last:border-0">
              <Avatar className="h-8 w-8 flex-shrink-0">
                <AvatarImage src={getUIAvatarUrl(comment.UserName)} alt="" />
                <AvatarFallback className="text-xs bg-muted">
                  {getUserInitials(comment.UserName)}
                </AvatarFallback>
              </Avatar>
              <div className="flex-1 min-w-0">
                <div className="flex items-center justify-between mb-1">
                  <span className="font-medium text-sm text-foreground">{comment.UserName}</span>
                  <span className="text-xs text-muted-foreground" title={getRelativeTimeWithTooltip(comment.CreatedAt).absolute}>
                    {getRelativeTimeWithTooltip(comment.CreatedAt).relative}
                  </span>
                </div>
                <p className="text-sm text-foreground whitespace-pre-wrap leading-relaxed">
                  {comment.Content}
                </p>
              </div>
            </div>
          ))
        )}
      </div>

      {comments.TotalPages > 1 && (
        <div className="flex items-center justify-between py-3 border-t border-border/50">
          <div className="text-xs text-muted-foreground">
            {comments.TotalCount} comentário{comments.TotalCount !== 1 ? 's' : ''}
          </div>
          <div className="flex gap-1">
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handlePageChange(comments.Page - 1)}
              disabled={!comments.HasPreviousPage}
              className="h-8 px-2"
            >
              <ChevronLeft className="h-3 w-3" />
            </Button>
            <span className="text-xs text-muted-foreground px-2 py-1">
              {comments.Page}/{comments.TotalPages}
            </span>
            <Button
              variant="ghost"
              size="sm"
              onClick={() => handlePageChange(comments.Page + 1)}
              disabled={!comments.HasNextPage}
              className="h-8 px-2"
            >
              <ChevronRight className="h-3 w-3" />
            </Button>
          </div>
        </div>
      )}

      <div className="border-t border-border/50 pt-4">
        <form onSubmit={handleSubmitComment} className="space-y-3">
          <div className="flex gap-3">
            <Avatar className="h-8 w-8 flex-shrink-0">
              <AvatarImage src={getUIAvatarUrl(user?.name || "Usuário")} alt="" />
              <AvatarFallback className="text-xs bg-primary text-primary-foreground">
                {getUserInitials(user?.name || "Usuário")}
              </AvatarFallback>
            </Avatar>
            <div className="flex-1">
              <Textarea
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                placeholder="Adicione um comentário..."
                rows={2}
                maxLength={1000}
                className="resize-none text-sm border border-border bg-muted/30 focus-visible:ring-2 focus-visible:ring-ring rounded-md"
                style={{ 
                  wordWrap: 'break-word', 
                  overflowWrap: 'break-word', 
                  whiteSpace: 'pre-wrap',
                  wordBreak: 'break-word'
                }}
              />
            </div>
          </div>
          <div className="flex justify-end items-center pl-11">
            <Button 
              type="submit" 
              disabled={!newComment.trim() || submitting}
              size="sm"
              className="h-8"
            >
              {submitting ? "Enviando..." : "Comentar"}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
