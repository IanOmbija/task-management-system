export type Role = "USER" | "ADMIN";
export type TaskStatus = "TODO" | "IN_PROGRESS" | "DONE";
export type TaskPriority = "LOW" | "MEDIUM" | "HIGH";

export interface AuthResponse {
  token: string;
  expiresAt: string;
  role: Role;
  username: string;
  userId: string;
}

//User
export interface User {
  id: string;
  username: string;
  email: string;
  role: Role;
}

// Task
export interface Task {
  id: string;
  tittle: string;
  description?: string | null;
  status: TaskStatus;
  taskPriority: TaskPriority;
  assigneeId?: string | null;
  creatorId: string;
  createdAt: string;
  updatedAt: string;
}
