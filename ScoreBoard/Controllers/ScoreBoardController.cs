using ScoreBoard.Interfaces;
using Match = ScoreBoard.Entities.Match;
using System.Text;

namespace ScoreBoard.Controllers
{
    public class ScoreBoardController : IScoreBoardController
    {
        private readonly ScoreBoard scoreBoard = ScoreBoard.GetScoreBoard();
        private readonly IValidator validator;
        private static readonly object locker = new();

        public ScoreBoardController(IValidator validatorObject)
        {
            validator = validatorObject;
        }

        public int StartMatch(ITeam homeTeam, ITeam awayTeam)
        {
            validator.ValidateStartMatch(homeTeam, awayTeam, scoreBoard.Matches);
            int matchId = 0;

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
            validator.ValidateUpdateScore(match, homeScore, awayScore);

            match.HomeTeamScore = homeScore;
            match.AwayTeamScore = awayScore;
        }

        public void FinishMatch(int matchId)
        {
            var match = scoreBoard.Matches.FirstOrDefault(x => x.Id == matchId);
            validator.ValidateFinishMatch(match);

            scoreBoard.Matches.Remove(match);
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
