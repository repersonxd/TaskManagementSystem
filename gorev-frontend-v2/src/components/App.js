
// src/App.js
import React from 'react';
import KullaniciListesi from './components/KullaniciListesi';
import KullaniciFormu from './components/KullaniciFormu';


function App() {
    return (
        <div className="App">
            <header className="App-header">
                <h1>Görev Yönetim Sistemi</h1>
                <KullaniciListesi />
                <KullaniciFormu />
            </header>
        </div>
    );
}

export default App;
