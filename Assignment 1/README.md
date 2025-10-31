# Task Manager Application

A full-stack task management application built with React and Node.js.

## Features

- Create, read, update, and delete tasks
- User authentication and authorization
- Responsive design for all devices
- Real-time updates

## Tech Stack

- **Frontend**: React, Vite, Tailwind CSS
- **Backend**: Node.js, Express
- **Database**: MongoDB
- **Authentication**: JWT

## Getting Started

### Prerequisites

- Node.js (v16 or later)
- npm or yarn
- MongoDB Atlas account or local MongoDB instance

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd Assignment-1
   ```

2. Install dependencies for both frontend and backend:
   ```bash
   # Install backend dependencies
   cd backend
   npm install
   
   # Install frontend dependencies
   cd ../frontend
   npm install
   ```

3. Set up environment variables:
   - Create a `.env` file in the backend directory
   - Add your environment variables (see .env.example)

4. Start the development servers:
   ```bash
   # Start backend server
   cd backend
   npm run dev
   
   # In a new terminal, start frontend server
   cd frontend
   npm run dev
   ```

## Project Structure

```
Assignment-1/
├── backend/           # Backend server code
├── frontend/          # Frontend React application
├── .gitignore         # Git ignore file
└── README.md          # This file
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
