"use client"

import * as React from "react"
import {
  BarChart3,
  CheckSquare,
  Search,
  Settings,
  Users,
  Zap,
} from "lucide-react"

import { NavMain } from "@/components/layout/NavMain"
import { NavSecondary } from "@/components/layout/NavSecondary"
import { NavUser } from "@/components/layout/NavUser"
import { useAuth } from "@/hooks/useAuth"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"

const navigationData = {
  navMain: [
    {
      title: "Tarefas",
      url: "/kanban",
      icon: CheckSquare,
    },
    {
      title: "Análises",
      url: "#",
      icon: BarChart3,
    },
    {
      title: "Equipe",
      url: "/equipe",
      icon: Users,
    },
  ],
  navSecondary: [
    {
      title: "Ajustes",
      url: "#",
      icon: Settings,
    },
    {
      title: "Busca",
      url: "#",
      icon: Search,
    },
  ],
  documents: [],
}

export function AppSidebar({ 
  onQuickAction,
  ...props 
}: React.ComponentProps<typeof Sidebar> & {
  onQuickAction?: () => void
}) {
  const { user } = useAuth()

  return (
    <Sidebar collapsible="offcanvas" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton
              asChild
              className="data-[slot=sidebar-menu-button]:!p-1.5"
            >
              <a href="#">
                <Zap className="!size-5" />
                <span className="text-base font-semibold">Eficaz</span>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={navigationData.navMain} onQuickAction={onQuickAction} />
        <NavSecondary items={navigationData.navSecondary} className="mt-auto" />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={user} />
      </SidebarFooter>
    </Sidebar>
  )
}
