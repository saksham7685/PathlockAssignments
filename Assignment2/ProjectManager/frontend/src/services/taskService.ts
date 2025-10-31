import { apiClient } from '../utils/apiClient';

export interface CreateTaskDto {
  title: string;
  dueDate?: string;
}

export interface UpdateTaskDto {
  title?: string;
  dueDate?: string | null;
  isCompleted?: boolean;
}

export const taskService = {
  createTask: async (projectId: number, data: CreateTaskDto) => {
    return apiClient.post(`/projects/${projectId}/tasks`, data);
  },

  updateTask: async (taskId: number, data: UpdateTaskDto) => {
    return apiClient.put(`/tasks/${taskId}`, data);
  },

  deleteTask: async (projectId: number, taskId: number) => {
    return apiClient.delete(`/projects/${projectId}/tasks/${taskId}`);
  },

  toggleTaskCompletion: async (projectId: number, taskId: number) => {
    return apiClient.post(`/projects/${projectId}/tasks/${taskId}/toggle`, {});
  },
};
