import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { API_URL } from "../services/endPoints";

const PrincipalDashboard = () => {
  const [studentName, setStudentName] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [principalId, setPrincipalId] = useState("");
  const [SchoolName, setSchoolName] = useState("");
  const [principalName, setPrincipalName] = useState("");
  const navigate = useNavigate(); 

  useEffect(() => {
    const id = localStorage.getItem("principalId");
    setPrincipalId(id);
    fetchPrincipalDetails(id);
  }, []);

  const fetchPrincipalDetails = (principalId) => {
    setIsLoading(true);
    axios
      .get(`${API_URL}/Principal/${principalId}/details`, {
        headers: { Authorization: `Bearer ${localStorage.getItem("authToken")}` },
      })
      .then((res) => {
        setPrincipalName(res.data.principalName)
        setSchoolName(res.data.schoolName); 
        setIsLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching school details:", err);
        setIsLoading(false);
      });
  };
  const handleSeeStudents = () => {
    navigate("/students"); 
  };

  const handleSendRequest = () => {
    if (!studentName.trim()) {
      alert("Student name cannot be empty");
      return;
    }

    // Send enrollment request to the Admin
    const token =localStorage.getItem("authToken");
    const principalId = localStorage.getItem("principalId");
    const request = { StudentName:studentName };
    axios
      .post(`${API_URL}/Principal/${principalId}/enroll-student`, request,
        {
          headers:{
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then(() => {
        alert("Request sent to Admin successfully!");
        setStudentName(""); 
      })
      .catch((err) => console.error(err));
  };

  return (
    <div style={{width: '1000px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Principal Dashboard</h1>
      {principalId && <p>Your Principal ID: <strong>{principalId}</strong></p>}
      {principalName ? (
        <p>Welcome, <strong>{principalName}</strong></p>
      ) : (
        <p>Loading Principal Details...</p>
      )}
      {SchoolName ? (
        <p>Your School: <strong>{SchoolName}</strong></p>
      ) : (
        <p>Loading School Details...</p>
      )}
      
      <h2>Request to Enroll a New Student</h2>
      <input
        type="text"
        placeholder="Student Name"
        value={studentName}
        onChange={(e) => setStudentName(e.target.value)}
      />
      <button onClick={handleSendRequest}>Send Request</button>
      <button onClick={handleSeeStudents}>See Students</button>
    </div>
  );
};

export default PrincipalDashboard;
