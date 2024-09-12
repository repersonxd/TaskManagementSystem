import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { notification } from 'antd';
import './Login.css';

const Login = () => {
    const [formData, setFormData] = useState({
        kullaniciAdi: '',
        sifre: ''
    });
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    // Notification function
    const openNotification = (type, message) => {
        notification[type]({
            message,
            className: 'notification',
            style: {
                backgroundColor: '#1C1A1B',
                color: '#E2DCDD',
            },
        });
    };

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);
        setLoading(true);

        if (!formData.kullaniciAdi || !formData.sifre) {
            setError('Kullanıcı adı ve şifre boş olamaz.');
            setLoading(false);
            return;
        }

        try {
            const response = await axios.post('http://localhost:5000/api/Kullanici/Login', formData);
            console.log('Full response from backend:', response.data); // Debugging the full response object

            const { token, kullaniciId } = response.data; // Extract token and kullaniciId
            console.log('Token:', token); // Check if token is correctly received
            console.log('KullaniciId:', kullaniciId); // Check if kullaniciId is correctly received

            if (token && kullaniciId) {
                // Store both token and KullaniciId in sessionStorage
                sessionStorage.setItem('token', token);
                sessionStorage.setItem('KullaniciId', kullaniciId);
                openNotification('success', 'Başarıyla giriş yapıldı!');
                navigate('/gorev-anasayfa'); // Redirect after successful login
            } else {
                setError('Geçersiz yanıt. Kullanıcı ID veya Token alınamadı.');
            }
        } catch (err) {
            console.error('API isteğinde bir hata oluştu:', err);
            setError('Giriş işlemi başarısız oldu. Lütfen tekrar deneyin.');
        } finally {
            setLoading(false);
        }
    };

    const handleRegister = () => {
        navigate('/register');
    };

    return (
        <div className="form-container">
            <h2>Giriş Yap</h2>
            {error && <p className="login-error">{error}</p>}
            <form onSubmit={handleSubmit} className="login-form">
                <div className="form-group">
                    <label htmlFor="kullaniciAdi" className="custom-label">Kullanıcı Adı</label>
                    <input
                        type="text"
                        id="kullaniciAdi"
                        name="kullaniciAdi"
                        placeholder="Kullanıcı Adı"
                        value={formData.kullaniciAdi}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="sifre" className="custom-label">Şifre</label>
                    <input
                        type="password"
                        id="sifre"
                        name="sifre"
                        placeholder="Şifre"
                        value={formData.sifre}
                        onChange={handleChange}
                        required
                    />
                </div>
                <div className="button-group">
                    <button type="submit" className="login-button" disabled={loading}>
                        {loading ? 'Giriş Yapılıyor...' : 'Giriş Yap'}
                    </button>
                    <button type="button" className="register-button" onClick={handleRegister} disabled={loading}>
                        Kayıt Ol
                    </button>
                </div>
            </form>
        </div>
    );
};

export default Login;
