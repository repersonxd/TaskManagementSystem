import React from 'react';
import './App.css';
import KullaniciListesi from './components/KullaniciListesi'; // Bileşeni içe aktarın

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <h1>Görev Yönetim Sistemi</h1>
            </header>
            <main>
                <KullaniciListesi /> {/* KullaniciListesi bileşenini ekleyin */}
            </main>
        </div>
    );
}

export default App;
