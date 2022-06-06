using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yazlab2.Models;
using Google.Cloud.Firestore;

namespace yazlab2.Controllers
{
    public class MapCarsController : Controller
    {
        string jsn = "yourjson";
        string projeId;
        private FirestoreDb firestoreDb;


        public MapCarsController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsn);
            projeId = "projectname";
            firestoreDb = FirestoreDb.Create(projeId);

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]





        public async Task<IActionResult> Check(CarModel carmodel)
        {
            string markers = "[";
            //int idsend = (int)TempData["idsend"];

            List<DataModel> datalist = new List<DataModel>();
            int carId = carmodel.CarId;
            string time = carmodel.Time;
            string time2 = carmodel.Time2;
          



            CollectionReference docRef = firestoreDb.Collection("aracveri");
            Query query = docRef.WhereEqualTo("CarId", carId);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(dic);
                DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                var timeData = DateTime.Parse(data.Time);
                var time3 = DateTime.Parse(time);
                var time4 = DateTime.Parse(time2);
                if (time3<=timeData && time4>=timeData)
                {
                    data.Id = documentSnapshot.Id;



                    datalist.Add(data);

                    markers += "{";

                    markers += string.Format("'Id': '{0}',", data.CarId);
                    markers += string.Format("'lat': '{0}',", data.Lang);
                    markers += string.Format("'lng': '{0}',", data.Lat);
                    markers += string.Format("'Time': '{0}'", data.Time);
                    markers += "},";

                }


            }


            markers += "];";
            ViewBag.Markers = markers;
            return View(datalist);


        }
    }
}

