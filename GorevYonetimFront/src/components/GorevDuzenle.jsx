
import { useParams, useNavigate } from 'react-router-dom';
import { useTasks } from '../contexts/TaskContext';
import { Form, Input, Button, message, Spin } from 'antd';

function GorevDuzenle() {
    const { id } = useParams();
    const navigate = useNavigate();
    const { tasks, updateTask, loading } = useTasks();
    const [form] = Form.useForm();
    const [task, setTask] = (null);

    (() => {
        const taskToEdit = tasks.find(t => t.id === id);
        if (taskToEdit) {
            form.setFieldsValue(taskToEdit);
            setTask(taskToEdit);
        }
    }, [id, tasks, form]);

    const onFinish = async (values) => {
        try {
            await updateTask({ ...task, ...values });
            message.success('Görev baþarýyla güncellendi.');
            navigate('/');
        } catch  {
            message.error('Görev güncellenirken bir hata oluþtu.');
        }
    };

    if (!task) {
        return <Spin size="large" />;
    }

    return (
        <div>
            <Form form={form} onFinish={onFinish}>
                <Form.Item name="title" label="Baþlýk" rules={[{ required: true, message: 'Baþlýk gereklidir' }]}>
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
