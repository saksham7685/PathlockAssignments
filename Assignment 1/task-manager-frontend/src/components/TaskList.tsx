import React, { useState, useEffect } from 'react';
import type { Task, TaskFilter } from '../types/task';
import { getTasks, updateTask, deleteTask } from '../services/taskService';

interface TaskListProps {
  filter: TaskFilter;
  onTaskUpdated: () => void;
}

const TaskList: React.FC<TaskListProps> = ({ filter, onTaskUpdated }) => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTasks = async () => {
      try {
        setLoading(true);
        const data = await getTasks();
        setTasks(data);
        setError(null);
      } catch (err) {
        setError('Failed to fetch tasks. Please try again.');
        console.error('Error fetching tasks:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchTasks();
  }, [onTaskUpdated]);

  const handleToggleComplete = async (task: Task) => {
    try {
      await updateTask({ ...task, isCompleted: !task.isCompleted });
      onTaskUpdated();
    } catch (err) {
      console.error('Error updating task:', err);
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await deleteTask(id);
      onTaskUpdated();
    } catch (err) {
      console.error('Error deleting task:', err);
    }
  };

  const filteredTasks = tasks.filter((task) => {
    if (filter === 'active') return !task.isCompleted;
    if (filter === 'completed') return task.isCompleted;
    return true; // 'all' filter
  });

  if (loading) return <div>Loading tasks...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <ul className="task-list">
      {filteredTasks.length === 0 ? (
        <li className="no-tasks">No tasks found</li>
      ) : (
        filteredTasks.map((task) => (
          <li key={task.id} className={`task-item ${task.isCompleted ? 'completed' : ''}`}>
            <input
              type="checkbox"
              checked={task.isCompleted}
              onChange={() => handleToggleComplete(task)}
              className="task-checkbox"
            />
            <span className="task-description">{task.description}</span>
            <button
              onClick={() => handleDelete(task.id)}
              className="delete-button"
              aria-label="Delete task"
            >
              ×
            </button>
          </li>
        ))
      )}
    </ul>
  );
};

export default TaskList;
