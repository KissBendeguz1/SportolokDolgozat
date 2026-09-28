using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo.Models;
using System.ComponentModel.DataAnnotations;

namespace Sportolo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LekersezesekController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13b;";

        [HttpGet("id")]
        public object GetNameEmiailByID(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = "SELECT `name`,`email` FROM `sportolo` WHERE `id`=@id";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var data = cmd.ExecuteReader();
            
            if (data.Read())
            {
                NameEmail eredmeny = new NameEmail
                {
                    Email = data.GetString("email"),
                    Name = data.GetString("name"),
                };
                connection.Close();
                return eredmeny;
            } else {
                connection.Close();
                return new { msg = "Nincs ilyen sportolo" };
            }
            
                

            
        
        }
    }
}
