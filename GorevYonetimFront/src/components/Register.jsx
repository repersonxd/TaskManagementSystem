import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { notification } from 'antd';
import './Register.css';

const Register = () => {
    const [formData, setFormData] = useState({ kullaniciAdi: '', email: '', sifre: '', confirmSifre: '' });
    const [error, setError] = useState(null);
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const navigate = useNavigate();

    const openNotification = (type, message) => {
        notification[type]({
            message,
            style: {
                backgroundColor: '#1C1A1B', // Bildirim arka plan rengi
                color: '#E2DCDD', // Bildirim yazı rengi
                borderRadius: '8px',
                border: '1px solid #E2DCDD',
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
        if (formData.sifre !== formData.confirmSifre) {
            setError('Şifreler eşleşmiyor.');
            return;
        }

        try {
            const response = await axios.post('http://localhost:5000/api/Kullanici/Register', formData);
            if (response.status === 201) {
                openNotification('success', 'Başarıyla kaydolundu!');
                navigate('/login');
            } else {
                setError('Kayıt işlemi başarısız oldu.');
            }
        } catch (err) {
            setError('Kayıt işlemi başarısız oldu.');
        }
    };

    return (
        <div className="form-container">
            <h2>Kayıt Ol</h2>
            {error && <p className="register-error">{error}</p>}
            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Kullanıcı Adı</label>
                    <input type="text" name="kullaniciAdi" onChange={handleChange} placeholder="Kullanıcı Adı" />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input type="email" name="email" onChange={handleChange} placeholder="Email" />
                </div>
                <div className="form-group">
                    <label>Şifre</label>
                    <div className="password-container">
                        <input
                            type={showPassword ? "text" : "password"}
                            name="sifre"
                            onChange={handleChange}
                            placeholder="Şifre"
                        />
                        <button
                            type="button"
                            className="toggle-password"
                            onClick={() => setShowPassword(!showPassword)}
                        >
                            {showPassword ? "Gizle" : "Göster"}
                        </button>
                    </div>
                </div>
                <div className="form-group">
                    <label>Şifreyi Onayla</label>
                    <div className="password-container">
                        <input
                            type={showConfirmPassword ? "text" : "password"}
                            name="confirmSifre"
                            onChange={handleChange}
                            placeholder="Şifreyi Onayla"
                        />
                        <button
                            type="button"
                            className="toggle-password"
                            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                        >
                            {showConfirmPassword ? "Gizle" : "Göster"}
                        </button>
                    </div>
                </div>
                <button type="submit" className="register-button">Kayıt Ol</button>
            </form>
        </div>
    );
};

export default Register;
