﻿import React, { useState } from 'react';
import axios from 'axios';
import { useHistory } from 'react-router-dom';

const Signup = () => {
    const history = useHistory();
    const [formData, setFormData] = useState({
        name:'',
        email: '',
        passwordhash: ''
    });
    const onTextChange = e => {
        const copy = { ...formData };
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        await axios.post('/api/jokes/signup', formData);
        history.push('/login');

    }
    return (
        <div className="row">
            <div className="col-md-6 offset-md-3 card card-body bg-light">
                <h3>Sign up for a new account</h3>
                <form onSubmit={onFormSubmit}>
                    <input onChange={onTextChange} value={formData.name} type="text" name="name" placeholder="Name" className="form-control" />
                    <br />
                    <input onChange={onTextChange} value={formData.email} type="text" name="email" placeholder="Email" className="form-control" />
                    <br />
                    <input onChange={onTextChange} value={formData.passwordhash} type="password" name="passwordhash" placeholder="Password" className="form-control" />
                    <br />
                    <button className="btn btn-primary">Signup</button>
                </form>
            </div>
        </div>
    );
}
export default Signup;
