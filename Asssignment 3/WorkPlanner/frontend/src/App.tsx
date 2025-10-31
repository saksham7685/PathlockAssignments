import { useState } from 'react';
import './App.css';

interface Task {
  title: string;
  estimatedHours: number;
  dueDate: string;
  dependencies: string[];
}

interface ScheduleResponse {
  recommendedOrder?: string[];
  error?: string;
}

// Backend API URL - using the actual port where the backend is running
const API_BASE_URL = 'http://localhost:5246/api/v1/projects';

function App() {
  const [projectId, setProjectId] = useState('default-project');
  const [tasksJson, setTasksJson] = useState<string>(
    JSON.stringify(
      {
        tasks: [
          { title: "Design API", estimatedHours: 5, dueDate: "2025-10-25", dependencies: [] },
          { title: "Implement Backend", estimatedHours: 12, dueDate: "2025-10-28", dependencies: ["Design API"] },
          { title: "Build Frontend", estimatedHours: 10, dueDate: "2025-10-30", dependencies: ["Design API"] },
          { title: "End-to-End Test", estimatedHours: 8, dueDate: "2025-10-31", dependencies: ["Implement Backend", "Build Frontend"] }
        ]
      },
      null,
      2
    )
  );
  const [scheduleResult, setScheduleResult] = useState<ScheduleResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setScheduleResult(null);
    setIsLoading(true);

    try {
      // Parse the JSON input
      let requestData;
      try {
        requestData = JSON.parse(tasksJson);
      } catch (parseError) {
        throw new Error('Invalid JSON format. Please check your input.');
      }
      
      // Make sure the request has the correct structure
      if (!requestData.tasks || !Array.isArray(requestData.tasks)) {
        throw new Error('Invalid tasks format. Please provide an object with a "tasks" array.');
      }
      
      // Validate each task
      requestData.tasks.forEach((task: any, index: number) => {
        if (!task.title || typeof task.title !== 'string') {
          throw new Error(`Task at index ${index} is missing a valid 'title'`);
        }
        if (typeof task.estimatedHours !== 'number' || task.estimatedHours <= 0) {
          throw new Error(`Task "${task.title}" must have a positive 'estimatedHours' number`);
        }
        if (!task.dueDate || isNaN(Date.parse(task.dueDate))) {
          throw new Error(`Task "${task.title}" must have a valid 'dueDate'`);
        }
        if (!Array.isArray(task.dependencies)) {
          throw new Error(`Task "${task.title}" must have a 'dependencies' array`);
        }
      });

      // Note: The controller name is 'scheduler' (case-sensitive)
      const url = new URL(`${API_BASE_URL}/${projectId}/scheduler`);
      console.log('Sending request to:', url.toString());
      console.log('Request data:', requestData);
      
      const response = await fetch(url.toString(), {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
        mode: 'cors',
        body: JSON.stringify(requestData),
      });

      console.log('Response status:', response.status);
      
      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(
          errorData.error || 
          `Server error: ${response.status} ${response.statusText}`
        );
      }

      const data = await response.json();
      console.log('Response data:', data);
      setScheduleResult(data);
    } catch (err) {
      console.error('Error:', err);
      setError(err instanceof Error ? err.message : 'An unknown error occurred');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="app-container">
      <div className="app-content">
        <h1>Work Planner</h1>
        
        <form onSubmit={handleSubmit} className="task-form">
          <div className="form-group">
            <label htmlFor="projectId">
              Project ID
            </label>
            <input
              type="text"
              id="projectId"
              value={projectId}
              onChange={(e) => setProjectId(e.target.value)}
              placeholder="Enter project ID"
            />
          </div>
          
          <div className="form-group">
            <label htmlFor="tasks">
              Tasks (JSON format)
            </label>
            <textarea
              id="tasks"
              value={tasksJson}
              onChange={(e) => setTasksJson(e.target.value)}
              placeholder="Paste your tasks in JSON format"
              spellCheck="false"
              className="task-input"
            />
          </div>
          
          <div className="button-container">
            <button
              type="submit"
              disabled={isLoading}
              className={`submit-button ${isLoading ? 'loading' : ''}`}
            >
              {isLoading ? 'Generating Schedule...' : 'Generate Schedule'}
            </button>
          </div>
        </form>
        
        {isLoading && (
          <div className="loading-container">
            <div className="spinner"></div>
            <p>Processing your tasks...</p>
          </div>
        )}
        
        {error && (
          <div className="error-message">
            <div className="error-icon">
              <svg viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
              </svg>
            </div>
            <div className="error-text">
              <p>{error}</p>
            </div>
          </div>
        )}
        
        {scheduleResult && !isLoading && (
          <div className="result-container">
            <h2>
              {scheduleResult.error ? 'Error' : 'Recommended Task Order'}
            </h2>
            
            {scheduleResult.error ? (
              <div className="warning-message">
                <div className="warning-icon">
                  <svg viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                  </svg>
                </div>
                <div className="warning-text">
                  <p>{scheduleResult.error}</p>
                </div>
              </div>
            ) : (
              <div className="task-list-container">
                <ol className="task-list">
                  {scheduleResult.recommendedOrder?.map((task, index) => (
                    <li key={index}>
                      <span>{task}</span>
                    </li>
                  ))}
                </ol>
              </div>
            )}
          </div>
        )}
      </div>
    </div>
  )
}

export default App
