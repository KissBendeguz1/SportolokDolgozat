using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Sportolo.Models;
using Sportolo.Models.DTOs;

namespace Sportolo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13b;";
        [HttpGet]
        public List<Eredmeny> GetSportolo()
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = "SELECT * FROM `eredmeny`";
            var cmd = new MySqlCommand(sql, connection);
            connection.Open();
            var data = cmd.ExecuteReader();
            List<Eredmeny> eredmenyek = new List<Eredmeny>();
            while (data.Read())
            {
                Eredmeny eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("competition"),
                    Description = data.GetString("description"),
                    ResultTime = data.GetDateTime("resulttime"),
                    UpdateTime = data.GetDateTime("updatetime"),
                    SportoloId = data.GetInt32("sportoloid")

                };

                eredmenyek.Add(eredmeny);
            }

            connection.Close();
            return eredmenyek;
        }

        [HttpGet("id")]
        public object GetEredmenyByID(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = @"SELECT * FROM `eredmeny` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var data = cmd.ExecuteReader();

            if (data.Read())
            {
                Eredmeny eredmeny = new Eredmeny
                {
                    Id = data.GetInt32("id"),
                    Competition = data.GetString("competition"),
                    Description = data.GetString("description"),
                    ResultTime = data.GetDateTime("resulttime"),
                    UpdateTime = data.GetDateTime("updatetime"),
                    SportoloId = data.GetInt32("sportoloid")

                };
                connection.Close();
                return eredmeny;
            }
            else {
                connection.Close();
                return ("Nincs ilyen adat"); 
            }

        }
        [HttpPost]
        public object AddNewEredmeny(AddNewEredmenyDto neweredmeny) {
            var connection = new MySqlConnection(ConnectionString);
            var sql = @"INSERT INTO `eredmeny`(`competition`, `description`, `resulttime`, `updatetime`, `sportoloid`) VALUES ('@competition','@description','@resulttime','@updatetime','@sportoloid')";
            var cmd = new MySqlCommand(sql, connection);
            connection.Open();
            cmd.Parameters.AddWithValue("@competition", neweredmeny.Competition);
            cmd.Parameters.AddWithValue("@description", neweredmeny.Description);
            cmd.Parameters.AddWithValue("@resulttime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updatetime", DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloid", neweredmeny.SportoloId);
            cmd.ExecuteNonQuery();
            connection.Close();
            return new { objektum = neweredmeny, message = "Sikeres felvetel" };
        }
    }
}
