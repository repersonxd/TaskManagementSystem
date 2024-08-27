import 'antd/dist/reset.css';
import './App.css'; 
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { TaskProvider } from "./contexts/TaskContext";
import UserDetails from "./components/UserDetails";
import GorevListesi from "./components/GorevListesi";
import GorevEkle from "./components/GorevEkle";
import GorevDuzenle from "./components/GorevDuzenle";
import UserProfile from "./components/UserProfile";

function App() {
    return (
        <Router>
            <TaskProvider>
                <Routes>
                    <Route path="/users/:id" element={<UserDetails />} />
                    <Route path="/" element={<GorevListesi />} />
                    <Route path="/gorev-ekle" element={<GorevEkle />} />
                    <Route path="/gorev-duzenle/:id" element={<GorevDuzenle />} />
                    <Route path="/user-profile" element={<UserProfile />} />
                </Routes>
            </TaskProvider>
        </Router>
    );
}

export default App;
