using CRUDWithADONet.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace CRUDWithADONet.DataAccessLayer
{
    public class EmployeeData
    {
        //display
        //private string connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";

        //public List<Employee> GetAllEmployees()
        //{
        //    List<Employee> employees = new List<Employee>();

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("spGetAllEmployeess", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Employee emp = new Employee()
        //            {
        //                Id = Convert.ToInt32(reader["Id"]),
        //                Name = reader["Name"].ToString(),
        //                Gender = reader["Gender"].ToString(),
        //                City = reader["City"].ToString(),
        //                Salary = Convert.ToDecimal(reader["Salary"]),
        //                Email = reader["Email"].ToString()
        //            };
        //            employees.Add(emp);
        //        }
        //    }
        //    return employees;

        //}second code

        private string connectionString = "Data Source=DESKTOP-BRCMGPK\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;Encrypt=False";

        public List<EmployeeType> GetEmployeeTypes()
        {
            List<EmployeeType> employeeTypes = new List<EmployeeType>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id, TypeName FROM EmployeeTypes", con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employeeTypes.Add(new EmployeeType
                    {
                        Id = (int)reader["Id"],
                        TypeName = reader["TypeName"].ToString()
                    });
                }
            }
            return employeeTypes;
        }


        private readonly string _connectionString;


        public EmployeeData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("spGetAllEmployeess", connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
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
                            EmployeeTypeId = (int)reader["EmployeeTypeId"]

                        });
                    }
                }
            }
            return employees;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployeess", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Age", employee.Age);
                cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                cmd.Parameters.AddWithValue("@City", employee.City);
                cmd.Parameters.AddWithValue("@Email", employee.Email);
                cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
                cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spUpdateEmployeess", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Id", employee.Id);
                    cmd.Parameters.AddWithValue("@Name", employee.Name);
                    cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                    cmd.Parameters.AddWithValue("@Age", employee.Age);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@City", employee.City);
                    cmd.Parameters.AddWithValue("@Email", employee.Email);
                    cmd.Parameters.AddWithValue("@EmployeeType", employee.EmployeeType);
                    cmd.Parameters.AddWithValue("@EmployeeTypeId", employee.EmployeeTypeId);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Update failed: No rows were affected, possibly due to incorrect ID.");
                    }

                    //cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public void DeleteEmployee(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {

                    SqlCommand cmd = new SqlCommand("spDeleteEmployeess", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("Delete failed: No rows were affected, possibly due to incorrect ID.");
                    }
                    //cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while deleting the employee: " + ex.Message);
            }










        }
    }
}

