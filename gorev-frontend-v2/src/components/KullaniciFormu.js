// src/components/KullaniciFormu.js
import React, { useState } from 'react';
import { createKullanici } from '../services/apiService';

const KullaniciFormu = () => {
    const [kullaniciAdi, setKullaniciAdi] = useState('');
    const [email, setEmail] = useState('');
    const [sifre, setSifre] = useState('');

    const handleSubmit = async (event) => {
        event.preventDefault();
        try {
            const yeniKullanici = {
                kullaniciAdi,
                email,
                sifre
            };
            await createKullanici(yeniKullanici);
            // Formu temizleyin veya kullanıcıyı bilgilendirin
            setKullaniciAdi('');
            setEmail('');
            setSifre('');
            alert('Kullanıcı başarıyla eklendi!');
        } catch (error) {
            console.error('Kullanıcı oluşturulurken bir hata oluştu:', error);
            alert('Bir hata oluştu, kullanıcı oluşturulamadı.');
        }
    };

    return (
        <div>
            <h2>Yeni Kullanıcı Ekle</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Kullanıcı Adı:</label>
                    <input
                        type="text"
                        value={kullaniciAdi}
                        onChange={(e) => setKullaniciAdi(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Şifre:</label>
                    <input
                        type="password"
                        value={sifre}
                        onChange={(e) => setSifre(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Ekle</button>
            </form>
        </div>
    );
};

export default KullaniciFormu;
