import { useState, useMemo } from "react"
import { IconPlus } from "@tabler/icons-react"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  Tabs,
  TabsList,
  TabsTrigger,
  TabsContent,
} from "@/components/ui/tabs"
import type { User } from "@/types/user"
import { Role, parseRole } from "@/types/role"

interface UserControlButtonsProps {
  onAddUser?: () => void
  users?: User[]
  onViewChange?: (view: string) => void
}

export function UserControlButtons({ 
  onAddUser,
  users = [],
  onViewChange
}: UserControlButtonsProps) {
  const [selectedView, setSelectedView] = useState("visao-geral")

  const counts = useMemo(() => {
    const safeUsers = Array.isArray(users) ? users : [];
    return {
      admins: safeUsers.filter(user => parseRole(user.role) === Role.Admin).length,
      gerentes: safeUsers.filter(user => parseRole(user.role) === Role.Gerente).length,
      qas: safeUsers.filter(user => parseRole(user.role) === Role.QA).length,
      desenvolvedores: safeUsers.filter(user => parseRole(user.role) === Role.Desenvolvedor).length,
    }
  }, [users])

  const handleViewChange = (view: string) => {
    setSelectedView(view)
    onViewChange?.(view)
  }

  return (
    <Tabs
      value={selectedView}
      onValueChange={handleViewChange}
      className="w-full flex-col justify-start gap-6"
    >
      <div className="flex items-center justify-between">
        <Label htmlFor="view-selector" className="sr-only">
          View
        </Label>
        <Select value={selectedView} onValueChange={handleViewChange}>
          <SelectTrigger
            className="flex w-fit @4xl/main:hidden"
            size="sm"
            id="view-selector"
          >
            <SelectValue placeholder="Select a view" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="visao-geral">Visão geral</SelectItem>
            <SelectItem value="admins">Administradores ({counts.admins})</SelectItem>
            <SelectItem value="gerentes">Gerentes ({counts.gerentes})</SelectItem>
            <SelectItem value="qas">QAs ({counts.qas})</SelectItem>
            <SelectItem value="desenvolvedores">Desenvolvedores ({counts.desenvolvedores})</SelectItem>
          </SelectContent>
        </Select>
        <TabsList className="**:data-[slot=badge]:bg-muted-foreground/30 hidden **:data-[slot=badge]:size-5 **:data-[slot=badge]:rounded-full **:data-[slot=badge]:px-1 @4xl/main:flex relative z-10">
          <TabsTrigger value="visao-geral" className="cursor-pointer">Visão geral</TabsTrigger>
          <TabsTrigger value="admins" className="cursor-pointer">
            Administradores <Badge variant="secondary">{counts.admins}</Badge>
          </TabsTrigger>
          <TabsTrigger value="gerentes" className="cursor-pointer">
            Gerentes <Badge variant="secondary">{counts.gerentes}</Badge>
          </TabsTrigger>
          <TabsTrigger value="qas" className="cursor-pointer">
            QAs <Badge variant="secondary">{counts.qas}</Badge>
          </TabsTrigger>
          <TabsTrigger value="desenvolvedores" className="cursor-pointer">
            Desenvolvedores <Badge variant="secondary">{counts.desenvolvedores}</Badge>
          </TabsTrigger>
        </TabsList>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" onClick={onAddUser}>
            <IconPlus />
            <span className="hidden lg:inline">Adicionar usuário</span>
          </Button>
        </div>
      </div>
      
      <TabsContent value="visao-geral" className="hidden" />
      <TabsContent value="admins" className="hidden" />
      <TabsContent value="gerentes" className="hidden" />
      <TabsContent value="qas" className="hidden" />
      <TabsContent value="desenvolvedores" className="hidden" />
    </Tabs>
  )
}
