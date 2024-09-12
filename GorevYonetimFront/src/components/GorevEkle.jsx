import React, { useState } from 'react';
import axios from 'axios';
import { Form, Input, Button, notification } from 'antd';
import './GorevEkle.css';

const GorevEkle = ({ onTaskAdded }) => {
    const [loading, setLoading] = useState(false);
    const [tamamlandi, setTamamlandi] = useState('hayir'); // Default to 'hayir'

    // Notification function
    const openNotification = (type, message) => {
        notification.open({
            message,
            style: {
                backgroundColor: '#1C1A1B',
                color: '#E2DCDD',
            },
        });
    };

    const onFinish = async (values) => {
        setLoading(true);
        try {
            const kullaniciId = sessionStorage.getItem('KullaniciId'); // Retrieve KullaniciId from session storage
            if (!kullaniciId) {
                openNotification('error', 'Kullanıcı kimliği bulunamadı.');
                setLoading(false);
                return;
            }

            const data = {
                GorevAdi: values.GorevAdi,
                Aciklama: values.Aciklama,
                Tamamlandi: tamamlandi === 'evet' ? true : false, // Ensure Tamamlandi is true/false
                KullaniciId: kullaniciId, // Include KullaniciId in the request
            };

            console.log("Gönderilen veri:", data); // Debugging the sent data
            const token = sessionStorage.getItem('token');

            if (!token) {
                openNotification('error', 'Oturum açma hatası: Geçersiz token.');
                setLoading(false);
                return;
            }

            // Task addition request
            const response = await axios.post('http://localhost:5000/api/tasks', data, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (response.status === 201) {
                openNotification('success', 'Görev başarıyla eklendi!');
                onTaskAdded(); // Refresh the task list when a task is added
            } else {
                openNotification('error', 'Görev eklenemedi, lütfen tekrar deneyin.');
            }
        } catch (error) {
            console.error('Görev ekleme hatası:', error);
            openNotification('error', 'Görev eklenirken bir hata oluştu.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="gorev-ekle-form">
            <Form onFinish={onFinish}>
                <Form.Item
                    label="Görev Başlığı"
                    name="GorevAdi"
                    rules={[{ required: true, message: 'Lütfen görev başlığını girin!' }]}
                >
                    <Input placeholder="Başlık" />
                </Form.Item>

                <Form.Item
                    label="Görev Açıklaması"
                    name="Aciklama"
                    rules={[{ required: true, message: 'Lütfen görev açıklamasını girin!' }]}
                >
                    <Input.TextArea rows={4} placeholder="Açıklama" />
                </Form.Item>

                <div className="tamamlandi-soru">Tamamlandı mı?</div>
                <div className="button-group">
                    <Button
                        className={`option-button ${tamamlandi === 'evet' ? 'selected' : ''}`}
                        onClick={() => setTamamlandi('evet')}
                    >
                        Evet
                    </Button>
                    <Button
                        className={`option-button ${tamamlandi === 'hayir' ? 'selected' : ''}`}
                        onClick={() => setTamamlandi('hayir')}
                    >
                        Hayır
                    </Button>
                </div>

                <Form.Item>
                    <Button type="primary" htmlType="submit" className="submit-button" loading={loading}>
                        Ekle
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
};

export default GorevEkle;
