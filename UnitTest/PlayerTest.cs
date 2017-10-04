using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette;
using Roulette.Bets;
using Roulette.Exceptions;

namespace UnitTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void Name_GetTestNaam_TestNaam()
        {
            Game game = new Game();
            Player player = new Player(game, 1000, "TestNaam");

            Assert.AreEqual("TestNaam", player.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(RouletteException))]
        public void PlaceBet_Enter100Bet_NotEnoughCredits()
        {
            Game game = new Game();
            Player player = new Player(game, 50, "TestNaam");
            Bet bet = new ColorBet(player, 100, "red");

            player.PlaceBet(bet);
        }

        [TestMethod]
        [ExpectedException(typeof(RouletteException))]
        public void PlaceBet_Enter5Bet_BetToLow()
        {
            Game game = new Game();
            game.StartTurn();
            game.Table.MinimumBet = 10;
            Player player = new Player(game, 1000, "TestNaam");
            Bet bet = new ColorBet(player, 5, "red");

            player.PlaceBet(bet);
        }

        [TestMethod]
        [ExpectedException(typeof(RouletteException))]
        public void PlaceBet_Enter5000Bet_TableLimitExceeded()
        {
            Game game = new Game();
            game.StartTurn();
            game.Table.TotalLimit = 500;
            Player player = new Player(game, 5000, "TestNaam");
            Bet bet = new ColorBet(player, 5000, "red");

            player.PlaceBet(bet);
        }

        [TestMethod]
        public void PlaceBet_Enter50Bet_Success()
        {
            Game game = new Game();
            game.StartTurn();
            game.Table.TotalLimit = 500;
            Player player = new Player(game, 5000, "TestNaam");
            Bet bet = new ColorBet(player, 50, "red");

            var betPlaced = player.PlaceBet(bet);

            Assert.AreEqual(true, betPlaced);
        }

        [TestMethod]
        public void AddCredits_Add50To100_ResultsIn150()
        {
            Player player = new Player(null, 100, "TestNaam");

            player.AddCredits(50);

            Assert.AreEqual(150, player.TotalCredits);
        }
    }
}
