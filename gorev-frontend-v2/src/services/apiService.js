import axios from 'axios';

const API_URL = 'https://localhost:7257/api'; // API'nizin URL'si

// Kullanıcıları listele
export const fetchKullanicilar = async () => {
    const response = await axios.get(`${API_URL}/Kullanici`);
    return response.data;
};

// Yeni kullanıcı oluştur
export const createKullanici = async (kullanici) => {
    const response = await axios.post(`${API_URL}/Kullanici`, kullanici);
    return response.data;
};

// Kullanıcıyı güncelle
export const updateKullanici = async (id, kullanici) => {
    const response = await axios.put(`${API_URL}/Kullanici/${id}`, kullanici);
    return response.data;
};

// Kullanıcıyı sil
export const deleteKullanici = async (id) => {
    await axios.delete(`${API_URL}/Kullanici/${id}`);
};

