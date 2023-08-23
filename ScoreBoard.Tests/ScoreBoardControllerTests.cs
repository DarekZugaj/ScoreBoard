using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScoreBoard.Interfaces;
using ScoreBoard.Controllers;

namespace ScoreBoard.Tests
{
    [TestClass]
    public class ScoreBoardControllerTests
    {
        private IScoreBoardController controller;
        private Mock<ITeam> homeTeam;
        private Mock<ITeam> awayTeam;
        private ScoreBoard scoreBoard;

        [TestInitialize]
        public void Setup()
        {
            controller = new ScoreBoardController();
            homeTeam = new Mock<ITeam>();
            awayTeam = new Mock<ITeam>();
            scoreBoard = ScoreBoard.GetScoreBoard();
            
        }

        [TestMethod]
        public void StartMatch_HomeTeamEmpty_ThrowsArgumentException()
        {
            homeTeam.SetupGet(x => x.Name).Returns(string.Empty);
            awayTeam.SetupGet(x => x.Name).Returns("Poland");

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam.Object, awayTeam.Object));
        }

        [TestMethod]
        public void StartMatch_AwayTeamEmpty_ThrowsArgumentException()
        {
            homeTeam.SetupGet(x => x.Name).Returns("Poland");
            awayTeam.SetupGet(x => x.Name).Returns(string.Empty);

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam.Object, awayTeam.Object));
        }

        [TestMethod]
        public void StartMatch_BothTeamsEmpty_ThrowsArgumentException()
        {
            homeTeam.SetupGet(x => x.Name).Returns(string.Empty);
            awayTeam.SetupGet(x => x.Name).Returns(string.Empty);

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam.Object, awayTeam.Object));
        }

        [TestMethod]
        public void StartMatch_HomeTeamNull_ThrowsArgumentException()
        {
            awayTeam.SetupGet(x => x.Name).Returns("Poland");

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(null, awayTeam.Object));
        }

        [TestMethod]
        public void StartMatch_AwayTeamNull_ThrowsArgumentException()
        {
            homeTeam.SetupGet(x => x.Name).Returns("Poland");

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam.Object, null));
        }

        [TestMethod]
        public void StartMatch_BothTeamsNulls_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(null,null));
        }

        [TestMethod]
        public void StartMatch_ValidTeams_AddsMatchToScoreBoard()
        {
            scoreBoard.Matches.Clear();
            homeTeam.SetupGet(x => x.Name).Returns("Poland");
            awayTeam.SetupGet(x => x.Name).Returns("Brazil");

            controller.StartMatch(homeTeam.Object, awayTeam.Object);

            Assert.AreEqual(1, scoreBoard.Matches.Count);
        }

        [TestMethod]
        public void StartMatch_ValidTeams_AddsMatchWithValidScore()
        {
            scoreBoard.Matches.Clear();
            homeTeam.SetupGet(x => x.Name).Returns("Poland");
            awayTeam.SetupGet(x => x.Name).Returns("Brazil");

            controller.StartMatch(homeTeam.Object, awayTeam.Object);

            Assert.AreEqual(0, scoreBoard.Matches[0].HomeTeamScore);
            Assert.AreEqual(0, scoreBoard.Matches[0].AwayTeamScore);
        }

        [TestMethod]
        public void StartMatch_ValidTeams_ReturnsValidMatchId()
        {
            scoreBoard.Matches.Clear();
            homeTeam.SetupGet(x => x.Name).Returns("Poland");
            awayTeam.SetupGet(x => x.Name).Returns("Brazil");

            var matchId = controller.StartMatch(homeTeam.Object, awayTeam.Object);

            Assert.AreEqual(scoreBoard.Matches.Count, matchId);
        }
    }
}