import { useCallback, useEffect, useMemo, useState } from "react";
import { Task, TaskStatus, TaskPriority } from "../types";
import * as api from "../services/tasks";
import { patch } from "@mui/material";


export function useTasks() {
  const [items, setItems] = useState<Task[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  /** * Load tasks from API */ 
  const load = useCallback(
    async (filters?: { status?: TaskStatus; assignee?: string }) => {
      setLoading(true);
      setError(null);
      try {
        console.log("[useTasks] Loading tasks with filters:", filters);
        const data = await api.fetchTasks(filters);
        console.log("[useTasks] Loading tasks with filters:", data);
        setItems(data);
      } catch (e: any) {
        console.error("[useTasks] Failed to load tasks", e);
        setError(e?.message || "Failed to load tasks");
      } finally {
        setLoading(false);
      }
    },
    []
  );

  /** * Create a task */ 
  const create = useCallback(
    async (payload: {
      tittle: string;
      description?: string;
      status?: TaskStatus;
      priority?: TaskPriority;
      assigneeId?: string | null;
    }) => {
      const tempId = `temp_${Date.now()}`;
      const optimistic: Task = {
        id: tempId,
        tittle: payload.tittle,
        description: payload.description || "",
        status: payload.status || "TODO",
        taskPriority: payload.priority || "MEDIUM",
        assigneeId: payload.assigneeId || null,
        creatorId: "me",
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
      };
      setItems((prev) => [optimistic, ...prev]);
      try {
        console.log("[useTasks] Creating task:", payload);
        const real = await api.createTask(payload);
        setItems((prev) => prev.map((t) => (t.id === tempId ? real : t)));
      } catch (e) {
        console.error("[useTasks] Failed to create task, rolling back:", e);
        setItems((prev) => prev.filter((t) => t.id !== tempId));
        throw e;
      }
    },
    []
  );

  
  /** * Update a task | Rollback incase of an error */ 
  const update =
    useCallback(
      async (
        id: string,
        patch: Partial<
          Pick<
            Task,
            "tittle" | "description" | "taskPriority" | "status" | "assigneeId"
          >
        >
      ) => {
        const snapshot = [...items];
        const idx = snapshot.findIndex((t) => t.id === id);
        if (idx === -1) return;
        const optimistic: Task = {
          ...snapshot[idx],
          ...patch,
          updatedAt: new Date().toISOString(),
        };
        setItems((prev) => prev.map((t) => (t.id === id ? optimistic : t)));
        try {
          console.log("[useTasks] Updating tasks:", id, patch);
          const real = await api.updateTask(id, patch);
          setItems((prev) => prev.map((t) => (t.id === id ? real : t)));
        } catch (e) {
          console.error("[useTasks] Failed to update, rolling back:", e);
          setItems(snapshot);
          throw e;
        }
      },
      [items]
    );
    
  /** * Delete a task | Rolls back on failure */ 
  const remove = useCallback(
    async (id: string) => {
      const snapshot = [...items];
      setItems((prev) => prev.filter((t) => t.id !== id));
      try {
        console.log("[useTasks] Deleting task:", id);
        await api.deleteTask(id);
      } catch (e) {
        console.error("[useTasks] Failed to delete, rolling back:", e);
        setItems(snapshot);
        throw e;
      }
    },
    [items]
  );
  return { items, loading, error, load, create, update, remove };
}
