using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace createFourInARowDB
{
    class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserName { get; set; }
        public string HassedPassword { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Loosess { get; set; }
        public int CareerGames { get; set; }
    }
}
