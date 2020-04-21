using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API_Web_application.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string StudioName { get; set; }
        public string FilmTriviality { get; set; }
        public int MovieScore { get; set; }






    }



}
