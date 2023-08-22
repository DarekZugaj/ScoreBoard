using ScoreBoard.Interfaces;

namespace ScoreBoard
{
    public class ScoreBoard
    {
        public List<IMatch> Matches { get; set; } = new List<IMatch>();
        
        public static ScoreBoard GetScoreBoard()
        {
            throw new NotImplementedException();    
        }
    }
}
