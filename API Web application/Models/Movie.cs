using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Web_application.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public int MaxLoanAmount { get; set; }
        public int LentUnits { get; set; }

    }

    


}
