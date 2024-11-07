using CRUDWithADONet.DataAccessLayer;
using CRUDWithADONet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace CRUDWithADONet.Controllers
{
    //display
    public class EmployeeController1 : Controller
    {
        private readonly string _connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";


        public IActionResult Index()
        {
            var employees = GetAllEmployees();
            return View(employees);
        }

        private List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();



            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployeess", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Age = (int)reader["Age"],
                        Salary = Convert.ToDecimal(reader["Salary"]),
                        City = reader["City"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }

            }
            return employees;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddEmployeess", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Age", employee.Age);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);

        }
        [HttpPost]
        public IActionResult Update(int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateEmployeess", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Age", employee.Age);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteEmployeess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");

        }

        private Employee GetEmployeeById(int id)
        {
            Employee employee = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllEmployeess", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if ((int)reader["Id"] == id)
                    {
                        employee = new Employee
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Age = (int)reader["Age"],
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            City = reader["City"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                        break;
                    }
                }
            }

            return employee;
        }
    }




}

