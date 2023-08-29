using ScoreBoard.Entities;
using ScoreBoard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreBoard.Validators
{
    public class Validator : IValidator
    {
        public void ValidateFinishMatch(IMatch match)
        {
            if (match == null)
            {
                throw new ArgumentException("Provided match does not exist");
            }
        }

        public void ValidateStartMatch(ITeam homeTeam, ITeam awayTeam, IEnumerable<IMatch> matches)
        {
            if (homeTeam == null || string.IsNullOrEmpty(homeTeam.Name))
                throw new ArgumentException("Home team is incorrect");
            if (matches.Any(x => x.HomeTeam.Equals(homeTeam) || x.AwayTeam.Equals(homeTeam)))
                throw new ArgumentException("Home team is currently playing");
            if (awayTeam == null || string.IsNullOrEmpty(awayTeam.Name))
                throw new ArgumentException("Away team name is incorrect");
            if (matches.Any(x => x.HomeTeam.Equals(awayTeam) || x.AwayTeam.Equals(awayTeam)))
                throw new ArgumentException("Away team is currently playing");
            if (homeTeam.Equals(awayTeam))
                throw new ArgumentException("Team can't play against itself");
        }

        public void ValidateUpdateScore(IMatch match, int homeScore, int awayScore)
        {
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
            }
        }
    }
}
