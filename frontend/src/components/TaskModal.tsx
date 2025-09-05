import { useState, useEffect } from "react";
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    MenuItem,
    Button,
    Stack
} from '@mui/material';
import { Task, TaskPriority, TaskStatus, User } from "../types";


interface Props {
    open: boolean;
    onClose: () => void;
    users: User[];
    task?: Task;
    onSave: (
        data: Pick<Task, 'tittle' | 'description' | 'taskPriority' | 'status' | 'assigneeId'>
    ) => Promise<void>;
    
}

const priorities: TaskPriority[] = ['LOW', 'MEDIUM', 'HIGH'];
const statuses: TaskStatus[] = ['TODO', 'IN_PROGRESS', 'DONE'];

const emptyState = {
    tittle: '',
    description: '',
    taskPriority: 'MEDIUM' as TaskPriority,
    status: 'TODO' as TaskStatus,
    assigneeId: '',
};

function TaskModal({ open, onClose, task, users, onSave }: Props) {
    const [local, setLocal] = useState(emptyState);

    useEffect(() => {
        setLocal(
            task
                ? {
                    tittle: task.tittle,
                    description: task.description || '',
                    taskPriority: task.taskPriority,
                    status: task.status,
                    assigneeId: task.assigneeId || '',
                }
            : emptyState
        );
    }, [task]);

    const handleChange = <K extends keyof typeof local>(field: K, value: typeof local[K]) => {
        setLocal((prev) => ({ ...prev, [field]: value }));
    };

    const handleSave = async () => {
        if (!local.tittle.trim()) return;
        await onSave(local);
        onClose();
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>{task ? 'Edit Task' : 'New Task'}</DialogTitle>
            <DialogContent>
                <Stack spacing={2} sx={{ mt: 1 }}>
                    <TextField
                        label="Title"
                        value={local.tittle}
                        onChange={(e) => handleChange('tittle', e.target.value)}
                        required
                        fullWidth
                    />
                    <TextField
                        label="Description"
                        value={local.description}
                        onChange={(e) => handleChange('description', e.target.value)}
                        multiline
                        rows={3}
                        fullWidth
                    />
                    <TextField 
                        select
                        label="Priority"
                        value={local.taskPriority}
                        onChange={(e) => handleChange('taskPriority', e.target.value as TaskPriority)}
                        fullWidth
                    >
                        {priorities.map((p) => (
                            <MenuItem key={p} value={p}>
                                {p}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        select
                        label="Status"
                        value={local.status}
                        onChange={(e) => handleChange('status', e.target.value as TaskStatus)}
                        fullWidth
                    >
                        {statuses.map((s) => (
                            <MenuItem key={s} value={s}>
                                {s}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        select
                        label="Assignee | User" 
                        value={local.assigneeId}
                        onChange={(e) => handleChange('assigneeId', e.target.value)}
                        fullWidth
                    >
                        <MenuItem value="">Unassigned</MenuItem>
                        {users.map((u) => (
                            <MenuItem key={u.id} value={u.id}>
                                {u.username}
                            </MenuItem>
                        ))}
                    </TextField>
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} color="inherit">
                    Cancel
                </Button>
                <Button
                    onClick={handleSave}
                    variant="contained"
                    disabled={!local.tittle.trim()}
                >
                    {task ? 'Update' : 'Create'}
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default TaskModal;
