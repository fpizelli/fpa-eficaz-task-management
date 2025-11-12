import { useState } from "react"
import { cn } from "@/utils/cn"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
} from "@/components/ui/card"
import {
  Field,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { useAuth } from "@/hooks/useAuth"

interface LoginFormProps extends React.ComponentProps<"div"> {
  onSuccess?: () => void;
}

export function LoginForm({
  className,
  onSuccess,
  ...props
}: LoginFormProps) {
  const { login, loading } = useAuth()
  const [formData, setFormData] = useState({
    email: "",
    password: ""
  })
  const [error, setError] = useState("")

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError("")

    if (!formData.email) {
      setError("Por favor, preencha o campo de e-mail")
      return
    }
    
    if (!formData.email.includes('@')) {
      setError("Por favor, inclua um '@' no endereço de e-mail")
      return
    }
    
    if (!formData.password) {
      setError("Por favor, preencha o campo de senha")
      return
    }

    try {
      const response = await login(formData)
      
      if (response.success) {
        onSuccess?.()
        window.location.href = '/'
      } else {
        setError(response.message)
      }
    } catch (error) {
      setError("Erro interno do sistema")
    }
  }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData(prev => ({
      ...prev,
      [e.target.name]: e.target.value
    }))
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <FieldGroup>
              
              {error && (
                <div className="p-3 text-sm text-red-600 bg-red-50 border border-red-200 rounded-md">
                  {error}
                </div>
              )}

              <Field>
                <FieldLabel htmlFor="email">E-mail</FieldLabel>
                <Input
                  id="email"
                  name="email"
                  type="text"
                  placeholder="seu@email.com"
                  value={formData.email}
                  onChange={handleChange}
                />
              </Field>
              <Field>
                <FieldLabel htmlFor="password">Senha</FieldLabel>
                <Input 
                  id="password" 
                  name="password"
                  type="password" 
                  placeholder="Digite sua senha"
                  value={formData.password}
                  onChange={handleChange}
                />
              </Field>
              <Field>
                <Button type="submit" disabled={loading}>
                  {loading ? "Entrando..." : "Entrar"}
                </Button>
              </Field>
            </FieldGroup>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}
