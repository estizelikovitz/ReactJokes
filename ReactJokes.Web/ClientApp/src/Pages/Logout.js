import getAxios from '../AuthAxios';
import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { useAuthContext } from '../AuthContext';

const Logout = () => {
    const history = useHistory();
    const { setUser } = useAuthContext();

    useEffect(() => {
        const doLogout = async () => {
            setUser(null);
            localStorage.removeItem('auth-token');
        }

        doLogout();
        history.push('/login');

    }, []);

    return <></>
}

export default Logout;