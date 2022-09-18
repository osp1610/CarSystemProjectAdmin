using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEndApp.Models
{
    public class CarDetails
    {
        public string CarNo { get; set; }
        public string CarImage { get; set; }
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string CarYear { get; set; }
        public string CarType { get; set; }
        public Nullable<int> NoOfOwners { get; set; }
        public Nullable<bool> CarVerified { get; set; }
        public Nullable<bool> CarSold { get; set; }
        public Nullable<int> CarUid { get; set; }
        public string City { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }

    }
}