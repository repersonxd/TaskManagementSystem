import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useTasks } from '../contexts/TaskContext';
import { Form, Input, Button, message, Spin } from 'antd';

function GorevDuzenle() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { tasks, updateTask, loading } = useTasks();
    const [form] = Form.useForm();
    const [task, setTask] = useState(null);

    useEffect(() => {
        const taskToEdit = tasks.find(t => t.id === id);
        if (taskToEdit) {
            form.setFieldsValue(taskToEdit);
            setTask(taskToEdit);
        }
    }, [id, tasks, form]);

    const onFinish = async (values) => {
        try {
            await updateTask({ ...task, ...values });
            message.success('Görev başarıyla güncellendi.');
            navigate('/');
        } catch {
            message.error('Görev güncellenirken bir hata oluştu.');
        }
    };

    if (!task) {
        return <Spin size="large" />;
    }

    return (
        <div>
            <Form form={form} onFinish={onFinish}>
                <Form.Item name="title" label="Başlık" rules={[{ required: true, message: 'Başlık gereklidir' }]}>
                    <Input />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>Güncelle</Button>
                </Form.Item>
            </Form>
        </div>
    );
}

export default GorevDuzenle;
