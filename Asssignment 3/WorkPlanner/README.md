# Work Planner

A task scheduling application that helps you plan and organize your work by automatically determining the optimal order of tasks based on their dependencies and due dates.

## Features

- **Task Management**: Create and manage tasks with titles, estimated hours, and due dates
- **Dependency Tracking**: Define dependencies between tasks to ensure proper execution order
- **Automatic Scheduling**: The system automatically calculates the optimal task order using topological sorting
- **Responsive UI**: Clean and intuitive user interface built with React and TypeScript
- **RESTful API**: Built with .NET 8 Web API for reliable backend operations

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v16 or later)
- [npm](https://www.npmjs.com/) (v8 or later) or [Yarn](https://yarnpkg.com/)

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the backend server:
   ```bash
   dotnet run
   ```

   The backend will be available at `http://localhost:5246`

### Frontend Setup

1. Open a new terminal and navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   # or
   yarn install
   ```

3. Start the development server:
   ```bash
   npm run dev
   # or
   yarn dev
   ```

   The frontend will be available at `http://localhost:5173`

## API Endpoints

### Schedule Tasks

- **URL**: `/api/v1/projects/{projectId}/scheduler`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "tasks": [
      {
        "title": "Design API",
        "estimatedHours": 5,
        "dueDate": "2025-10-25T00:00:00",
        "dependencies": []
      },
      {
        "title": "Implement Backend",
        "estimatedHours": 12,
        "dueDate": "2025-10-28T00:00:00",
        "dependencies": ["Design API"]
      }
    ]
  }
  ```

## Project Structure

```
WorkPlanner/
├── backend/               # .NET 8 Web API
│   ├── Controllers/       # API Controllers
│   ├── Models/            # Data models and DTOs
│   ├── Services/          # Business logic
│   └── Program.cs         # Application entry point
├── frontend/              # React + TypeScript app
│   ├── public/            # Static files
│   ├── src/               # Source code
│   │   ├── components/    # React components
│   │   ├── App.tsx        # Main App component
│   │   └── main.tsx       # Entry point
│   └── package.json       # Frontend dependencies
└── README.md              # This file
```

## Technologies Used

### Backend
- .NET 8.0
- ASP.NET Core Web API
- C# 10

### Frontend
- React 18
- TypeScript
- Vite
- CSS3

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


---

