import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
    plugins: [react()],
    server: {
        port: 5173, // Sabit bir port numarasý belirleyin
        strictPort: true, // Port kullanýlamýyorsa hata verir
    },
});
