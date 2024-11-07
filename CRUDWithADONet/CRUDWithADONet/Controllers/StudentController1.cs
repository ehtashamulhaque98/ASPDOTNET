using CRUDWithADONet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace CRUDWithADONet.Controllers
{
    public class StudentController1 : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";

        public IActionResult Index()
        {
            var students = GetAllStudents();
            return View(students);
        }

        private List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllStudents", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    students.Add(new Student()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = (int)reader["Age"],
                        Email = reader["Email"].ToString(),
                        Department = reader["Department"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        City = reader["City"].ToString()
                    });
                }
            }
            return students;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@Department", student.Department);
                    cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    cmd.Parameters.AddWithValue("@City", student.City);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            var student = GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        public IActionResult Update(int id, Student student)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateStudent", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@Department", student.Department);
                    cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    cmd.Parameters.AddWithValue("@City", student.City);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(student);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteStudent", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private Student GetStudentById(int id)
        {
            Student student = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllStudents", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if ((int)reader["Id"] == id)
                    {
                        student = new Student
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Age = (int)reader["Age"],
                            Email = reader["Email"].ToString(),
                            Department = reader["Department"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            City = reader["City"].ToString()
                        };
                        break;
                    }
                }
            }
            return student;
        }
    }

}

