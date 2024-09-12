import React, { createContext, useState, useCallback, useContext } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';

const TaskContext = createContext();

export function TaskProvider({ children }) {
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    // Görevleri al
    const fetchTasks = useCallback(async () => {
        setLoading(true);
        setError(null); // Yeni bir istek yapmadan önce önceki hatayı sıfırlayın
        try {
            const response = await axios.get('http://localhost:7257/api/tasks');
            setTasks(response.data);
        } catch (err) {
            setError(err.response ? err.response.data : err.message);
        } finally {
            setLoading(false);
        }
    }, []);

    // Görev silme
    const deleteTask = useCallback(async (id) => {
        setError(null); // Silme işleminden önce hatayı sıfırlayın
        try {
            await axios.delete(`http://localhost:7257/api/tasks/${id}`);
            setTasks((prevTasks) => prevTasks.filter((task) => task.id !== id));
        } catch (err) {
            setError(err.response ? err.response.data : err.message);
        }
    }, []);

    return (
        <TaskContext.Provider value={{ tasks, loading, error, fetchTasks, deleteTask }}>
            {children}
        </TaskContext.Provider>
    );
}

// PropTypes ile props doğrulaması ekleyin
TaskProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

// Görevleri kullanmak için özel hook
export function useTasks() {
    const context = useContext(TaskContext);
    if (!context) {
        throw new Error('useTasks must be used within a TaskProvider');
    }
    return context;
}

export default TaskContext;
