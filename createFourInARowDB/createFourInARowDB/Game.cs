using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace createFourInARowDB
{
    class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public string Winner { get; set; }
        public int WinnerPoint { get; set; }
    }
}
