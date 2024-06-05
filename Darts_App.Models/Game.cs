using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Darts_App.Models
{
    public class Game
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Player> Players { get; set; }
        public int WinnerId { get; set; }
        //public int? Sets { get; set; }
        //public int? Legs { get; set; }
        public int? StartPoints { get; set; }
        public string Check_Out { get; set; }
        [NotMapped]
        public List<int> Sets { get; set; }
        [NotMapped]
        public List<int> Legs { get; set; }
        public int SetCount { get; set; }
        public int LegCount { get; set; }


        public Game()
        {
            Players=new HashSet<Player>();
            Sets = new List<int>();
            Legs = new List<int>();
        }
    }
}
