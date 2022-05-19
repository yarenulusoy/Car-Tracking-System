using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yazlab2.Models
{
    [FirestoreData]
    public class CarModel
    {
        public int CarId { get; set; }
        [FirestoreProperty]
        public string Time { get; set; }
        [FirestoreProperty]
        public string Time2{ get; set; }
   
    }
}
