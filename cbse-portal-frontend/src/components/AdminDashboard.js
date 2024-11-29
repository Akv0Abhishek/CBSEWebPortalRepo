import React, { useState, useEffect } from "react";
import axios from "axios";
import { API_URL } from "../services/endPoints";

const AdminDashboard = () => {
  const [students, setStudents] = useState([]);
  const [studentRequests, setStudentRequests] = useState([]);
  const [selectedStudent, setSelectedStudent] = useState(null);
  const [marks, setMarks] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [isViewingMarks, setIsViewingMarks] = useState(false);
  const [error, setError] = useState(null);

  const token =localStorage.getItem("authToken");
  const handleUnauthorizedError = () => {
    setError("Unauthorized access. Please log in again.");
    localStorage.removeItem("authToken"); 
    window.location.href = "/"; 
  };
  const fetchStudentRequests = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await axios.get(
        `${API_URL}/CBSEAdmin/student-requests`,
        {
          headers: {
            Authorization: `Bearer ${token}`, // Send token in the header
          },
        }
      );
      setStudentRequests(response.data);
    } catch (err) {
      setError("Failed to fetch student requests.");
    } finally {
      setIsLoading(false);
    }
  };

  const fetchStudents = () => {
    setIsLoading(true);
    axios
      .get(`${API_URL}/CBSEAdmin/students`,{
        headers:{
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        setStudents(res.data);
        setIsLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setIsLoading(false);
      });
  };
  const handleAddStudent = async (requestId, studentName, principalId) => {
    try {
      const addStudentResponse = await axios.post(
        `${API_URL}/api/CBSEAdmin/enroll-student`,
        {
          id: requestId,
          studentName,
          principalId,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`, 
          },
        }
      );

      if (addStudentResponse.status === 200) {
        alert(`${studentName} has been successfully added.`);
        await fetchStudentRequests();
      }
    } catch (err) {
      console.error("Error adding student:", err);
      alert("Failed to add the student. Please try again.");
    }
  };

  const handleUpdateMarks = (studentId) => {
    axios
      .put(
        `${API_URL}/CBSEAdmin/${studentId}/marks`,
        marks,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': 'application/json',
          },
        }
      )
      .then(() => {
        alert("Marks updated successfully!");
        setMarks({});
        setSelectedStudent(null); //reset after update
      })
      .catch((err) => {
        if (err.response && err.response.status === 401) {
          handleUnauthorizedError();
        } else {
          console.error(err);
          alert("Failed to update marks. Please try again.");
        }
      });
  };

  const fetchStudentMarks = (studentId) => {
    axios
      .get(`${API_URL}/CBSEAdmin/student/${studentId}/marks`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        setMarks(res.data);
        setIsViewingMarks(true);
        setSelectedStudent(students.find((s) => s.id === studentId));
      })
      .catch((err) => {
        console.error(err);
        alert("Failed to fetch marks. Please try again.");
      });
  };

  useEffect(() => {
    if (token) {
      fetchStudents();
      fetchStudentRequests();
    } else {
      handleUnauthorizedError();
    }
  }, [token]);

  return (
    <div style={{width: '1000px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Admin Dashboard</h1>
      {isLoading ? (
        <p>Loading student requests...</p>
      ) : studentRequests.length === 0 ? (
        <p>No pending requests.</p>
      ) :  (
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Student Name</th>
              <th>Principal ID</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {studentRequests.map((request) => (
              <tr key={request.id}>
                <td>{request.id}</td>
                <td>{request.studentName}</td>
                <td>{request.principalId}</td>
                <td>{request.status}</td>
                <td>
                  <button
                    onClick={() =>
                      handleAddStudent(
                        request.id,
                        request.studentName,
                        request.principalId
                      )
                    }
                  >
                    Add Student
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {isLoading ? (
        <p>Loading students...</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Roll Number</th>
              <th>Name</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {students.map((student) => (
              <tr key={student.id}>
                <td>{student.roll_No}</td>
                <td>{student.name}</td>
                <td>
                  <button onClick={() => setSelectedStudent(student)}>
                    Add/Edit Marks
                  </button>
                  <button onClick={() => fetchStudentMarks(student.id)}>
                    View Marks
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {selectedStudent && !isViewingMarks && (
        <div>
          <h2>Edit/Add Marks for {selectedStudent.name}</h2>
          {["Sub1", "Sub2", "Sub3", "Sub4", "Sub5"].map((subject, index) => (
            <div key={index}>
              <label>{subject}:</label>
              <input
                type="number"
                value={marks[subject.toLowerCase()] || ""}
                onChange={(e) =>
                  setMarks({
                    ...marks,
                    [subject.toLowerCase()]: +e.target.value,
                  })
                }
              />
            </div>
          ))}
          <button onClick={() => handleUpdateMarks(selectedStudent.id)}>
            Update Marks
          </button>
          <button onClick={() => setSelectedStudent(null)}>Cancel</button>
        </div>
      )}

      {isViewingMarks && selectedStudent && (
        <div>
          <h2>Marks for {selectedStudent.name}</h2>
          <ul>
            {Object.entries(marks).map(([subject, mark], index) => (
              <li key={index}>
                {subject.toUpperCase()}: {mark}
              </li>
            ))}
          </ul>
          <button onClick={() => setIsViewingMarks(false)}>Close</button>
        </div>
      )}
    </div>
  );
};

export default AdminDashboard;
