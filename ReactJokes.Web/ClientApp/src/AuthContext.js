import React, { createContext, useContext, useEffect, useState } from 'react';
import getAxios from './AuthAxios';

const AuthContext = createContext();

const AuthContextComponent = ({ children }) => {

    const [user, setUser] = useState(null);

    useEffect(() => {
        const getUser = async () => {
            const { data } = await getAxios().get('/api/jokes/getcurrentuser');
            setUser(data);
        }

        getUser();
    }, []);


    return <AuthContext.Provider value={{ user, setUser }}>
        {children}
    </AuthContext.Provider>

}

const useAuthContext = () => useContext(AuthContext);


export { AuthContextComponent, useAuthContext };