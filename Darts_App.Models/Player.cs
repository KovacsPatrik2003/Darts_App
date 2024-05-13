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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        public virtual List<Game> Games { get; set; }
        public int GamesWinCount { get; set; }
        public int GamesLoseCount { get; set; }

    }
}
