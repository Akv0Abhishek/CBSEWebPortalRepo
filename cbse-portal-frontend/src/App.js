import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './components/Login';
import RegisterAdmin from './components/RegisterAdmin';
import RegisterPrincipal from './components/RegisterPrincipal';
import AdminDashboard from './components/AdminDashboard';
import PrincipalDashboard from './components/PrincipalDashboard';
import StudentsPage from './components/StudentsPage';
import MarksPage from './components/MarksPage';
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/register-admin" element={<RegisterAdmin />} />
        <Route path="/register-principal" element={<RegisterPrincipal />} />
        <Route path="/admin-dashboard" element={<AdminDashboard />} />
        <Route path="/principal-dashboard" element={<PrincipalDashboard />} />
        <Route path="/students" element={<StudentsPage />} />
        <Route path="/marks/:studentId" element={<MarksPage />} />
      </Routes>
    </Router>
  );
}

export default App;
