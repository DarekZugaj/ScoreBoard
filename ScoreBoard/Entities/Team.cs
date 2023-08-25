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

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Team)
            {
                return false;
            }

            return (this.Name == ((Team)obj).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
