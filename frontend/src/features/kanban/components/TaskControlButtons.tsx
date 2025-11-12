import { useState } from "react"
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
} from "@/components/ui/tabs"
import type { TaskDto } from "@/api/types"

interface TaskControlButtonsProps {
  onAddSection?: () => void
  tasks?: TaskDto[]
  onViewChange?: (view: string) => void
}

export function TaskControlButtons({ 
  onAddSection,
  tasks = [],
  onViewChange
}: TaskControlButtonsProps) {
  const [selectedView, setSelectedView] = useState("visao-geral")

  const getTaskCounts = () => {
    const counts = {
      pendencias: tasks.filter(task => task.status === 'Todo').length,
      emAndamento: tasks.filter(task => task.status === 'InProgress').length,
      finalizados: tasks.filter(task => task.status === 'Done').length,
      arquivo: tasks.filter(task => task.status === 'Archived').length,
    }
    return counts
  }

  const counts = getTaskCounts()

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
      <div className="flex items-center justify-between px-4 lg:px-6">
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
            <SelectItem value="pendencias">Pendências ({counts.pendencias})</SelectItem>
            <SelectItem value="em-andamento">Em andamento ({counts.emAndamento})</SelectItem>
            <SelectItem value="finalizados">Finalizados ({counts.finalizados})</SelectItem>
            <SelectItem value="arquivo">Arquivo ({counts.arquivo})</SelectItem>
          </SelectContent>
        </Select>
        <TabsList className="**:data-[slot=badge]:bg-muted-foreground/30 hidden **:data-[slot=badge]:size-5 **:data-[slot=badge]:rounded-full **:data-[slot=badge]:px-1 @4xl/main:flex">
          <TabsTrigger value="visao-geral">Visão geral</TabsTrigger>
          <TabsTrigger value="pendencias">
            Pendências <Badge variant="secondary">{counts.pendencias}</Badge>
          </TabsTrigger>
          <TabsTrigger value="em-andamento">
            Em andamento <Badge variant="secondary">{counts.emAndamento}</Badge>
          </TabsTrigger>
          <TabsTrigger value="finalizados">
            Finalizados <Badge variant="secondary">{counts.finalizados}</Badge>
          </TabsTrigger>
          <TabsTrigger value="arquivo">
            Arquivo <Badge variant="secondary">{counts.arquivo}</Badge>
          </TabsTrigger>
        </TabsList>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" onClick={onAddSection}>
            <IconPlus />
            <span className="hidden lg:inline">Adicionar tarefa</span>
          </Button>
        </div>
      </div>
    </Tabs>
  )
}
