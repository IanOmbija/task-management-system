import { api } from "./api";
import { AuthResponse } from "../types";
import axios, { type AxiosResponse } from "axios";

/**
 * Helper: Handling API calls
 */
async function handleRequest<T>(promise: Promise<{data: T}>): Promise<T> {
    try {
        const { data } = await promise;
        return data;
    } catch ( error: any) {
        console.log("check this error:",error);
        const message = 
        error?.response?.data?.message ||
        'Something went wrong. Please try again later.';
        throw new Error(message);
      
    }
}


/**
 * Login user with email and password.
 * @param email 
 * @param password 
 */

export async function login(email: string, password: string): Promise<AuthResponse> {
    return handleRequest (
        api.post<AuthResponse>('/auth/login', { email, password })
    );
}

/**
 * Register a new user
 * @param username 
 * @param email 
 * @param password 
 * @returns 
 */
export async function register(
    username: string,
    email: string,
    password: string
): Promise<AuthResponse> {
    return handleRequest(
        api.post<AuthResponse>('/auth/register', {username, email, password})
    );
}

