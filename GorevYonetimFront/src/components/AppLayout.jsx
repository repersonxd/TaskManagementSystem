import React from 'react';
import { Layout, Menu } from 'antd';

const { Header, Content, Footer } = Layout;

const AppLayout = () => (
    <Layout className="layout">
        <Header>
            <div className="logo" />
            <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['1']}>
                <Menu.Item key="1">Home</Menu.Item>
                <Menu.Item key="2">Tasks</Menu.Item>
                <Menu.Item key="3">Profile</Menu.Item>
            </Menu>
        </Header>
        <Content style={{ padding: '0 50px' }}>
            <div className="site-layout-content" style={{ marginTop: '20px' }}>
                {/* İçerik burada olacak */}
            </div>
        </Content>
        <Footer style={{ textAlign: 'center' }}>Görev Yönetim Sistemi ©2024</Footer>
    </Layout>
);

export default AppLayout;
