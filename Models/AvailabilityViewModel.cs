using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USF_Accom.Models
{
    public class AvailabilityViewModel
    {
        public string StudentId { get; set; }
        [Required]
        public string CommunityName { get; set; }
        public int CommunityID { get; set; }
        [Required]
        public string AptNo { get; set; }
        [Required]
        public int Slots { get; set; }
        public string Preference { get; set; }
    }
}