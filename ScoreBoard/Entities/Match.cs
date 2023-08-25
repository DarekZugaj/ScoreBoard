using ScoreBoard.Interfaces;

namespace ScoreBoard.Entities
{
    public class Match : IMatch
    {
        public int Id { get; set; }
        public ITeam HomeTeam { get;set; }
        public int HomeTeamScore { get; set; }
        public ITeam AwayTeam { get; set; }
        public int AwayTeamScore { get; set; }

        public Match(int id, ITeam homeTeam, int homeTeamScore, ITeam awayTeam, int awayTeamScore)
        {
            Id = id;
            HomeTeam = homeTeam;
            HomeTeamScore = homeTeamScore;
            AwayTeam = awayTeam;
            AwayTeamScore = awayTeamScore;
        }

        public override string ToString()
        {
            return $"{HomeTeam.Name} {HomeTeamScore} - {AwayTeam.Name} {AwayTeamScore}";
        }
    }
}
