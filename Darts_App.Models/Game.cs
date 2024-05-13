using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Models
{
    public class Game
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [NotMapped]
        public virtual List<Player> Players { get; set; }
        public virtual Player Winner { get; set; }
        public int WhoWon { get; set; }
        public int Sets { get; set; }
        public int Legs { get; set; }

    }
}
