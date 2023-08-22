namespace ScoreBoard.Interfaces
{
    public interface IMatch
    {
        int Id { get; set; }
        ITeam HomeTeam { get; set; }    
        int HomeTeamScore { get; set; }
        ITeam AwayTeam { get; set; }    
        int AwayTeamScore { get; set; }
    }
}
