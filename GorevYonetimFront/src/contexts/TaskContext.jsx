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
        try {
            const response = await axios.get('http://localhost:7257/api/tasks');
            setTasks(response.data);
        } catch (err) {
            setError(err);
        } finally {
            setLoading(false);
        }
    }, []);

    // Görev silme
    const deleteTask = async (id) => {
        try {
            await axios.delete(`http://localhost:7257/api/tasks/${id}`);
            setTasks((prevTasks) => prevTasks.filter((task) => task.id !== id));
        } catch (err) {
            setError(err);
        }
    };

    return (
        <TaskContext.Provider value={{ tasks, loading, error, fetchTasks, deleteTask }}>
            {children}
        </TaskContext.Provider>
    );
}

// PropTypes ile props doðrulamasý ekleyin
TaskProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export function useTasks() {
    const context = useContext(TaskContext);
    if (!context) {
        throw new Error('useTasks must be used within a TaskProvider');
    }
    return context;
}

export default TaskContext;
