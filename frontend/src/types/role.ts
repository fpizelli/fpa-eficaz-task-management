export const Role = {
  Desenvolvedor: 1,
  QA: 2,
  Gerente: 3,
  Admin: 4
} as const

export type Role = typeof Role[keyof typeof Role]

export const RoleLabels: Record<Role, string> = {
  [Role.Desenvolvedor]: 'Desenvolvedor',
  [Role.QA]: 'QA',
  [Role.Gerente]: 'Gerente',
  [Role.Admin]: 'Administrador'
}

export const RoleColors: Record<Role, string> = {
  [Role.Desenvolvedor]: 'bg-blue-100 text-blue-800',
  [Role.QA]: 'bg-yellow-100 text-yellow-800', 
  [Role.Gerente]: 'bg-red-100 text-red-800',
  [Role.Admin]: 'bg-purple-100 text-purple-800'
}

export const parseRole = (role: string | number | undefined): Role => {
  if (typeof role === 'number' && role >= 1 && role <= 4) {
    return role as Role
  }
  
  if (typeof role === 'string') {
    switch (role) {
      case 'Desenvolvedor': return Role.Desenvolvedor
      case 'QA': return Role.QA  
      case 'Gerente': return Role.Gerente
      case 'Admin': return Role.Admin
      case '1': return Role.Desenvolvedor
      case '2': return Role.QA
      case '3': return Role.Gerente
      case '4': return Role.Admin
    }
  }
  
  console.warn('Role inválido recebido:', role, 'usando Desenvolvedor como fallback')
  return Role.Desenvolvedor
}
