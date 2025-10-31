import { useState } from 'react';
import './App.css';
import TaskList from './components/TaskList';
import AddTask from './components/AddTask';
import type { TaskFilter } from './types/task';

function App() {
  const [filter, setFilter] = useState<TaskFilter>('all');
  const [refreshKey, setRefreshKey] = useState(0);

  const handleTaskUpdated = () => {
    setRefreshKey(prev => prev + 1);
  };

  return (
    <div className="app">
      <header>
        <h1>Task Manager</h1>
      </header>
      
      <main>
        <div className="task-controls">
          <AddTask onTaskAdded={handleTaskUpdated} />
          
          <div className="filter-buttons">
            <button 
              className={filter === 'all' ? 'active' : ''} 
              onClick={() => setFilter('all')}
            >
              All
            </button>
            <button 
              className={filter === 'active' ? 'active' : ''} 
              onClick={() => setFilter('active')}
            >
              Active
            </button>
            <button 
              className={filter === 'completed' ? 'active' : ''} 
              onClick={() => setFilter('completed')}
            >
              Completed
            </button>
          </div>
        </div>
        
        <TaskList 
          key={refreshKey}
          filter={filter} 
          onTaskUpdated={handleTaskUpdated} 
        />
      </main>
      
      <footer>
        <p>Task Manager App - Built with React, TypeScript, and .NET 8</p>
      </footer>
    </div>
  );
}

export default App;
