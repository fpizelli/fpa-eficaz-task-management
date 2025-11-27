# fpa-eficaz-task-management

### 1. O projeto

Trata-se de uma aplicação para gestão de tarefas em estilo Kanban, com um sistema de priorização baseado na metodologia RICE, além de um sistema de comentários e auditoria, desenvolvida como projeto integrador das disciplinas de Arquitetura de Software e Desenvolvimento Full Stack, Design Pattern e Clean Code e Fábrica de Projetos Ágeis, em parceria com a empresa Eficaz Marketing.

---

### 2. Como rodar

#### Pré-requisitos

- .NET SDK 10
- Node.js
- Git

#### Clone o repositório

```bash
git clone https://github.com/fpizelli/fpa-eficaz-task-management.git
cd fpa-eficaz-task-management
```

#### Inicie o backend

Em um terminal:

```bash
cd backend
dotnet run
```

Isso irá:

- Subir a API REST (ASP.NET Core)
- Criar o banco `app.db` (SQLite)
- Rodar o seeder inicial com usuários, tarefas, comentários e logs

#### Inicie o frontend

Em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

Abra o navegador no endereço exibido pelo Vite (geralmente `http://localhost:5173`).

---

### 3. Tecnologias utilizadas

- **Backend**
  - ASP.NET Core (.NET 10)
  - Entity Framework Core + SQLite

- **Frontend**
  - React 19 + TypeScript
  - Vite
  - Tailwind CSS

- **Arquitetura e boas práticas**
  - Arquitetura em camadas / ports e casos de uso
  - Domain Services, Repositórios, Unit of Work
  - Seed de dados e CLI para manutenção (seed/clear/help)
  - Clean Code e Design Patterns aplicados no domínio e na infraestrutura

---

### 4. A metologia RICE

No contexto deste projeto, a priorização das tarefas é realizada por meio de uma adaptação da metodologia RICE. Em vez das variáveis tradicionais (Reach, Impact, Confidence e Effort), o modelo adotado considera três dimensões principais: impacto, urgência e esforço, todas avaliadas em uma escala de 1 a 10. A pontuação de prioridade de cada tarefa é calculada automaticamente pela aplicação a partir da fórmula:

Prioridade = (Impacto × Urgência) ÷ Esforço

Dessa forma, tarefas com alto impacto para o negócio, alta urgência e baixo esforço tendem a receber uma pontuação mais elevada, sendo destacadas no quadro Kanban como de prioridade alta. Já tarefas com impacto ou urgência menores, ou que exigem um esforço muito grande, obtêm pontuações menores e são tratadas como prioridade média ou baixa. Esse cálculo é persistido no backend e utilizado pelo frontend para exibir rótulos e cores de prioridade, conferindo um critério objetivo e consistente para apoiar a tomada de decisão sobre o que deve ser feito primeiro pela equipe.


### 5. Autores

- Anthony Gabriel Piovan dos Santos
- Luan Moreira Alves
- Luís Felipe Pizelli Marques
- Leonardo Nunes Navas 
- Leonardo Ribeiro de Assis
- Wendell Pereira Ribeiro

---