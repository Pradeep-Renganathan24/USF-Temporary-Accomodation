using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USF_Accom.Models
{
    public class RequestsViewModel
    {
        public List<RequestsPosted> RequestsPosted { get; set; }
        public List<RequestsReceived> RequestsReceived { get; set; }
    }

    public class RequestsPosted
    {
        public string RequesteeID { get; set; }
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public string AptNo { get; set; }

    }

    public class RequestsReceived
    {
        //StudentName Student_Phone RequestorID AptNo CommunityName CommunityID
        public string Student_Name { get; set; }
        public string Student_Phone { get; set; }
        public string RequestorID { get; set; }
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public string AptNo { get; set; }
    }
}