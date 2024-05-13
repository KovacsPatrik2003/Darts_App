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
        public virtual Player Player { get; set; }
        [NotMapped]
        public List<Player> GuestPlayers { get; set; }
        public int WinnerId { get; set; }
        public int Sets { get; set; }
        public int Legs { get; set; }
        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

    }
}
