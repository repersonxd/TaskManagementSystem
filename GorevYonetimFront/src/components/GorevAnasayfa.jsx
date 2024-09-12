import React, { useState } from 'react';
import GorevEkle from './GorevEkle';
import GorevListesi from './GorevListesi';
import KullaniciAramaSilme from './KullaniciAramaSilme'; // Kullanıcı Arama ve Silme bileşeni
import './GorevAnasayfa.css';

const GorevAnasayfa = () => {
    const [updateList, setUpdateList] = useState(false);

    const handleTaskAdded = () => {
        setUpdateList(!updateList); // Görev eklendikçe listeyi günceller
    };

    return (
        <div className="anasayfa-container">
            <div className="welcome-message">
                Hoşgeldiniz!
            </div>
            <div className="section-container">
                <div className="gorev-ekle-container">
                    <h2>Görev Ekle</h2>
                    <GorevEkle onTaskAdded={handleTaskAdded} />
                </div>
                <div className="gorev-listesi-container">
                    <h2>Görev Listesi</h2>
                    <GorevListesi updateList={updateList} />
                </div>
                <div className="kullanici-arama-container">
                    <h2>Kullanıcı Arama ve Silme</h2>
                    <KullaniciAramaSilme />
                </div>
            </div>
        </div>
    );
};

export default GorevAnasayfa;
