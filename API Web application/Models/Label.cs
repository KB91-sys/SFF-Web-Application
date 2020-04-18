using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API_Web_application.Models
{
    public class Label 
    { 

        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }



    }
}
