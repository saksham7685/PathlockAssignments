export interface Task {
  id: string;
  description: string;
  isCompleted: boolean;
}

export type TaskFilter = 'all' | 'active' | 'completed';
