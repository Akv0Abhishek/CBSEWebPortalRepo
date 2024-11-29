import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import { API_URL } from "../services/endPoints";

const MarksPage = () => {
  const { studentId } = useParams();
  const [marks, setMarks] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    fetchMarks(studentId);
  }, [studentId]);

  const fetchMarks = async(studentId) => {
    setIsLoading(true);
    axios
      .get(`${API_URL}/Principal/student/${studentId}/marks`, {
        headers: { Authorization: `Bearer ${localStorage.getItem("authToken")}` },
      })
      .then((res) => {
        setMarks(res.data);
        setIsLoading(false);
      })
      .catch((err) => {
        console.error("Error fetching marks:", err);
        setIsLoading(false);
      });
  };

  return (
    <div style={{width: '500px', backgroundColor: '#b2b6f3', marginTop: '150px', borderRadius: '10px'}} className='p-3 mx-auto'>
      <h1>Student Marks</h1>
      {isLoading ? (
        <p>Loading marks...</p>
      ) : marks ? (
        <div>
          <h3>Marks for Student ID: {studentId}</h3>
          <ul>
            <li>Subject 1: {marks.sub1}</li>
            <li>Subject 2: {marks.sub2}</li>
            <li>Subject 3: {marks.sub3}</li>
            <li>Subject 4: {marks.sub4}</li>
            <li>Subject 5: {marks.sub5}</li>
          </ul>
        </div>
      ) : (
        <p>No marks found for this student.</p>
      )}
    </div>
  );
};

export default MarksPage;
