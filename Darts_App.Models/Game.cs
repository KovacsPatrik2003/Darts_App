﻿using System;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        public virtual ICollection<Player> Players { get; set; }
        public int WinnerId { get; set; }
        public int Sets { get; set; }
        public int Legs { get; set; }


        public Game()
        {
            Players=new HashSet<Player>();
        }
    }
}
