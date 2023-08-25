using ScoreBoard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreBoard.Entities
{
    public class Team : ITeam
    {
        public string Name { get; set; }

        public Team(string name)
        {
            Name = name;
        }   
    }
}
