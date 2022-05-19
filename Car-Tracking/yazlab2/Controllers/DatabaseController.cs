using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using yazlab2.Models;
using Newtonsoft.Json;

namespace yazlab2.Controllers
{
    public class DatabaseController : Controller
    {
        string jsn = "./carsproject-343312-d8e6de01b9cb.json";
        string projeId;
        private FirestoreDb firestoreDb;
        public DatabaseController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsn);
            projeId = "carsproject-343312";
            firestoreDb = FirestoreDb.Create(projeId);

        }
        public async Task<IActionResult> Index()
        {
            Query query = firestoreDb.Collection("aracveri");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            List<DataModel> datalist = new List<DataModel>();
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(dic);
                    DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                    datalist.Add(data);

                }
            }
            return View(datalist);
        }

        [HttpGet]
        public IActionResult Getir()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Getir(DataModel model)
        {
            CollectionReference collectionReference = firestoreDb.Collection("aracveri");
            await collectionReference.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }
        /*
        public async Task<List<DataModel>> VeriGetir()
        {

            try
            {
                Query query = firestoreDb.Collection("aracverileri");
                QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
                List<DataModel> datalist = new List<DataModel>();

                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> dic = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(dic);
                        DataModel data = JsonConvert.DeserializeObject<DataModel>(json);
                        data.Id = documentSnapshot.Id;
                        datalist.Add(data);
                    }
                }
                return datalist;

            }
            catch (Exception)
            {
                throw;
            }





        }
    
        /*
        public async Task<List<DataModel>> VeriEkle()
        {
            CollectionReference docref = firestoreDb.Collection("aracverileri");
            Query query = firestoreDb.Collection("aracverileri");
          

            DataModel model = new DataModel
            {
                Id = "20",
                Time = "10",
                Lang="55"

            };

            firestoreDb = FirestoreDb.Create("carsproject-343312");
            DocumentReference docRef = firestoreDb.Collection("aracverileri").Document("" + model.Id+ "");

            await docRef.SetAsync(model);
     
        }*/
    }
}
