import { api } from "./api";
import { User } from "../types";

export async function fetchUsers() {
    const { data } = await api.get<User[]>('/users');
    return data;

}