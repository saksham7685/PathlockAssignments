import React, { useState } from 'react';
import { addTask } from '../services/taskService';

interface AddTaskProps {
  onTaskAdded: () => void;
}

const AddTask: React.FC<AddTaskProps> = ({ onTaskAdded }) => {
  const [description, setDescription] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!description.trim()) return;

    try {
      setIsSubmitting(true);
      setError(null);
      console.log('Attempting to add task:', description);
      await addTask(description);
      console.log('Task added successfully');
      setDescription('');
      onTaskAdded();
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to add task';
      console.error('Error in handleSubmit:', { error: err, message: errorMessage });
      setError(errorMessage);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="add-task-form">
      <div className="input-group">
        <input
          type="text"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Enter a new task..."
          className="task-input"
          disabled={isSubmitting}
          aria-label="Task description"
        />
        <button 
          type="submit" 
          className="add-button"
          disabled={!description.trim() || isSubmitting}
          aria-busy={isSubmitting}
        >
          {isSubmitting ? 'Adding...' : 'Add Task'}
        </button>
      </div>
      {error && (
        <div className="error-message" role="alert">
          {error}
        </div>
      )}
    </form>
  );
};

export default AddTask;
