import axios from 'axios';
import type { Task } from '../types/task';

// Base URL for the API
const API_URL = 'http://localhost:5262/api/tasks';

// Create axios instance with default config
const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true, // Include credentials for CORS
});

// Request interceptor for logging
api.interceptors.request.use(
  (config) => {
    console.log('Request:', {
      url: config.url,
      method: config.method,
      data: config.data,
    });
    return config;
  },
  (error) => {
    console.error('Request Error:', error);
    return Promise.reject(error);
  }
);

// Response interceptor for logging and error handling
api.interceptors.response.use(
  (response) => {
    console.log('Response:', {
      status: response.status,
      data: response.data,
    });
    return response;
  },
  (error) => {
    const errorData = {
      status: error.response?.status,
      message: error.message,
      responseData: error.response?.data,
    };
    console.error('API Error:', errorData);
    return Promise.reject(errorData);
  }
);

/**
 * Fetches all tasks from the server
 * @returns Promise with array of tasks
 */
export const getTasks = async (): Promise<Task[]> => {
  try {
    const response = await api.get<Task[]>('');
    return response.data;
  } catch (error) {
    console.error('Error in getTasks:', error);
    throw new Error('Failed to fetch tasks. Please try again later.');
  }
};

/**
 * Fetches a single task by ID
 * @param id Task ID
 * @returns Promise with the task data
 */
export const getTaskById = async (id: string): Promise<Task> => {
  try {
    const response = await api.get<Task>(`/${id}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching task ${id}:`, error);
    throw new Error('Failed to fetch task. It may have been deleted.');
  }
};

/**
 * Adds a new task
 * @param description Task description
 * @returns Promise with the created task
 */
export const addTask = async (description: string): Promise<Task> => {
  if (!description.trim()) {
    throw new Error('Task description cannot be empty');
  }

  try {
    const response = await api.post<Task>('', { 
      description: description.trim(),
      isCompleted: false 
    });
    return response.data;
  } catch (error: any) {
    console.error('Error in addTask:', error);
    const errorMessage = error.responseData?.title || 'Failed to add task. Please try again.';
    throw new Error(errorMessage);
  }
};

/**
 * Updates an existing task
 * @param task Task object with updated data
 * @returns Promise with the updated task
 */
export const updateTask = async (task: Task): Promise<Task> => {
  if (!task.id) {
    throw new Error('Task ID is required for update');
  }

  try {
    const response = await api.put<Task>(`/${task.id}`, task);
    return response.data;
  } catch (error: any) {
    console.error('Error in updateTask:', error);
    const errorMessage = error.responseData?.title || 'Failed to update task. Please try again.';
    throw new Error(errorMessage);
  }
};

/**
 * Toggles the completion status of a task
 * @param id Task ID
 * @param currentStatus Current completion status
 * @returns Promise with the updated task
 */
export const toggleTaskCompletion = async (id: string, currentStatus: boolean): Promise<Task> => {
  try {
    const task = await getTaskById(id);
    return await updateTask({
      ...task,
      isCompleted: !currentStatus
    });
  } catch (error) {
    console.error('Error in toggleTaskCompletion:', error);
    throw new Error('Failed to toggle task status');
  }
};

/**
 * Deletes a task by ID
 * @param id Task ID to delete
 */
export const deleteTask = async (id: string): Promise<void> => {
  try {
    await api.delete(`/${id}`);
  } catch (error: any) {
    console.error('Error in deleteTask:', error);
    const errorMessage = error.responseData?.title || 'Failed to delete task. Please try again.';
    throw new Error(errorMessage);
  }
};

/**
 * Deletes all completed tasks
 * @returns Promise that resolves when all tasks are deleted
 */
export const clearCompletedTasks = async (): Promise<void> => {
  try {
    const tasks = await getTasks();
    const completedTasks = tasks.filter(task => task.isCompleted);
    await Promise.all(completedTasks.map(task => deleteTask(task.id)));
  } catch (error) {
    console.error('Error in clearCompletedTasks:', error);
    throw new Error('Failed to clear completed tasks');
  }
};
