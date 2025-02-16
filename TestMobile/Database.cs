using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MobileServer.DataBase
{

    class Database
    {
        public static void Reset()
        {
            using var db = new ApplicationContext(true);
            db.SaveChanges();
        }

        public static void Add(Item item)
        {
            MethodPatch(item);

            
        }

        static async void MethodPatch(Item item)
        {
            await Task.Run(() =>
            {


                HttpClient client = new HttpClient();


                string data = JsonConvert.SerializeObject(item);

                HttpContent content = new StringContent(data); // данные для отправки


                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                try
                {

                    HttpResponseMessage response = client.PatchAsync("http://77.50.192.67:3306/Data", content).Result; // отправляем запрос

                    if (response.StatusCode.ToString() == "OK")
                    {

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });

        }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; } = String.Empty;
        public double Latitude { get; set; } = 0;
        public double Longitude { get; set; } = 0;
        public string CodeDot { get; set; }
        public string SaveTime { get; set; }
    }

    public sealed class ApplicationContext : DbContext
    {
        private string _connectoinString = "server=77.50.192.67:3306;uid=user;pwd=1590;database=kbp;";
        public DbSet<Item> Items { get; set; } = null!;

        public ApplicationContext(bool reset = false)
        {
            if (reset)
                Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseMySql(_connectoinString, new MySqlServerVersion(new Version(8, 0, 20)));
    }

    class Ghokc
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
}
