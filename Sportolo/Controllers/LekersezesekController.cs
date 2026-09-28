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

        [HttpGet("osszeseredmeny")]
        public object GetOsszesEredmeny()
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = "SELECT COUNT(`id`) AS count FROM `eredmeny`";
            var cmd = new MySqlCommand(sql, connection);
            connection.Open();
            var data = cmd.ExecuteReader();

            data.Read();
            int count = data.GetInt32("count");
            connection.Close();
            return new { Összes_eredmeny = count };




        }
        [HttpGet("eredmeny/id")]
        public object GetOsszesEredmeny(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = "SELECT COUNT(*) AS count FROM `eredmeny` WHERE `sportoloid` = @id";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            var data = cmd.ExecuteReader();

            data.Read();
            int count = data.GetInt32("count");
            connection.Close();
            return new { Sportolo_Összes_eredmeny = count};




        }

        [HttpGet("egysportolo/id")]
        public object GetNameCompDesc(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            var sql = "SELECT `sportolo`.`name`, `eredmeny`.`competition`, `eredmeny`.`description` FROM `sportolo` INNER JOIN `eredmeny` ON `sportolo`.`id` = `eredmeny`.`sportoloid` WHERE `sportolo`.`id` = @id;";
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();
            var data = cmd.ExecuteReader();


            SportoloNevCompDesc sportoloadatai = new SportoloNevCompDesc();
            List<string> Descs = new List<string>();
            List<string> Comps = new List<string>();
            

            while (data.Read())
            {

                
                Descs.Add(data.GetString("description"));

                sportoloadatai.Name = data.GetString("name");

                Comps.Add(data.GetString("competition"));
               
            }
            
            sportoloadatai.Competitions = Comps;
            sportoloadatai.Description = Descs;

            connection.Close();
            return sportoloadatai;

            
            




        }
    }
}
