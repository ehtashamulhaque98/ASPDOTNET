using CRUDWithADONet.DataAccessLayer;
using CRUDWithADONet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace CRUDWithADONet.Controllers
{
    //display
    public class EmployeeController1 : Controller
    {
        //private readonly string _connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";
        private readonly string _connectionString;

        public EmployeeController1(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var employees = GetAllEmployees();
            return View(employees);
        }
        private List<SelectListItem> GetEmployeeTypeList()
        {
            var employeeTypes = GetEmployeeTypes();

            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (var type in employeeTypes)
            {
                selectList.Add(new SelectListItem
                {
                    Text = type.TypeName,
                    Value = type.Id.ToString()
                });
            }
            return selectList;
        }

        private List<EmployeeType> GetEmployeeTypes()
        {
            var employeeTypes = new List<EmployeeType>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, TypeName FROM EmployeeType";

                SqlCommand cmd = new SqlCommand(query, conn);

                //SqlCommand cmd = new SqlCommand("spGetEmployeeTypes", conn);
                //cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employeeTypes.Add(new EmployeeType()
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()

                    });              
                }   
            }
            return employeeTypes;
        }


        //private List<SelectListItem> GetEmployeeTypeList()
        //{
        //    return new List<SelectListItem>()
        //        {
        //            new SelectListItem { Text = "Intern", Value = "1" },
        //            new SelectListItem { Text = "Permanent", Value = "2" },
        //            new SelectListItem { Text = "Temp", Value = "3" },
        //            new SelectListItem { Text = "Contract", Value = "4" }
        //        };
        //}

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
                        Email = reader["Email"].ToString(),
                        EmployeeType = reader["EmployeeType"].ToString(),
                        EmployeeTypeId = reader.IsDBNull(reader.GetOrdinal("EmployeeTypeId")) ? 0 : (int)reader["EmployeeTypeId"]
                    });
                }

            }
            return employees;
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.EmployeeTypeList = GetEmployeeTypeList();
            return View();

            //ViewBag.EmployeeTypeList = new List<SelectListItem>()
            //{
            //    new SelectListItem { Text = "Intern", Value = "Intern" },
            //    new SelectListItem { Text = "Permanent", Value = "Permanent" },
            //    new SelectListItem { Text = "Temp", Value = "Temp" }
            //};
            //return View();
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
                    cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
                    cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeTypeList = GetEmployeeTypeList();
            return View(employee);
            //ViewBag.EmployeeTypeList = new List<SelectListItem>()
            //{
            //     new SelectListItem { Text = "Intern", Value = "Intern" },
            //     new SelectListItem { Text = "Permanent", Value = "Permanent" },
            //     new SelectListItem { Text = "Temp", Value = "Temp" }
            //};
            //return View(employee);
        }

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var employee = GetEmployeeById(id);  
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewBag.EmployeeTypes = GetEmployeeTypeList();
        //    return View(employee);
        //}

        //[HttpPost]

        //public IActionResult Edit(Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("spUpdateEmployeess", conn);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Id",employee.Id);
        //            cmd.Parameters.AddWithValue("@Name", employee.Name);
        //            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
        //            cmd.Parameters.AddWithValue("@Age", employee.Age);
        //            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
        //            cmd.Parameters.AddWithValue("@City", employee.City);
        //            cmd.Parameters.AddWithValue("@Email", employee.Email);
        //            cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
        //            cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.EmployeeTypes = GetEmployeeTypeList();
        //    return View(employee);
        //}


        [HttpGet]
        public IActionResult Update(int id)
        {
            var employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.EmployeeTypeList = GetEmployeeTypeList();

            //ViewBag.EmployeeTypeList = new List<SelectListItem>()
            //{
            //     new SelectListItem { Text = "Intern", Value = "Intern" },
            //     new SelectListItem { Text = "Permanent", Value = "Permanent" },
            //     new SelectListItem { Text = "Temp", Value = "Temp" }
            //};

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
                    cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
                    cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeTypeList = GetEmployeeTypeList();
            //ViewBag.EmployeeTypeList = new List<SelectListItem>()
            //{

            //    new SelectListItem { Text = "Intern", Value = "Intern" },
            //    new SelectListItem { Text = "Permanent", Value = "Permanent" },
            //    new SelectListItem { Text = "Temp", Value = "Temp" }
            //};
            return View(employee);
        }




        //[HttpGet]
        //public IActionResult Update(int id)
        //{
        //    var employee = GetEmployeeById(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(employee);

        //}
        //[HttpPost]
        //public IActionResult Update(int id, Employee employee)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("spUpdateEmployeess", conn);
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Id", id);
        //            cmd.Parameters.AddWithValue("@Name", employee.Name);
        //            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
        //            cmd.Parameters.AddWithValue("@Age", employee.Age);
        //            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
        //            cmd.Parameters.AddWithValue("@City", employee.City);
        //            cmd.Parameters.AddWithValue("@Email", employee.Email);

        //            conn.Open();
        //            cmd.ExecuteNonQuery();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    return View(employee);
        //}

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
                            Email = reader["Email"].ToString(),
                            EmployeeType = reader["EmployeeType"].ToString(),
                            EmployeeTypeId = (int )reader["EmployeeTypeId"],
                        };
                        break;
                    }
                }
            }

            return employee;
        }

    }




}

