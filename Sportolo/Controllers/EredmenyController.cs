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
            connection.Open();
            var sql = @"INSERT INTO `eredmeny`(`competition`, `description`, `resulttime`, `updatetime`, `sportoloid`) VALUES (@comp,@desc,@rest,@updt,@sid);";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@comp", neweredmeny.Competition);
            cmd.Parameters.AddWithValue("@desc", neweredmeny.Description);
            cmd.Parameters.AddWithValue("@rest", DateTime.Now);
            cmd.Parameters.AddWithValue("@updt", DateTime.Now);
            cmd.Parameters.AddWithValue("@sid", neweredmeny.SportoloId);

            cmd.ExecuteNonQuery();


            connection.Close();
            return new { objectum = neweredmeny, message = "Sikeres felvétel" };
        }

        [HttpPut]

        public object updateEredmeny([FromQuery] int id, UpdateEredmenyDTO updateEredmeny)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var sql = @"UPDATE `eredmeny` SET `competition`=@comp,`description`=@desc,`updatetime`=@updtime,`sportoloid`=@spid WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@comp", updateEredmeny.Competition);
            cmd.Parameters.AddWithValue("@desc", updateEredmeny.Description);

            cmd.Parameters.AddWithValue("@updtime", DateTime.Now);
            cmd.Parameters.AddWithValue("@spid", updateEredmeny.SportoloId);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connection.Close();

            return new { message = "sikeres frissites", resoult = updateEredmeny };
        }

        [HttpDelete("id")]
        public object deleteEredmeny(int id) {

            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var sql = @"DELETE FROM `eredmeny` WHERE `id`=@id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connection.Close();

            return new { message = "sikeres törlés" };
        }
    }
}
