type LogLevel = 'debug' | 'info' | 'warn' | 'error'

class Logger {
  private isDevelopment = import.meta.env.DEV
  
  private log(level: LogLevel, message: string, data?: any) {
    if (!this.isDevelopment && level === 'debug') return
    
    const emoji = this.getEmoji(level)
    const prefix = `${emoji} [${level.toUpperCase()}]`
    
    if (data) {
      console[level](`${prefix} ${message}`, data)
    } else {
      console[level](`${prefix} ${message}`)
    }
  }
  
  private getEmoji(level: LogLevel): string {
    switch (level) {
      case 'debug': return '🔍'
      case 'info': return 'ℹ️'
      case 'warn': return '⚠️'
      case 'error': return '❌'
      default: return '📝'
    }
  }
  
  debug(message: string, data?: any) {
    this.log('debug', message, data)
  }
  
  info(message: string, data?: any) {
    this.log('info', message, data)
  }
  
  warn(message: string, data?: any) {
    this.log('warn', message, data)
  }
  
  error(message: string, data?: any) {
    this.log('error', message, data)
  }
  
  taskCreation = {
    start: () => this.info('Iniciando criação de tarefa'),
    dataReceived: (data: any) => this.debug('Dados recebidos do modal', data),
    validationFailed: (missing: any) => this.error('Dados obrigatórios faltando', missing),
    dtoCreated: (dto: any) => this.debug('DTO preparado para envio', dto),
    connectionTest: () => this.debug('Testando conexão com backend'),
    connectionSuccess: (status: number) => this.debug(`Backend online (${status})`),
    connectionFailed: (status: number) => this.error(`Backend não responde (${status})`),
    creating: () => this.info('Criando tarefa no backend'),
    success: (task: any) => this.info('Tarefa criada com sucesso', { id: task.id, title: task.title }),
    reloading: () => this.debug('Recarregando tarefas no Kanban'),
    reloadSuccess: () => this.debug('Tarefas recarregadas com sucesso'),
    reloadMissing: () => this.warn('Função reloadTasks não encontrada'),
    completed: () => this.info('Processo de criação concluído'),
    failed: (error: Error) => this.error('Erro ao criar tarefa', { 
      message: error.message, 
      stack: error.stack 
    })
  }
  
  kanban = {
    loadStart: () => this.debug('Carregando tarefas do backend'),
    loadSuccess: (count: number) => this.info(`${count} tarefas carregadas com sucesso`),
    loadError: (error: any) => this.error('Erro ao carregar tarefas', error),
    dragStart: (taskId: string) => this.debug(`Iniciando drag da tarefa ${taskId}`),
    dragEnd: (taskId: string, newStatus: string) => this.debug(`Movendo tarefa ${taskId} para ${newStatus}`),
    statusUpdateSuccess: (taskId: string, status: string) => this.info(`Status atualizado: ${taskId} → ${status}`),
    statusUpdateError: (taskId: string, error: any) => this.error(`Erro ao atualizar status da tarefa ${taskId}`, error),
    reloadExposed: () => this.debug('Função reloadTasks registrada globalmente')
  }
}

export const logger = new Logger()
