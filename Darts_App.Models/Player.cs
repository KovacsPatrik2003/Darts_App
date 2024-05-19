using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darts_App.Models
{
    public class Player
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(240)]
        public string Name { get; set; }
        [Required]
        [StringLength(240)]
        public string Password { get; set; }
        [NotMapped]
        public virtual ICollection<Game> Games { get; set; }
        public int GamesWinCount { get; set; }
        public int GamesLoseCount { get; set; }
        [NotMapped]
        public int CurrentPoints { get; set; }

        public Player()
        {
            Games=new HashSet<Game>();
        }

    }
}
