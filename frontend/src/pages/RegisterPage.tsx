import React from 'react';
import {
  Container,
  Paper,
  Typography,
  TextField,
  Button,
  Stack,
  Link,
} from '@mui/material';
import { useForm, SubmitHandler } from 'react-hook-form';
import { register as registerApi } from '../services/auth';
import { useAuth } from '../context/AuthContext';
import { useNavigate, Link as RouterLink } from 'react-router-dom';


interface RegisterFormValues {
  username: string;
  email: string;
  password: string;
}

const Register: React.FC = () => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormValues>();

  const { login } = useAuth();
  const navigate = useNavigate();

  // Handle form submission
  const onSubmit: SubmitHandler<RegisterFormValues> = async (values) => {
    const resp = await registerApi(values.username, values.email, values.password);
    login(resp);
    navigate('/');
  };

  return (
    <Container maxWidth="xs" sx={{ mt: 8 }}>
      <Paper sx={{ p: 3 }}>
        <Typography variant="h6" gutterBottom>
          Create Account
        </Typography>

        <Stack
          component="form"
          spacing={2}
          onSubmit={handleSubmit(onSubmit)}
          noValidate
        >
          <TextField
            label="Username"
            fullWidth
            {...register('username', {
              required: 'Username is required',
              minLength: { value: 3, message: 'Minimum 3 characters required' },
            })}
            error={!!errors.username}
            helperText={errors.username?.message}
          />

          <TextField
            label="Email"
            type="email"
            fullWidth
            {...register('email', {
              required: 'Email is required',
            })}
            error={!!errors.email}
            helperText={errors.email?.message}
          />

          <TextField
            label="Password"
            type="password"
            fullWidth
            {...register('password', {
              required: 'Password is required',
              minLength: { value: 6, message: 'Minimum 6 characters required' },
            })}
            error={!!errors.password}
            helperText={errors.password?.message}
          />

          <Button
            type="submit"
            variant="contained"
            disabled={isSubmitting}
            fullWidth
          >
            {isSubmitting ? 'Registering...' : 'Register'}
          </Button>

          <Typography variant="body2" align="center">
            Already have an account?{' '}
            <Link component={RouterLink} to="/login">
              Login
            </Link>
          </Typography>
        </Stack>
      </Paper>
    </Container>
  );
};

export default Register;
