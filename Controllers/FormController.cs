using BlazorDeneme.Core;
using Microsoft.AspNetCore.Mvc;
    using MySql.Data.MySqlClient;

    namespace BlazorDeneme.Controller
    {
        [ApiController]
        [Route("api/[controller]")]
        public class FormController:ControllerBase
        {
            private readonly string _connectionString = "server=localhost;user=root;password=admin;database=blazordb";
            [HttpPost]
            [Route("formdata")]
            public IActionResult PostForm ([FromBody] PostFormData data)
            {
                string token = "";
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand("CALL sp_postform(@name,@e_mail,@age,@phone_number,@adress)",conn))
                    {
                        command.Parameters.AddWithValue("name",data.name);
                        command.Parameters.AddWithValue("e_mail",data.e_mail);
                        command.Parameters.AddWithValue("age",data.age);
                        command.Parameters.AddWithValue("phone_number",data.phone_number);
                        command.Parameters.AddWithValue("adress",data.adress);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                token = reader.GetString(0);  
                            }
                        }
                    }
                }

                return Ok(new { token = token });
            }
            [HttpGet]
            [Route("get-form-data")]
            public IActionResult GetFormData()
            {
                List<FormData> formList = new List<FormData>();
                using(MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using(MySqlCommand command = new MySqlCommand("CALL sp_get_data()",conn))
                    {
                        using(MySqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                formList.Add(new FormData(){
                                    name = reader["name"].ToString(),
                                    e_mail = reader["e_mail"].ToString(),
                                    age = Convert.ToInt32(reader["age"]),
                                    phone_number = reader["phone_number"].ToString(),
                                    adress = reader["adress"].ToString(),
                                });
                            }
                        }
                    }
                }
                return Ok(formList);
            }
        }






        public class PostFormData
        {
            public string name { get; set; }
            public string e_mail { get; set; }
            public int age { get; set; }
            public string phone_number { get; set; }
            public string adress { get; set; }
        }
    }