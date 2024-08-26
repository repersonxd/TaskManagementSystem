import React from 'react';
import KullaniciListesi from './components/KullaniciListesi';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Görev Yönetim Sistemi</h1>
      </header>
      <main>
        <KullaniciListesi />
      </main>
    </div>
  );
}

export default App;
