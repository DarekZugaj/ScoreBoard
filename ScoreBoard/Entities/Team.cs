using ScoreBoard.Interfaces;

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
