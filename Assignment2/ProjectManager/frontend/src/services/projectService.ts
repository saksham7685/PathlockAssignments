import { apiClient } from '../utils/apiClient';

export interface ProjectDto {
  id: number;
  name: string;
  description: string | null;
  createdAt: string;
  taskCount: number;
  completedTaskCount: number;
}

export interface ProjectDetailDto extends ProjectDto {
  tasks: TaskDto[];
}

export interface CreateProjectDto {
  name: string;
  description?: string;
}

export interface TaskDto {
  id: number;
  title: string;
  dueDate: string | null;
  isCompleted: boolean;
  projectId: number;
}

export const projectService = {
  getProjects: async (): Promise<ProjectDto[]> => {
    return apiClient.get<ProjectDto[]>('/projects');
  },

  getProject: async (id: number): Promise<ProjectDetailDto> => {
    return apiClient.get<ProjectDetailDto>(`/projects/${id}`);
  },

  createProject: async (data: CreateProjectDto): Promise<ProjectDto> => {
    return apiClient.post<ProjectDto>('/projects', data);
  },

  deleteProject: async (id: number): Promise<void> => {
    return apiClient.delete(`/projects/${id}`);
  },
};
