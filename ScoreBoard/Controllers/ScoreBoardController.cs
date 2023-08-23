using ScoreBoard.Interfaces;
using System.Text.RegularExpressions;
using ScoreBoard.Entities;
using Match = ScoreBoard.Entities.Match;

namespace ScoreBoard.Controllers
{
    public class ScoreBoardController : IScoreBoardController
    {
        private ScoreBoard scoreBoard = ScoreBoard.GetScoreBoard();
        private static readonly object locker = new object();
        public int StartMatch(ITeam homeTeam, ITeam awayTeam)
        {
            int matchId = 0;
            if (homeTeam == null || string.IsNullOrEmpty(homeTeam.Name))
                throw new ArgumentException("Home team is incorrect");
            if (awayTeam == null || string.IsNullOrEmpty(awayTeam.Name))
                throw new ArgumentException("Away team name is incorrect");

            lock (locker)
            {
                matchId = scoreBoard.Matches.Count + 1;
                scoreBoard.Matches.Add(new Match(matchId, homeTeam, 0, awayTeam, 0));
            }

            return matchId;
        }
    }
}
