import React from 'react';
import PropTypes from 'prop-types';
import '../App.css';

function UserProfile({ name, surname, profilePicture }) {
    if (!name || !surname || !profilePicture) {
        return <div>Loading...</div>;
    }

    return (
        <div className="user-profile">
            <img src={profilePicture} alt={`${name} ${surname}`} className="profile-picture" />
            <h1>{name} {surname}</h1>
        </div>
    );
}

UserProfile.propTypes = {
    name: PropTypes.string.isRequired,
    surname: PropTypes.string.isRequired,
    profilePicture: PropTypes.string.isRequired,
};

export default UserProfile;
