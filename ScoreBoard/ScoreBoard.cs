using ScoreBoard.Interfaces;

namespace ScoreBoard
{
    public class ScoreBoard
    {
        private static ScoreBoard instance;
        private static readonly object locker = new object();
        public List<IMatch> Matches { get; set; } = new List<IMatch>();
        
        public static ScoreBoard GetScoreBoard()
        {
            if (instance == null)
            {
                lock(locker)
                {
                    if (instance == null)
                    {
                        instance = new ScoreBoard();
                    }
                }
            }

            return instance;
        }
    }
}
