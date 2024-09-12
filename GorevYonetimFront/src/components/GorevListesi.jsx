import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { List, message, Spin } from 'antd';
import './GorevListesi.css';

const GorevListesi = () => {
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);

    const fetchTasks = async () => {
        try {
            const response = await axios.get('http://localhost:5000/api/tasks');
            setTasks(response.data);
        } catch (error) {
            message.error('Görevler yüklenirken bir hata oluştu.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTasks();
    }, []);

    if (loading) {
        return <Spin size="large" />;
    }

    return (
        <div className="gorev-listesi">
            <List
                itemLayout="horizontal"
                dataSource={tasks}
                renderItem={task => (
                    <List.Item>
                        <List.Item.Meta
                            title={<span className="task-title">{task.title}</span>}
                            description={<span className="task-description">{task.description}</span>}
                        />
                    </List.Item>
                )}
            />
        </div>
    );
};

export default GorevListesi;
