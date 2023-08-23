namespace ScoreBoard.Interfaces
{
    public interface IScoreBoardController
    {
        int StartMatch(ITeam homeTeam, ITeam awayTeam);

        void UpdateScore(int matchId, int homeScore, int awayScore);
    }
}
