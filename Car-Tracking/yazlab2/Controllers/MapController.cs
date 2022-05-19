using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yazlab2.Models;

namespace yazlab2.Controllers
{
    public class MapController : Controller
    {
        string jsn = "./carsproject-343312-d8e6de01b9cb.json";
        string projeId;
        private FirestoreDb firestoreDb;
        public MapController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsn);
            projeId = "carsproject-343312";
            firestoreDb = FirestoreDb.Create(projeId);

        }

        public async Task<IActionResult> Index(string id)
        {
           
            string x = "2.10.2005 00:00:00";
            var time = DateTime.Parse(x);
            var time2 = DateTime.Parse(x);
            List<int> sayilar = new List<int>();
            sayilar.Add(0);
            int sayac = 0;
            List<DataModel> datalist = new List<DataModel>();
            string markers = "[";
            string[] stringArray = id.Split(',');
            int[] idler = new int[stringArray.Length];













            for (int i = 0; i < idler.Length; i++)
            {
                idler[i] = Convert.ToInt32(stringArray[i]);
            }


            for (int i = 0; i < idler.Length; i++)
            {
                CollectionReference docRef2 = firestoreDb.Collection("aracveri");
                Query query2 = docRef2.WhereEqualTo("CarId", idler[i]);
                QuerySnapshot querySnapshot2 = await query2.GetSnapshotAsync();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot2.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(dic);
                        DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                        data.Id = documentSnapshot.Id;


                        var timeData = DateTime.Parse(data.Time);

                        if(i == 0){
                            if (time < timeData)
                            {
                                time = timeData;

                            }
                        }
                        if (i == 1)
                        {
                            if (time2 < timeData)
                            {
                                time2 = timeData;

                            }
                        }





                    }
                }


                CollectionReference docRef = firestoreDb.Collection("aracveri");
                Query query = docRef.WhereEqualTo("CarId", idler[i]);
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(dic);
                        DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                        //data.Id = documentSnapshot.Id;
                       

                        var timeData = DateTime.Parse(data.Time);
                       
                        var time3 = time.AddMinutes(-30);
                        var time4 = time2.AddMinutes(-30);

                        if (i == 0)
                        {
                            if (time3 < timeData)
                            {
                                data.Id = documentSnapshot.Id;

                                datalist.Add(data);

                                markers += "{";
                                markers += string.Format("'Id': '{0}',", data.CarId);
                                markers += string.Format("'lat': '{0}',", data.Lang);
                                markers += string.Format("'lng': '{0}',", data.Lat);
                                markers += string.Format("'Time': '{0}'", data.Time);
                                markers += "},";


                                for (int j = 0; j < sayilar.Count; j++)
                                {
                                    if (sayilar[j] == data.CarId)
                                    {
                                        sayac = 0;
                                    }
                                    else
                                    {
                                        sayac = 1;
                                    }
                                }
                                if (sayac > 0)
                                {
                                    sayilar.Add(Convert.ToInt32(data.CarId));
                                }

                            }
                        }
                        if (i == 1)
                        {
                            if (time4 < timeData)
                            {
                                data.Id = documentSnapshot.Id;

                                datalist.Add(data);

                                markers += "{";
                                markers += string.Format("'Id': '{0}',", data.CarId);
                                markers += string.Format("'lat': '{0}',", data.Lang);
                                markers += string.Format("'lng': '{0}',", data.Lat);
                                markers += string.Format("'Time': '{0}'", data.Time);
                                markers += "},";


                                for (int j = 0; j < sayilar.Count; j++)
                                {
                                    if (sayilar[j] == data.CarId)
                                    {
                                        sayac = 0;
                                    }
                                    else
                                    {
                                        sayac = 1;
                                    }
                                }
                                if (sayac > 0)
                                {
                                    sayilar.Add(Convert.ToInt32(data.CarId));
                                }
                            }
                        }
                      
                         
                    }
                }
            }

            markers += "];";
            ViewBag.idler = sayilar;
            ViewBag.Markers = markers;
            sayilar.RemoveAt(0);
            return View(sayilar);

        }
        
        public async Task<IActionResult> ShowDialogAsync(int id)
        {

            string x = "2.10.2005 00:00:00";
            var time = DateTime.Parse(x);
            List<CarModel> datalist = new List<CarModel>();
            CarModel model = new CarModel();
            CollectionReference docRef = firestoreDb.Collection("aracveri");
            Query query = docRef.WhereEqualTo("CarId", id);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(dic);
                DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
        
                var timeData = DateTime.Parse(data.Time);
               

                if (time < timeData)
                {
                    time = timeData;

                }
                var time2 = time.AddHours(-1);
                model.CarId = id;
                model.Time = time2.ToString();
                model.Time2 = time.ToString();

            }

            return PartialView("_MessageShow", model);

        }


    }
}
