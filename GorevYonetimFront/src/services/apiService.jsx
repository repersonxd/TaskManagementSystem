import axios from 'axios';

const API_URL = 'http://localhost:5260/api/tasks';  // Backend API URL'iniz

export const fetchKullanicilar = async () => {
    try {
        const response = await axios.get(API_URL);
        return response.data;
    } catch (error) {
        console.error('Kullanýcýlarý alýrken bir hata oluþtu:', error);
        throw error;
    }
};
