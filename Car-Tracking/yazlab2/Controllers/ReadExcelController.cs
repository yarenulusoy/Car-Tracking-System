using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yazlab2.Models;


namespace yazlab2.Controllers
{
    public class ReadExcelController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            List<ReadExcelModel> users = new List<ReadExcelModel>();
 
            var fileName = "./car2.xlsx";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read()) 
                    {
                        users.Add(new ReadExcelModel
                        {

                            Time = reader.GetValue(0).ToString(),
                            Lang = reader.GetValue(1).ToString(),
                            Lat = reader.GetValue(2).ToString(),
                            CarId = reader.GetValue(3).ToString()

                        });


                        var factory = new ConnectionFactory()
                        {
                            HostName = "localhost",
                            UserName = "guest",
                            Password = "guest",
                        };


                        using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue: "kuyruk",
                                                 durable: false,
                                                 exclusive: false,
                                                 autoDelete: false,
                                                 arguments: null);
                            int say = users.Count;

                            var message = JsonConvert.SerializeObject(users[say-1]);
                            var body = Encoding.UTF8.GetBytes(message);

                            channel.BasicPublish(exchange: "",
                                                 routingKey: "kuyruk",
                                                 basicProperties: null,
                                                 body: body);
                        }




                    }
                }
            }



            return View(users);
        }
    }

}