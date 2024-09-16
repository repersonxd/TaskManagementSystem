import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { TaskProvider } from "./contexts/TaskContext";
import UserDetails from "./components/UserDetails";
import GorevListesi from "./components/GorevListesi";
import GorevEkle from "./components/GorevEkle";
import GorevDuzenle from "./components/GorevDuzenle";
import UserProfile from "./components/UserProfile";
import Login from "./components/Login";
import Register from "./components/Register";
import GorevAnasayfa from "./components/GorevAnasayfa";

const App = () => {
    const [userData, setUserData] = useState(null);
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const token = sessionStorage.getItem('token');
        const kullaniciId = sessionStorage.getItem('KullaniciId');

        if (token && kullaniciId) {
            // If token exists, set user as logged in
            setIsLoggedIn(true);

            // Optionally, fetch user data with the token
            axios.get('http://localhost:5000/api/Kullanici', {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
                .then((response) => {
                    setUserData(response.data);
                })
                .catch((error) => {
                    console.error("Error fetching user data:", error);
                    setIsLoggedIn(false);
                });
        }
    }, []);

    return (
        <Router>
            <TaskProvider>
                <Routes>
                    <Route path="/" element={isLoggedIn ? <Navigate to="/gorev-anasayfa" /> : <Navigate to="/login" />} />
                    <Route path="/login" element={<Login setUserData={setUserData} setIsLoggedIn={setIsLoggedIn} />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/gorev-anasayfa" element={isLoggedIn ? <GorevAnasayfa userData={userData} /> : <Navigate to="/login" />} />
                    <Route path="/gorev-ekle" element={isLoggedIn ? <GorevEkle /> : <Navigate to="/login" />} />
                    <Route path="/gorev-listesi" element={isLoggedIn ? <GorevListesi /> : <Navigate to="/login" />} />
                    <Route path="/gorev-duzenle/:id" element={isLoggedIn ? <GorevDuzenle /> : <Navigate to="/login" />} />
                    <Route path="/users/:id" element={isLoggedIn ? <UserDetails /> : <Navigate to="/login" />} />
                    <Route path="/user-profile" element={isLoggedIn ?
                        <UserProfile
                            name={userData?.kullaniciAdi || 'Unknown'}
                            surname={userData?.soyadi || 'Unknown'}
                            profilePicture={userData?.profilePicture || 'default.png'}
                        />
                        : <Navigate to="/login" />}
                    />
                    <Route path="*" element={<Navigate to={isLoggedIn ? "/gorev-anasayfa" : "/login"} />} />
                </Routes>
            </TaskProvider>
        </Router>
    );
};

export default App;
