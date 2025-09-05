import React, { useEffect, useMemo, useState } from "react";
import {
  Box,
  Button,
  Container,
  IconButton,
  MenuItem,
  Paper,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import Grid from "@mui/material/GridLegacy"; 

import AddIcon from "@mui/icons-material/Add";
import DeleteIcon from "@mui/icons-material/Delete";

import TaskCard from "../components/TaskCard";
import TaskModal from "../components/TaskModal";
import { useTasks } from "../hooks/useTasks";
import { fetchUsers } from "../services/users";
import { TaskStatus, User } from "../types";

const columns: TaskStatus[] = ["TODO", "IN_PROGRESS", "DONE"];

const Dashboard: React.FC = () => {
  const { items, loading, error, load, create, update, remove } = useTasks();
  const [open, setOpen] = useState(false);
  const [users, setUsers] = useState<User[]>([]);
  const [search, setSearch] = useState("");
  const [assignee, setAssignee] = useState("");

  useEffect(() => {
    load();
    (async () => {
      const fetched = await fetchUsers();
      setUsers(fetched);
    })();
  }, [load]);

  const filteredTasks = useMemo(() => {
    const q = search.trim().toLowerCase();
    return items.filter(
      (t) =>
        (!q ||
          t.tittle.toLowerCase().includes(q) ||
          (t.description ?? "").toLowerCase().includes(q)) &&
        (!assignee || t.assigneeId === assignee)
    );
  }, [items, search, assignee]);

  return (
    <Container maxWidth="lg">
      {/* Header */}
      <Stack
        direction={{ xs: "column", sm: "row" }}
        spacing={2}
        alignItems={{ xs: "stretch", sm: "center" }}
        justifyContent="space-between"
        sx={{ mb: 2 }}
      >
        <Typography variant="h5" fontWeight={700}>
          Dashboard
        </Typography>

        <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
          <TextField
            size="small"
            placeholder="Search tasks"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />

          <TextField
            size="small"
            select
            label="Assignee"
            value={assignee}
            onChange={(e) => setAssignee(e.target.value)}
            sx={{ minWidth: 160 }}
          >
            <MenuItem value="">All</MenuItem>
            {users.map((u) => (
              <MenuItem key={u.id} value={u.id}>
                {u.username}
              </MenuItem>
            ))}
          </TextField>

          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => setOpen(true)}
          >
            New Task
          </Button>
        </Stack>
      </Stack>

      
      {loading && <Typography sx={{ mb: 2 }}>Loading tasks...</Typography>}
      {error && (
        <Typography color="error" sx={{ mb: 2 }}>
          {error}
        </Typography>
      )}

      {/* Kanban Board */}
      <Grid container spacing={2}>
        {columns.map((col) => {
          const tasks = filteredTasks.filter((t) => t.status === col);

          return (
            <Grid item xs={12} md={4} key={col}>
              <Paper variant="outlined" sx={{ p: 1.5, minHeight: 400 }}>
                <Typography
                  variant="subtitle1"
                  fontWeight={700}
                  sx={{ mb: 1 }}
                >
                  {col.replace("_", " ")}
                </Typography>

                {tasks.length === 0 ? (
                  <Typography variant="body2" color="text.secondary">
                    No Tasks
                  </Typography>
                ) : (
                  tasks.map((t) => (
                    <Stack
                      key={t.id}
                      direction="row"
                      alignItems="center"
                      spacing={1}
                    >
                      <Box flex={1}>
                        <TaskCard task={t} onSave={update} />
                      </Box>
                      <IconButton
                        size="small"
                        aria-label="Delete task"
                        onClick={() => remove(t.id)}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Stack>
                  ))
                )}
              </Paper>
            </Grid>
          );
        })}
      </Grid>

      <TaskModal open={open} onClose={() => setOpen(false)} users={users}
onSave={async (data) => { await create({...data, description: data.description ?? undefined}); }} />
    </Container>
  );
};

export default Dashboard;
