using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yazlab2.Models;

namespace yazlab2.Controllers
{
    public class MessageController : Controller
    {
        string jsn = "yourjson";
        string projeId;
        private FirestoreDb firestoreDb;
        public MessageController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsn);
            projeId = "projectname";
            firestoreDb = FirestoreDb.Create(projeId);

        }

        [HttpGet]
        public IActionResult Index()
        {
            ReadExcelModel myModel = new ReadExcelModel();
            DatabaseModel myModel2 = new DatabaseModel();
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "kuyruk",
                          durable: false,
                          exclusive: false,
                          autoDelete: false,
                          arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                ReadExcelModel myModel = JsonConvert.DeserializeObject<ReadExcelModel>(message);
                myModel2.CarId = Convert.ToInt32(myModel.CarId);
                myModel2.Lang = myModel.Lang;
                myModel2.Lat = myModel.Lat;
                myModel2.Time = myModel.Time;
                CollectionReference docref = firestoreDb.Collection("aracveri");
                Query query = firestoreDb.Collection("aracveri");



                firestoreDb = FirestoreDb.Create("projectname");
                DocumentReference docRef = firestoreDb.Collection("aracveri").Document();

                await docRef.SetAsync(myModel2);


            };




            channel.BasicConsume(queue: "kuyruk", autoAck: true, consumer: consumer);

            return View();

        }
    }
}
