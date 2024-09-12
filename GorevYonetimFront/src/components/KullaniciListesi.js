import React, { useEffect, useState } from 'react';
import { fetchKullanicilar } from '../services/apiService';

const KullaniciListesi = () => {
    const [kullanicilar, setKullanicilar] = useState([]);

    useEffect(() => {
        const getKullanicilar = async () => {
            try {
                const data = await fetchKullanicilar();
                setKullanicilar(data);
            } catch (error) {
                console.error('Kullanıcıları alırken bir hata oluştu:', error);
            }
        };

        getKullanicilar();
    }, []);

    return (
        <div>
            <h2>Kullanıcı Listesi</h2>
            <ul>
                {kullanicilar.map(kullanici => (
                    <li key={kullanici.id}>{kullanici.kullaniciAdi}</li>
                ))}
            </ul>
        </div>
    );
};

export default KullaniciListesi;
