import React, { useState, useEffect } from 'react';
import { Alert, Spin } from 'antd';
import kullaniciService from '../services/kullaniciService';

const KullaniciListesi = () => {
    const [kullanicilar, setKullanicilar] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchKullanicilar = async () => {
            try {
                const response = await kullaniciService.getAllKullanicilar();
                setKullanicilar(response.data);
            } catch (err) {
                setError(`Kullanıcılar yüklenirken bir hata oluştu: ${err.message}`);
            } finally {
                setLoading(false);
            }
        };

        fetchKullanicilar();
    }, []);

    return (
        <div>
            <h2>Kullanıcı Listesi</h2>
            {loading && <Spin />}
            {error && <Alert message={error} type="error" showIcon />}
            {!loading && !kullanicilar.length && <p>Henüz kullanıcı yok.</p>}
            <ul>
                {kullanicilar.map((kullanici) => (
                    <li key={kullanici.id || kullanici.Id}>{kullanici.kullaniciAdi}</li>
                ))}
            </ul>
        </div>
    );
};

export default KullaniciListesi;
