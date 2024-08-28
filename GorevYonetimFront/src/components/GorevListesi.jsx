import React, { useEffect, useState } from 'react';
import { List, Button, Spin, message } from 'antd';
import axios from 'axios';

function GorevListesi() {
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchTasks = async () => {
        try {
            setLoading(true);
            const response = await axios.get('http://localhost:7257/api/tasks');
            setTasks(response.data);
            setLoading(false);
        } catch (err) {
            setError(err.message);
            setLoading(false);
        }
    };

    const deleteTask = async (taskId) => {
        try {
            await axios.delete(`http://localhost:7257/api/tasks/${taskId}`);
            setTasks(tasks.filter(task => task.id !== taskId));
            message.success('Görev baþarýyla silindi.');
        } catch (err) {
            message.error('Görev silinirken bir hata oluþtu.');
        }
    };

    useEffect(() => {
        fetchTasks(); // Görevleri al
    }, []);

    useEffect(() => {
        if (error) {
            message.error('Görevler alýnýrken bir hata oluþtu.');
        }
    }, [error]);

    if (loading) {
        return <div style={{ textAlign: 'center', marginTop: 50 }}><Spin size="large" /></div>;
    }

    return (
        <div>
            <List
                dataSource={tasks}
                renderItem={(task) => (
                    <List.Item
                        key={task.id}  // Buraya key prop'u eklendi
                        actions={[
                            <Button
                                key={`delete-${task.id}`} // Buraya da key prop'u eklendi
                                onClick={() => deleteTask(task.id)}
                                type="primary"
                                danger
                            >
                                Sil
                            </Button>,
                        ]}
                    >
                        {task.title}
                    </List.Item>
                )}
            />
        </div>
    );
}

export default GorevListesi;
