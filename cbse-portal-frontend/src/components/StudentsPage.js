import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from 'react-router-dom';
import { API_URL } from "../services/endPoints";

const StudentsPage = () => {
  const [students, setStudents] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    fetchStudents();
  }, []);

  const fetchStudents = () => {
    setIsLoading(true);
    const principalId = localStorage.getItem("principalId");
    axios
      .get(`${API_URL}/Principal/${principalId}/students`, {
        headers: { Authorization: `Bearer ${localStorage.getItem("authToken")}` },
      })
      .then((res) => {
        console.log(res.data);
        setStudents(res.data);
        setIsLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching students:", err);
        setIsLoading(false);
      });
  };
  const handleViewMarks = (studentId) =>{
    navigate(`/marks/${studentId}`);
  };
  return (
    <div style={{width: '500px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Students List</h1>
      {isLoading ? (
        <p>Loading students...</p>
      ) : students.length > 0 ? (
        <table>
          <thead>
            <tr>
              <th>Roll_No</th>
              <th>Name</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
          {students.map((student) => (
            <tr key={student.id}>
                <td>{student.roll_No}</td>
                <td>{student.name}</td>
                <td>
                  <button onClick={() => handleViewMarks(student.id)}>View Marks</button>
                </td>
            </tr>
          ))}
          </tbody>
        </table>
      ) : (
        <p>No students found.</p>
      )}
    </div>
  );
};

export default StudentsPage;
