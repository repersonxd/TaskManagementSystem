import React, { useState } from 'react';
import axios from 'axios';

const KullaniciArama = ({ onSelectUser }) => {
    const [query, setQuery] = useState('');
    const [results, setResults] = useState([]);

    const handleSearch = async (e) => {
        setQuery(e.target.value);

        if (e.target.value.length > 2) {
            try {
                const response = await axios.get(`http://localhost:5000/api/Kullanici/Ara?query=${e.target.value}`);
                setResults(response.data);
            } catch (error) {
                console.error('Kullanıcı arama hatası:', error);
            }
        } else {
            setResults([]);
        }
    };

    return (
        <div className="kullanici-arama">
            <input
                type="text"
                placeholder="Kullanıcı Adı Ara"
                value={query}
                onChange={handleSearch}
            />
            <ul className="search-results">
                {results.map(user => (
                    <li key={user.id} onClick={() => onSelectUser(user)}>
                        {user.kullaniciAdi}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default KullaniciArama;
