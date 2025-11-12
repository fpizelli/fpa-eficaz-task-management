import * as React from "react"
import { useEffect } from "react"
import { Routes, Route, Navigate } from 'react-router-dom'
import { AppSidebar } from "@/components/layout/AppSidebar"
import { SiteHeader } from "@/components/layout/SiteHeader"
import { Home } from "@pages/Home"
import { TeamView } from "@pages/TeamView"
import { QuickActionModal } from "@/components/modals/QuickActionModal"
import {
  SidebarInset,
  SidebarProvider,
} from "@/components/ui/sidebar"
import { useAuth } from "@/hooks/useAuth"

function LoginRedirect() {
  const { logout } = useAuth()
  
  useEffect(() => {
    logout()
  }, [logout])
  
  return <div>Redirecionando para login...</div>
}

export default function MainPage() {
  const [isQuickActionModalOpen, setIsQuickActionModalOpen] = React.useState(false)

  const handleQuickAction = () => {
    setIsQuickActionModalOpen(true)
  }
  return (
    <SidebarProvider
      style={
        {
          "--sidebar-width": "calc(var(--spacing) * 72)",
          "--header-height": "calc(var(--spacing) * 12)",
        } as React.CSSProperties
      }
    >
      <AppSidebar variant="inset" onQuickAction={handleQuickAction} />
      <SidebarInset>
        <SiteHeader />
        <Routes>
          <Route path="/" element={<Navigate to="/kanban" replace />} />
          <Route path="/kanban" element={<Home />} />
          <Route path="/equipe" element={<TeamView />} />
          <Route path="/login" element={<LoginRedirect />} />
        </Routes>
      </SidebarInset>
      
      <QuickActionModal
        isOpen={isQuickActionModalOpen}
        onClose={() => setIsQuickActionModalOpen(false)}
      />
    </SidebarProvider>
  )
}
