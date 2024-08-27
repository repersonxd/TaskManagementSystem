import React, { useState } from 'react';
import { useTasks } from '../contexts/TaskContext';
import { Form, Input, Button, message } from 'antd';

function GorevEkle() {
    const [form] = Form.useForm();
    const { addTask } = useTasks();
    const [loading, setLoading] = useState(false);

    const onFinish = async (values) => {
        setLoading(true);
        try {
            await addTask(values);
            form.resetFields();
            message.success('Görev baþarýyla eklendi.');
        } catch  {
            message.error('Görev eklenirken bir hata oluþtu.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Form form={form} onFinish={onFinish}>
                <Form.Item name="title" label="Baþlýk" rules={[{ required: true, message: 'Baþlýk gereklidir' }]}>
                    <Input />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>Ekle</Button>
                </Form.Item>
            </Form>
        </div>
    );
}

export default GorevEkle;
