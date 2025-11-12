"use client"

import * as React from "react"
import { ChevronLeft, ChevronRight } from "lucide-react"

interface Calendar01Props {
  selected?: Date | undefined
  onSelect?: (date: Date | undefined) => void
  defaultMonth?: Date
  className?: string
}

export function Calendar01({ selected, onSelect, defaultMonth, className }: Calendar01Props = {}) {
  const [currentDate, setCurrentDate] = React.useState(defaultMonth || new Date())
  
  const monthNames = [
    "janeiro", "fevereiro", "março", "abril", "maio", "junho",
    "julho", "agosto", "setembro", "outubro", "novembro", "dezembro"
  ]
  
  const weekDays = ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"]
  
  const getDaysInMonth = (date: Date) => {
    return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate()
  }
  
  const getFirstDayOfMonth = (date: Date) => {
    return new Date(date.getFullYear(), date.getMonth(), 1).getDay()
  }
  
  const getPreviousMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 1))
  }
  
  const getNextMonth = () => {
    setCurrentDate(new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 1))
  }
  
  const handleDateClick = (day: number) => {
    const newDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), day)
    onSelect?.(newDate)
  }
  
  const isSelected = (day: number) => {
    if (!selected) return false
    return selected.getDate() === day && 
           selected.getMonth() === currentDate.getMonth() && 
           selected.getFullYear() === currentDate.getFullYear()
  }
  
  const renderCalendarDays = () => {
    const daysInMonth = getDaysInMonth(currentDate)
    const firstDay = getFirstDayOfMonth(currentDate)
    const days = []
    
    const prevMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 0)
    const daysInPrevMonth = prevMonth.getDate()
    
    for (let i = firstDay - 1; i >= 0; i--) {
      const day = daysInPrevMonth - i
      days.push(
        <button
          key={`prev-${day}`}
          onClick={() => {
            const prevDate = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, day)
            onSelect?.(prevDate)
          }}
          className="w-8 h-8 text-xs text-gray-400 hover:bg-gray-50 transition-colors rounded-md"
        >
          {day}
        </button>
      )
    }
    
    for (let day = 1; day <= daysInMonth; day++) {
      const isSelectedDay = isSelected(day)
      days.push(
        <button
          key={day}
          onClick={() => handleDateClick(day)}
          className={`w-8 h-8 text-xs rounded-md hover:bg-gray-100 transition-colors ${
            isSelectedDay 
              ? "bg-black text-white hover:bg-black" 
              : "text-gray-900 hover:bg-gray-100"
          }`}
        >
          {day}
        </button>
      )
    }
    
    const totalCells = Math.ceil((firstDay + daysInMonth) / 7) * 7
    const remainingCells = totalCells - (firstDay + daysInMonth)
    
    for (let day = 1; day <= remainingCells; day++) {
      days.push(
        <button
          key={`next-${day}`}
          onClick={() => {
            const nextDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, day)
            onSelect?.(nextDate)
          }}
          className="w-8 h-8 text-xs text-gray-400 hover:bg-gray-50 transition-colors rounded-md"
        >
          {day}
        </button>
      )
    }
    
    return days
  }
  
  return (
    <div className={`bg-white rounded-lg border shadow-sm p-4 w-fit ${className || ""}`}>
      {/* Header with month/year and navigation */}
      <div className="flex items-center justify-between mb-4">
        <button 
          onClick={getPreviousMonth}
          className="p-1 hover:bg-gray-100 rounded-md"
        >
          <ChevronLeft className="w-4 h-4" />
        </button>
        
        <h2 className="text-sm font-medium">
          {monthNames[currentDate.getMonth()]} de {currentDate.getFullYear()}
        </h2>
        
        <button 
          onClick={getNextMonth}
          className="p-1 hover:bg-gray-100 rounded-md"
        >
          <ChevronRight className="w-4 h-4" />
        </button>
      </div>
      
      {/* Week days header */}
      <div className="grid grid-cols-7 gap-1 mb-2">
        {weekDays.map((day) => (
          <div key={day} className="w-8 h-6 text-xs text-gray-500 text-center font-normal">
            {day}
          </div>
        ))}
      </div>
      
      {/* Calendar days */}
      <div className="grid grid-cols-7 gap-1">
        {renderCalendarDays()}
      </div>
    </div>
  )
}
