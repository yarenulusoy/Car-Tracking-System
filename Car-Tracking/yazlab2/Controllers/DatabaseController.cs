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
        string jsn = "yourjson";
        string projeId;
        private FirestoreDb firestoreDb;
        public DatabaseController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsn);
            projeId = "projectname";
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
       
    }
}
