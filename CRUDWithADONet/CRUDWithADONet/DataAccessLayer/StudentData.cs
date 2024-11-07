using CRUDWithADONet.Models;
using System.Data.SqlClient;
using System.Data;

namespace CRUDWithADONet.DataAccessLayer
{
    public class StudentData
    {
        private readonly string _connectionString;

        public StudentData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spGetAllStudents", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
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
            }
            return students;
        }

        public void AddStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddStudent", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@Department", student.Department);
                cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                cmd.Parameters.AddWithValue("@City", student.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStudent(Student student)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateStudent", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Id", student.Id);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@Department", student.Department);
                    cmd.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                    cmd.Parameters.AddWithValue("@City", student.City);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Update failed: No rows were affected, possibly due to incorrect ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void DeleteStudent(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spDeleteStudent", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Delete failed: No rows were affected, possibly due to incorrect ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the student: " + ex.Message);
            }
        }
    }
}
