import React, { useState } from 'react';
import apiService from '../services/apiService';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import { useNavigate } from 'react-router-dom';

const RegisterPrincipal = () => {
  const [email, setEmail] = useState('');
  const [name, setName] = useState('');
  const [password, setPassword] = useState('');
  const [schoolName, setSchoolName] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await apiService.registerPrincipal({ email, name, password,schoolName });
      navigate("/");
    } catch (error) {
      alert('Error: ' + error.message);
    }
  };

  return (
    <div style={{width: '500px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Register Principal</h1>
      <Form onSubmit={handleSubmit}> 
      <Form.Group className="mb-3" controlId="formBasicRegisterName">
        <Form.Label>Name</Form.Label>
        <Form.Control type="text" placeholder="Enter Name" 
          value={name}
          onChange={(e) => setName(e.target.value)}
          required />
      </Form.Group>

      <Form.Group className="mb-3" controlId="formBasicRegisterEmail">
        <Form.Label>Email address</Form.Label>
        <Form.Control type="email" placeholder="Enter email" 
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required />
      </Form.Group>

      <Form.Group className="mb-3" controlId="formBasicPassword">
        <Form.Label>Password</Form.Label>
        <Form.Control type="password" placeholder="Password" 
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        required/>
      </Form.Group>
      <Form.Group className="mb-3" controlId="formBasicRegisterSchoolName">
        <Form.Label>School Name</Form.Label>
        <Form.Control type="text" placeholder="Enter School Name" 
          value={schoolName}
          onChange={(e) => setSchoolName(e.target.value)}
          required />
      </Form.Group>
      <Button variant="primary" type="submit">
        Register
      </Button> 
    </Form>
    </div> 
  );
};

export default RegisterPrincipal;
