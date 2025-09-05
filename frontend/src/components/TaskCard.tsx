import React, { useState } from 'react';
import { Card, CardContent, Typography, IconButton, MenuItem, TextField, Stack } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import SaveIcon from '@mui/icons-material/Save';
import CloseIcon from '@mui/icons-material/Close';
import { Task, TaskPriority, TaskStatus ,User} from '../types';


interface Props {
    task: Task;
    onSave: (
        id: string,
        update: Partial<
        Pick<Task, 'tittle' | 'description' | 'taskPriority' | 'status' | 'assigneeId'>
        >
    ) => Promise<void>
}

const priorities: TaskPriority[] = ['LOW', 'MEDIUM', 'HIGH'];
const statuses: TaskStatus[] = ['TODO', 'IN_PROGRESS', 'DONE'];

function TaskCard({ task, onSave }: Props) {
    const [editing, setEditing] = useState(false);
    const [local, setLocal] = useState({
        tittle: task.tittle,
        description: task.description || '',
        priority: task.taskPriority,
        status: task.status,
    });

    const updatedField = (key: keyof typeof local, value: string) => {
        setLocal((prev) => ({ ...prev, [key]: value }));
    };

    const hasChanges = 
        local.tittle !== task.tittle ||
        local.description !== (task.description || '') ||
        local.priority !== task.taskPriority ||
        local.status !== task.status;

    const handleSave = async () => {
        if (!hasChanges) {
            setEditing(false);
            return;
        }
        await onSave(task.id, local);
        setEditing(false);
    };


    if (!editing) {
        return (
            <Card variant='outlined' sx={{ mb: 1 }}>
                <CardContent>
                    <Stack direction="row" justifyContent="space-between" alignItems="start" gap={1}>
                        <div>
                            <Typography variant="h6">{task.tittle}</Typography>
                            {task.description && (
                                <Typography variant="body2" color="text.secondary">
                                    {task.description}
                                </Typography>
                            )}
                            <Typography variant="caption" color="text.secondary">
                                Priority: {task.taskPriority} | Status: {task.status}
                            </Typography>
                        </div>
                        <IconButton size="small" onClick={() => setEditing(true)}>
                            <EditIcon fontSize="small" />
                        </IconButton>
                    </Stack>
                </CardContent>
            </Card>
        );
    }

    return (
        <Card variant="outlined" sx={{ mb: 1}}>
            <CardContent>
                <Stack spacing={2}>
                    <TextField
                        label="Title"
                        value={local.tittle}
                        onChange={(e) => updatedField('tittle', e.target.value)}
                        size="small"
                        fullWidth
                    />
                    <TextField
                        label="Description"
                        value={local.description}
                        onChange={(e) => updatedField('description', e.target.value)}
                        size="small"
                        fullWidth
                        multiline
                        rows={2}
                    />
                    <TextField
                        select
                        label="Task Priority"
                        value={local.priority}
                        onChange={(e) => updatedField('priority', e.target.value)}
                        size="small"
                    >
                        {priorities.map((p) => (
                            <MenuItem key={p} value={p}>
                                {p}
                            </MenuItem>
                        ))}
                    </TextField>
                    <TextField
                        select
                        label="Task Status"
                        value={local.status}
                        onChange={(e) => updatedField('status', e.target.value)}
                        size="small"
                    >
                        {statuses.map((s) => (
                            <MenuItem key={s} value={s}>
                                {s}
                            </MenuItem>
                        ))}
                    </TextField>

                    <Stack direction="row" justifyContent="flex-end" spacing={1}>
                        <IconButton size="small" onClick={() => setEditing(false)}>
                            <CloseIcon fontSize="small" />
                        </IconButton>
                        <IconButton
                            size="small"
                            color="primary"
                            onClick={handleSave}
                            disabled={!hasChanges}
                        >
                            <SaveIcon fontSize="small" />
                        </IconButton>
                    </Stack>
                    
                </Stack>
            </CardContent>
        </Card>
    );

}

export default TaskCard;