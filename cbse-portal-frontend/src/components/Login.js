import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import authService from '../services/authService';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [role, setRole] = useState('admin');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await authService.login(email, password, role);
      if (role === 'admin') {
        navigate('/admin-dashboard');
      } else {
        navigate('/principal-dashboard');
      }
    } catch (error) {
      alert('Login failed: ' + error.message);
    }
  };
  const handleRegisterPrincipal = () => {
    navigate("/register-principal"); 
  };
  const handleRoleChange = (e) => {
    setRole(e.target.value);
  };

  return (
    <div style={{width: '500px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Login</h1>
      <Form onSubmit={handleSubmit}> 
      <Form.Group style={{}} className="mb-3" controlId="formBasicEmail">
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
      <div className="mb-3">
        <Form.Check
            type="radio"
            name="role"
            id="User_role1"
            label="Admin"
            value="admin"
            checked={role === "admin"} 
            onChange={handleRoleChange}
        />

          <Form.Check
          type="radio"
          name="role"
          id="User_role2"
          label="Principal"
          value="principal"
          checked={role === "principal"}
          onChange={handleRoleChange}
        />
        </div>
      <div className='d-flex justify-content-between'>
      <Button variant="success" type="submit">
        Login
      </Button> 
      <Button variant="light" onClick={handleRegisterPrincipal}>Register Principal</Button>
      </div>
    </Form>
    </div>
  );
};

export default Login;
