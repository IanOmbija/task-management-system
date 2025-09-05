import { api } from "./api";
import { Task, TaskStatus, TaskPriority } from "../types";

interface FetchTasksResponse {
  statusCode: number;
  message: string;
  payload: { output: Task[]; rowCount: number };
}


/**
 * Fetch all existing task(s)
 * @param filters 
 * @returns 
 */
export async function fetchTasks(filters?: {
  status?: TaskStatus;
  assignee?: string;
}) {
  try {
    const response = await api.get("/tasks", { params: filters });
    return response.data?.payload?.output ?? [];
  } catch (error) {
    console.error("Failed to fetch tasks", error);
    throw error;
  }
}

/**
 * Create a task function
 * @param task 
 * @returns 
 */
export async function createTask(task: {
  tittle: string;
  description?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  assigneeId?: string | null;
}) {
  try {
    const response = await api.post("/tasks", task);
    return response.data;
  } catch (error) {
    console.error("Failed to create task:", error);
    throw error;
  }
}

/**
 * Update task function
 * @param id 
 * @param updates 
 * @returns 
 */
export async function updateTask(
  id: string,
  updates: Partial<
    Pick<
      Task,
      "tittle" | "description" | "taskPriority" | "status" | "assigneeId"
    >
  >
) {
  try {
    const response = await api.put<Task>(`/tasks/${id}`, updates);
    return response.data;
  } catch (error) {
    console.error(`Failed to update task ${id}:`, error);
    throw error;
  }
}


/**
 * Delete Task
 * @param id 
 * @returns 
 */
export async function deleteTask(
  id: string
) {
  try {
    await api.delete(`/tasks/${id}`);
    return true;
  } catch (error) {
    console.error(`Failed to delete task ${id}`, error);
    throw error;
  }
}
