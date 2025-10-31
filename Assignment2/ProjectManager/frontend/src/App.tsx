import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { LoginForm } from './components/auth/LoginForm';
import './App.css';

// Protected Route Component
const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />;
};

// Main App Component
function App() {
  return (
    <AuthProvider>
      <Router>
        <div className="min-h-screen bg-gray-100">
          <header className="bg-blue-600 text-white p-4 shadow-md">
            <div className="container mx-auto flex justify-between items-center">
              <h1 className="text-2xl font-bold">Project Manager</h1>
              <nav>
                <ul className="flex space-x-4">
                  <li><a href="/" className="hover:underline">Home</a></li>
                  <li><a href="/projects" className="hover:underline">Projects</a></li>
                  <li><a href="/login" className="hover:underline">Login</a></li>
                </ul>
              </nav>
            </div>
          </header>

          <main className="container mx-auto p-4">
            <Routes>
              <Route path="/" element={
                <div className="text-center py-10">
                  <h2 className="text-3xl font-bold mb-4">Welcome to Project Manager</h2>
                  <p className="text-lg text-gray-600">Manage your projects efficiently and effectively</p>
                </div>
              } />
              <Route path="/login" element={<LoginForm />} />
              <Route 
                path="/projects" 
                element={
                  <ProtectedRoute>
                    <div className="bg-white p-6 rounded-lg shadow">
                      <h2 className="text-2xl font-semibold mb-4">Your Projects</h2>
                      <p>Your projects will appear here...</p>
                    </div>
                  </ProtectedRoute>
                } 
              />
            </Routes>
          </main>

          <footer className="bg-gray-800 text-white p-4 mt-8">
            <div className="container mx-auto text-center">
              <p>© {new Date().getFullYear()} Project Manager. All rights reserved.</p>
            </div>
          </footer>
        </div>
      </Router>
    </AuthProvider>
  );
}

export default App;
