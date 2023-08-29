using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScoreBoard.Interfaces;
using ScoreBoard.Controllers;
using Match = ScoreBoard.Entities.Match;

namespace ScoreBoard.Tests
{
    [TestClass]
    public class ScoreBoardControllerTests
    {
        private IScoreBoardController controller;
        private Mock<ITeam> homeTeam1;
        private Mock<ITeam> homeTeam2;
        private Mock<ITeam> homeTeam3;
        private Mock<ITeam> awayTeam1;
        private Mock<ITeam> awayTeam2;
        private Mock<ITeam> awayTeam3;
        private ScoreBoard scoreBoard;

        [TestInitialize]
        public void Setup()
        {
            controller = new ScoreBoardController();

            homeTeam1 = new Mock<ITeam>();
            homeTeam1.SetupGet(x => x.Name).Returns("Argentina");

            homeTeam2 = new Mock<ITeam>();
            homeTeam2.SetupGet(x => x.Name).Returns("Belgium");

            homeTeam3 = new Mock<ITeam>();
            homeTeam3.SetupGet(x => x.Name).Returns("Chile");

            awayTeam1 = new Mock<ITeam>();
            awayTeam1.SetupGet(x => x.Name).Returns("Australia");

            awayTeam2 = new Mock<ITeam>();
            awayTeam2.SetupGet(x => x.Name).Returns("Brazil");

            awayTeam3 = new Mock<ITeam>();
            awayTeam3.SetupGet(x => x.Name).Returns("China");

            scoreBoard = ScoreBoard.GetScoreBoard();
        }

        [TestMethod]
        public void StartMatch_HomeTeamEmpty_ThrowsArgumentException()
        {
            homeTeam1.SetupGet(x => x.Name).Returns(string.Empty);

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, awayTeam1.Object));
        }

        [TestMethod]
        public void StartMatch_AwayTeamEmpty_ThrowsArgumentException()
        {
            awayTeam1.SetupGet(x => x.Name).Returns(string.Empty);

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, awayTeam1.Object));
        }

        [TestMethod]
        public void StartMatch_BothTeamsEmpty_ThrowsArgumentException()
        {
            homeTeam1.SetupGet(x => x.Name).Returns(string.Empty);
            awayTeam1.SetupGet(x => x.Name).Returns(string.Empty);

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, awayTeam1.Object));
        }

        [TestMethod]
        public void StartMatch_HomeTeamNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(null, awayTeam1.Object));
        }

        [TestMethod]
        public void StartMatch_AwayTeamNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, null));
        }

        [TestMethod]
        public void StartMatch_BothTeamsNulls_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(null, null));
        }

        [TestMethod]
        public void StartMatch_ValidTeams_AddsMatchToScoreBoard()
        {
            scoreBoard.Matches.Clear();

            controller.StartMatch(homeTeam1.Object, awayTeam1.Object);

            Assert.AreEqual(1, scoreBoard.Matches.Count);
        }

        [TestMethod]
        public void StartMatch_ValidTeams_AddsMatchWithValidScore()
        {
            scoreBoard.Matches.Clear();

            controller.StartMatch(homeTeam1.Object, awayTeam1.Object);

            Assert.AreEqual(0, scoreBoard.Matches[0].HomeTeamScore);
            Assert.AreEqual(0, scoreBoard.Matches[0].AwayTeamScore);
        }

        [TestMethod]
        public void StartMatch_ValidTeams_ReturnsValidMatchId()
        {
            scoreBoard.Matches.Clear();

            var matchId = controller.StartMatch(homeTeam1.Object, awayTeam1.Object);

            Assert.AreEqual(scoreBoard.Matches.Count, matchId);
        }

        [TestMethod]
        public void StartMatch_HomeTeamAlreadyPlaying_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.HomeTeam == homeTeam1.Object && x.AwayTeam == awayTeam1.Object));

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, awayTeam2.Object));
        }

        [TestMethod]
        public void StartMatch_AwayTeamAlreadyPlaying_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.HomeTeam == homeTeam1.Object && x.AwayTeam == awayTeam1.Object));

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam2.Object, awayTeam1.Object));
        }

        [TestMethod]
        public void StartMatch_HomeAndAwayTeamsAreSame_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();

            Assert.ThrowsException<ArgumentException>(() => controller.StartMatch(homeTeam1.Object, homeTeam1.Object));
        }

        [TestMethod]
        public void UpdateScore_MatchIdDoesNotExist_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 1, 0));
        }

        [TestMethod]
        public void UpdateScore_HomeScoreInvalid_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, -1, 0));
        }

        [TestMethod]
        public void UpdateScore_AwayScoreInvalid_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 0, -1));
        }

        [TestMethod]
        public void UpdateScore_HomeScoreCantDiffByMoreThan1_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 3, 1));
        }

        [TestMethod]
        public void UpdateScore_AwayScoreCantDiffByMoreThan1_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 1, 3));
        }

        [TestMethod]
        public void UpdateScore_HomeScoreCantDecrease_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 0, 1));
        }

        [TestMethod]
        public void UpdateScore_AwayScoreCantDecrease_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 1, 0));
        }

        [TestMethod]
        public void UpdateScore_BothTeamsCantScoreAtTheSameTime_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            Assert.ThrowsException<ArgumentException>(() => controller.UpdateScore(1, 2, 2));
        }

        [TestMethod]
        public void UpdateScore_HomeTeamScored_UpdatesScoreBoard()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            controller.UpdateScore(1, 2, 1);

            Assert.AreEqual(2, scoreBoard.Matches.First(x => x.Id == 1).HomeTeamScore);
        }

        [TestMethod]
        public void UpdateScore_AwayTeamScored_UpdatesScoreBoard()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            controller.UpdateScore(1, 1, 2);

            Assert.AreEqual(2, scoreBoard.Matches.First(x => x.Id == 1).AwayTeamScore);
        }

        [TestMethod]
        public void UpdateScore_NoChangeInScore_UpdatesScoreBoard()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1 && x.HomeTeamScore == 1 && x.AwayTeamScore == 1));

            controller.UpdateScore(1, 1, 1);

            Assert.AreEqual(1, scoreBoard.Matches.First(x => x.Id == 1).HomeTeamScore);
            Assert.AreEqual(1, scoreBoard.Matches.First(x => x.Id == 1).AwayTeamScore);
        }

        [TestMethod]
        public void FinishMatch_MatchIdDoesNotExist_ThrowsArgumentException()
        {
            scoreBoard.Matches.Clear();

            Assert.ThrowsException<ArgumentException>(() => controller.FinishMatch(1));
        }

        [TestMethod]
        public void FinishMatch_ValidMatchIdOnlyOneMatchOnBoard_RemovesMatchFromBoard()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1));

            controller.FinishMatch(1);

            Assert.AreEqual(0, scoreBoard.Matches.Count);
        }

        [TestMethod]
        public void FinishMatch_ValidMatchIdMultipleMatchesOnBoard_RemovesMatchFromBoard()
        {
            scoreBoard.Matches.Clear();
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 1));
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 2));
            scoreBoard.Matches.Add(Mock.Of<IMatch>(x => x.Id == 3));

            controller.FinishMatch(2);

            Assert.AreEqual(2, scoreBoard.Matches.Count);
        }

        [TestMethod]
        public void GetSummary_EmptyBoard_ReturnsEmptyString()
        {
            scoreBoard.Matches.Clear();

            Assert.AreEqual(string.Empty, controller.GetSummary());
        }

        [TestMethod]
        public void GetSummary_DifferentTotalScores_ReturnsValidOrder()
        {
            scoreBoard.Matches.Clear();

            scoreBoard.Matches.Add(new Match(1, homeTeam1.Object, 1, awayTeam1.Object, 1));
            scoreBoard.Matches.Add(new Match(2, homeTeam2.Object, 2, awayTeam2.Object, 2));
            scoreBoard.Matches.Add(new Match(3, homeTeam3.Object, 3, awayTeam3.Object, 3));

            var output = controller.GetSummary();
            var expectedOutput = $"1. {homeTeam3.Object.Name} 3 - {awayTeam3.Object.Name} 3{Environment.NewLine}" +
                                 $"2. {homeTeam2.Object.Name} 2 - {awayTeam2.Object.Name} 2{Environment.NewLine}" +
                                 $"3. {homeTeam1.Object.Name} 1 - {awayTeam1.Object.Name} 1{Environment.NewLine}";

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void GetSummary_SameTotalScoresOnly_ReturnsValidOrder()
        {
            scoreBoard.Matches.Clear();

            scoreBoard.Matches.Add(new Match(1, homeTeam1.Object, 1, awayTeam1.Object, 1));
            scoreBoard.Matches.Add(new Match(2, homeTeam2.Object, 1, awayTeam2.Object, 1));
            scoreBoard.Matches.Add(new Match(3, homeTeam3.Object, 1, awayTeam3.Object, 1));

            var output = controller.GetSummary();
            var expectedOutput = $"1. {homeTeam3.Object.Name} 1 - {awayTeam3.Object.Name} 1{Environment.NewLine}" +
                                 $"2. {homeTeam2.Object.Name} 1 - {awayTeam2.Object.Name} 1{Environment.NewLine}" +
                                 $"3. {homeTeam1.Object.Name} 1 - {awayTeam1.Object.Name} 1{Environment.NewLine}";

            Assert.AreEqual(expectedOutput, output);
        }

        [TestMethod]
        public void GetSummary_SameAndDifferentTotalScores_ReturnsValidOrder()
        {
            scoreBoard.Matches.Clear();

            scoreBoard.Matches.Add(new Match(1, homeTeam1.Object, 1, awayTeam1.Object, 1));
            scoreBoard.Matches.Add(new Match(2, homeTeam2.Object, 0, awayTeam2.Object, 3));
            scoreBoard.Matches.Add(new Match(3, homeTeam3.Object, 1, awayTeam3.Object, 1));

            var output = controller.GetSummary();
            var expectedOutput = $"1. {homeTeam2.Object.Name} 0 - {awayTeam2.Object.Name} 3{Environment.NewLine}" +
                                 $"2. {homeTeam3.Object.Name} 1 - {awayTeam3.Object.Name} 1{Environment.NewLine}" +
                                 $"3. {homeTeam1.Object.Name} 1 - {awayTeam1.Object.Name} 1{Environment.NewLine}";

            Assert.AreEqual(expectedOutput, output);
        }

        [TestCleanup]
        public void Cleanup()
        {
            scoreBoard.Matches.Clear();
        }


    }
}