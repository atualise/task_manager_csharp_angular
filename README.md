# Sistema de Gerenciamento de Tarefas

Um sistema completo de gerenciamento de tarefas construído com uma arquitetura de microserviços, utilizando .NET 8 para o backend e Angular para o frontend.

## 🚀 Tecnologias

### Backend
- .NET 8
- Entity Framework Core
- SQLite
- JWT Authentication
- CQRS Pattern
- MediatR
- Swagger/OpenAPI

### Frontend
- Angular 17
- Angular Material
- RxJS
- TypeScript

## 📋 Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (versão 18 ou superior)
- [Angular CLI](https://angular.io/cli)
- [Git](https://git-scm.com/)

## 🔧 Instalação

### 1. Clone o repositório
```bash
git clone https://seu-repositorio/task-management.git
cd task-management
```

### 2. Backend - UserService
```bash
cd backend/UserService
dotnet restore
dotnet run
```
O serviço estará disponível em: http://localhost:5022

### 3. Backend - TaskService
```bash
cd backend/TaskService
dotnet restore
dotnet run
```
O serviço estará disponível em: http://localhost:5023

### 4. Frontend
```bash
cd frontend
npm install
ng serve
```
A aplicação estará disponível em: http://localhost:4200

## 🌐 Endpoints da API

### UserService
- POST /user/register - Registra um novo usuário
- POST /user/login - Realiza login e retorna token JWT

### TaskService
- GET /task - Lista todas as tarefas
- POST /task - Cria uma nova tarefa
- PUT /task/{id} - Atualiza uma tarefa
- DELETE /task/{id} - Remove uma tarefa
- GET /task/export - Exporta tarefas para Excel

## 🔐 Autenticação

O sistema utiliza autenticação JWT. Para acessar endpoints protegidos:
1. Faça login para obter o token
2. Inclua o token no header das requisições:

```http
Authorization: Bearer {seu-token}
```

## 📦 Estrutura do Projeto

```
├── backend/
│   ├── UserService/         # Microserviço de autenticação
│   └── TaskService/         # Microserviço de tarefas
├── frontend/
│   └── app/                 # Aplicação Angular
└── README.md
```

## 🛠️ Desenvolvimento

### Configuração do Ambiente de Desenvolvimento

1. Configure o arquivo appsettings.json em cada serviço:
```json
{
  "ConnectionStrings": {
    "Default": "Data Source=app.db"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-aqui"
  }
}
```

2. Configure o ambiente Angular:
```bash
cd frontend
cp environment.example.ts environment.ts
```

### Executando Testes

```bash
# Backend
cd backend/TaskService
dotnet test

# Frontend
cd frontend
ng test
```

## 📄 Documentação da API

- UserService Swagger UI: http://localhost:5022
- TaskService Swagger UI: http://localhost:5023


## ✨ Funcionalidades

- Autenticação completa de usuários
- CRUD de tarefas
- Filtros por status e prioridade
- Exportação para Excel
- Interface responsiva
- Proteção de rotas
- Documentação OpenAPI/Swagger

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

