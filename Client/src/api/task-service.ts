import axios from 'axios';
import { Task } from '../types/Task';

const API_BASE = 'https://localhost:7100/api/task';

export const getTasks = async (): Promise<Task[]> => {
    const response = await axios.get(API_BASE);
    return response.data;
};

export const addTask = async (task: Task): Promise<Task> => {
    const response = await axios.post(API_BASE, task, { headers: { 'Content-Type': 'application/json' } });
    return response.data;
};

export const updateTask = async (task: Task): Promise<Task> => {
    const response = await axios.put(`${API_BASE}`, task);
    return response.data;
};

export const deleteTask = async (id: string): Promise<void> => {
    await axios.delete(`${API_BASE}/${id}`);
};
