import React, { useState } from 'react';
import { Modal, Button } from 'antd';

const ExampleModal = () => {
    const [isModalOpen, setIsModalOpen] = useState(false);

    const showModal = () => {
        setIsModalOpen(true);
    };

    const handleCancel = () => {
        setIsModalOpen(false);
    };

    return (
        <>
            <Button type="primary" onClick={showModal}>
                Modal Aç
            </Button>
            <Modal open={isModalOpen} onCancel={handleCancel}>
                <p>Modal Ýçeriði</p>
            </Modal>
        </>
    );
};

export default ExampleModal;
