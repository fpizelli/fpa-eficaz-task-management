export interface AuditLog {
  id: string;
  taskId: string;
  action: string;
  oldValue?: string;
  newValue?: string;
  userId?: string;
  userName?: string;
  createdAt: string;
}
