import React, { useState } from 'react';
import axios from 'axios';
import { Form, Input, Button, notification } from 'antd';
import './GorevEkle.css';

const GorevEkle = ({ onTaskAdded }) => {
    const [loading, setLoading] = useState(false);
    const [tamamlandi, setTamamlandi] = useState(null);

    // Bildirim fonksiyonu
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
            const data = {
                GorevAdi: values.GorevAdi,
                Aciklama: values.Aciklama,
                Tamamlandi: tamamlandi === 'evet' ? true : false,
            };

            console.log("Gönderilen veri:", data); // Verileri kontrol edin
            const token = sessionStorage.getItem('token');

            if (!token) {
                openNotification('error', 'Oturum açma hatası: Geçersiz token.');
                setLoading(false);
                return;
            }

            // Görev ekleme isteği
            const response = await axios.post('http://localhost:5000/api/tasks', data, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (response.status === 201) {
                openNotification('success', 'Görev başarıyla eklendi!');
                onTaskAdded(); // Görev eklendiğinde listeyi güncelle
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
