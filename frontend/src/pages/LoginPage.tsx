import React, { useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import {
    Container,
    TextField,
    Button,
    Typography,
    Alert,
    Stack,
    Link,
    Paper
} from "@mui/material";
import { useAuth } from '../context/AuthContext';
import { AuthResponse } from '../types';
import { api } from '../services/api';
import { login as loginApi } from '../services/auth';
import { useNavigate, Link as RouterLink } from 'react-router-dom';

interface FormValues {
    email: string;
    password: string;
}

type LoginForm = {
    email: string;
    password: string;
};

function LoginPage() {
    const { login } = useAuth();
    const navigate = useNavigate();
    const {
        control,
        handleSubmit,
        formState: { isSubmitting },
    } = useForm<FormValues>();
    const [error, setError] = useState("");

    const onSubmit = async (data: FormValues) => {
        try {
            console.log("We are here ----");
            //const resp = await api.post<AuthResponse>("/login", data);
            const resp = await loginApi(data.email, data.password);
            login(resp);
            navigate('/');

        } catch (err: unknown) {
            setError(err instanceof Error ? err.message : "Login failed");
        }
    };

    return (
        <Container maxWidth="sm" sx={{ mt: 8 }}>
            <Paper sx={{ p: 3 }}>
            <Stack spacing={3} mt={10}>
                <Typography variant="h4" component="h1" sx={{ textAlign: "center"}}>
                    Login
                </Typography>

                {error && <Alert severity="error">{error}</Alert>}

                <form onSubmit={handleSubmit(onSubmit)} noValidate>
                    <Controller
                        name="email"
                        control={control}
                        defaultValue=""
                        rules={{
                            required: "Email is required",
                            pattern: {
                                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                                message: "Please enter a valid email",
                            },
                        }}
                        render={({ field, fieldState }) => (
                            <TextField
                            {...field}
                            label="Email"
                            type="email"
                            fullWidth
                            margin="normal"
                            error={!!fieldState.error}
                            helperText={fieldState.error?.message}
                            autoFocus
                        />
                        )}
                    />

                    <Controller
                        name="password"
                        control={control}
                        defaultValue=""
                        rules={{
                            required: "Password is required",
                            minLength: { value: 6, message: "Atleast 6 characters"},
                        }}
                        render={({ field, fieldState }) => (
                            <TextField
                                {...field}
                                label="Password"
                                type="password"
                                fullWidth
                                margin="normal"
                                error={!!fieldState.error}
                                helperText={fieldState.error?.message}
                            />
                        )}
                    />

                    <Button
                        type="submit"
                        variant="contained"
                        fullWidth
                        sx={{ mt: 2 }}
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? "Loggin in.." : "Login"}
                    </Button>
                    <Typography 
                        variant="body2"
                        sx={{ mt: 2, textAlign: "center"}}
                        >
                        No Account? <Link component={RouterLink}
                            to="/register"
                            underline="hover"
                            sx={{ fontWeight: "bold"}}
                            >Register</Link>
                    </Typography>
                </form>
            </Stack>
            </Paper>
        </Container>
    );
};


export default LoginPage;
