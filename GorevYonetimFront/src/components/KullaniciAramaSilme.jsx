import React, { useState, useEffect } from 'react';
import { Input, Button, Table, message, Modal } from 'antd';
import axios from 'axios';
import './KullaniciAramaSilme.css';

function KullaniciAramaSilme() {
    const [searchTerm, setSearchTerm] = useState('');
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(false);
    const [deleteUserId, setDeleteUserId] = useState(null);

    useEffect(() => {
        if (searchTerm) {
            fetchUsers();
        }
    }, [searchTerm]);

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const response = await axios.get('http://localhost:5000/api/Kullanici');
            const filteredUsers = response.data.filter(user =>
                user.kullaniciAdi.toLowerCase().includes(searchTerm.toLowerCase())
            );
            setUsers(filteredUsers);
        } catch (error) {
            message.error('Kullanıcılar alınırken bir hata oluştu.');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        try {
            await axios.delete(`http://localhost:5000/api/Kullanici/${deleteUserId}`);
            setUsers(users.filter(user => user.id !== deleteUserId));
            message.success('Kullanıcı başarıyla silindi.');
        } catch (error) {
            message.error('Kullanıcı silinirken bir hata oluştu.');
        } finally {
            setDeleteUserId(null);
        }
    };

    const columns = [
        {
            title: 'Kullanıcı Adı',
            dataIndex: 'kullaniciAdi',
            key: 'kullaniciAdi',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },
        {
            title: 'Görevler',
            dataIndex: 'gorevler',
            key: 'gorevler',
            render: gorevler => gorevler.map(g => g.gorevAdi).join(', ')
        },
        {
            title: 'İşlemler',
            key: 'actions',
            render: (_, record) => (
                <Button
                    type="danger"
                    onClick={() => setDeleteUserId(record.id)}
                >
                    Sil
                </Button>
            ),
        },
    ];

    return (
        <div className="kullanici-arama-silme-container">
            <Input
                placeholder="Kullanıcı adı ara..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
            />
            <Table
                dataSource={users}
                columns={columns}
                loading={loading}
                rowKey="id"
            />
            <Modal
                title="Kullanıcıyı Sil"
                visible={!!deleteUserId}
                onOk={handleDelete}
                onCancel={() => setDeleteUserId(null)}
                okText="Evet"
                cancelText="Hayır"
            >
                <p>Bu kullanıcıyı silmek istediğinizden emin misiniz?</p>
            </Modal>
        </div>
    );
}

export default KullaniciAramaSilme;
