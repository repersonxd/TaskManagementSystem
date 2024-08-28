import React, { useState, useEffect } from 'react';
import axios from 'axios';  // Axios'u buraya ekleyin
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { TaskProvider } from "./contexts/TaskContext";
import UserDetails from "./components/UserDetails";
import GorevListesi from "./components/GorevListesi";
import GorevEkle from "./components/GorevEkle";
import GorevDuzenle from "./components/GorevDuzenle";
import UserProfile from "./components/UserProfile";

function App() {
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await axios.get('http://localhost:7257/api/Kullanici');
                setUserData(response.data);
            } catch (error) {
                console.error('Error fetching user data:', error);
            }
        };

        fetchUserData();
    }, []);

    if (!userData) {
        return <div>Loading...</div>;
    }

    return (
        <Router>
            <TaskProvider>
                <Routes>
                    <Route path="/users/:id" element={<UserDetails />} />
                    <Route path="/" element={<GorevListesi />} />
                    <Route path="/gorev-ekle" element={<GorevEkle />} />
                    <Route path="/gorev-duzenle/:id" element={<GorevDuzenle />} />
                    <Route path="/user-profile" element={<UserProfile
                        name={userData.name}
                        surname={userData.surname}
                        profilePicture={userData.profilePicture}
                    />} />
                </Routes>
            </TaskProvider>
        </Router>
    );
}

export default App;
