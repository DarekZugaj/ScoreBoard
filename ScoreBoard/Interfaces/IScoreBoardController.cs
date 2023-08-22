namespace ScoreBoard.Interfaces
{
    public interface IScoreBoardController
    {
        int StartMatch(ITeam homeTeam, ITeam awayTeam);
    }
}
