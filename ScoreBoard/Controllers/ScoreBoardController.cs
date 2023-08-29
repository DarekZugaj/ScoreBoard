using ScoreBoard.Interfaces;
using Match = ScoreBoard.Entities.Match;
using System.Text;

namespace ScoreBoard.Controllers
{
    public class ScoreBoardController : IScoreBoardController
    {
        private ScoreBoard scoreBoard = ScoreBoard.GetScoreBoard();
        private static readonly object locker = new();

        public int StartMatch(ITeam homeTeam, ITeam awayTeam)
        {
            int matchId = 0;
            if (homeTeam == null || string.IsNullOrEmpty(homeTeam.Name) || scoreBoard.Matches.Any(x => x.HomeTeam.Equals(homeTeam) || x.AwayTeam.Equals(homeTeam)))
                throw new ArgumentException("Home team is incorrect");
            if (awayTeam == null || string.IsNullOrEmpty(awayTeam.Name) || scoreBoard.Matches.Any(x => x.HomeTeam.Equals(awayTeam) || x.AwayTeam.Equals(awayTeam)))
                throw new ArgumentException("Away team name is incorrect");
            if (homeTeam.Equals(awayTeam))
                throw new ArgumentException("Team can't play against itself");

            lock (locker)
            {
                matchId = scoreBoard.Matches.Count + 1;
                scoreBoard.Matches.Add(new Match(matchId, homeTeam, 0, awayTeam, 0));
            }

            return matchId;
        }

        public void UpdateScore(int matchId, int homeScore, int awayScore)
        {
            var match = scoreBoard.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
            {
                throw new ArgumentException("Provided match does not exist");
            }
            else
            {
                if (homeScore < match.HomeTeamScore || homeScore > match.HomeTeamScore + 1) //score can't decrease and increase by more than 1
                    throw new ArgumentException("Provided home team score is incorrect");
                if (awayScore < match.AwayTeamScore || awayScore > match.AwayTeamScore + 1)//score can't decrease and increase by more than 1
                    throw new ArgumentException("Provided away team score is incorrect");
                if (match.HomeTeamScore != homeScore && match.AwayTeamScore != awayScore)
                    throw new ArgumentException("Both teams can't score at the same time");

                match.HomeTeamScore = homeScore;
                match.AwayTeamScore = awayScore;    
            }
        }

        public void FinishMatch(int matchId)
        {
            var match = scoreBoard.Matches.FirstOrDefault(x => x.Id == matchId);

            if (match == null)
            {
                throw new ArgumentException("Provided match does not exist");
            }
            else
            {
                scoreBoard.Matches.Remove(match);
            }
        }

        public string GetSummary()
        {
            var summaryBuilder = new StringBuilder();
            var orderedBoard = scoreBoard.Matches.OrderByDescending(x => x.HomeTeamScore + x.AwayTeamScore).ThenByDescending(x => x.Id);
              
            for(int i = 0; i < orderedBoard.Count(); i++)
            {
                summaryBuilder.AppendLine($"{i+1}. {orderedBoard.ElementAt(i).ToString()}");
            }

            return summaryBuilder.ToString();
        }
    }
}
