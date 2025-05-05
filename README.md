# FullStack To-Do App:

## Features include:
- Add, edit, delete to-dos
- Toggle urgent and completed status
- Case-insensitive search with debounce
- Switch between API provider and local storage

## Stack:
- Frontend: Vue 3 + Vite + Axios
- Backend: ASP.NET Core 8 Web API + Entity Framework Core
- DB: SQLite

## Make sure to download:
- .NET 8 SDK
- Node.js & npm (v20.19.1)
- Git

------------------

### 1. Clone the repo
https://github.com/xAustin-Holmes/To-Do-Workspace.git

### 2. Backend Setup
- cd To-Do-Backend
- dotnet restore
- dotnet ef database update
- dotnet run
- Note: Check localhost url.  Code expects http://localhost:5213 (See api.js)

### 3. Frontend Setup
- cd To-Do-App
- npm install
- npm run dev
- Note: Check localhost url.  Code expects http://localhost:5173
