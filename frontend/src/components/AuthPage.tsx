import { LoginForm } from "@/components/forms/login-form"
import { usePageTitle } from "@/hooks/usePageTitle"

interface AuthPageProps {
  onAuthSuccess?: () => void;
}

export function AuthPage({ onAuthSuccess }: AuthPageProps) {
  usePageTitle("Entrar | Eficaz")
  
  const handleAuthSuccess = () => {
    onAuthSuccess?.()
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div className="text-center">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            Eficaz
          </h1>
        </div>

        <LoginForm 
          onSuccess={handleAuthSuccess}
        />

      </div>
    </div>
  )
}
