using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreBoard.Interfaces
{
    public interface IValidator
    {
        void ValidateStartMatch(ITeam homeTeam, ITeam awayTeam, IEnumerable<IMatch> matches);

        void ValidateUpdateScore(IMatch match, int homeScore, int awayScore);

        void ValidateFinishMatch(IMatch match);
    }
}
