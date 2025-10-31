# Project Manager

A full-stack project management application built with .NET backend and React frontend.

## Features

- Create and manage projects
- Task management
- User authentication
- Real-time updates
- Responsive design

## Tech Stack

- **Frontend**: React.js, TypeScript, Redux, Material-UI
- **Backend**: .NET Core Web API, Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JWT

## Getting Started

### Prerequisites

- .NET 7.0 SDK or later
- Node.js 16.x or later
- SQL Server 2019 or later
- npm or yarn

### Installation

1. Clone the repository
   ```bash
   git clone <repository-url>
   cd ProjectManager
   ```

2. Backend Setup
   ```bash
   cd backend
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

3. Frontend Setup
   ```bash
   cd ../frontend
   npm install
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

## Environment Variables

Create a `.env` file in the frontend directory with the following variables:

```
REACT_APP_API_URL=http://localhost:5000/api
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
