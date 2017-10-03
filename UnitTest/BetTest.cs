using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette;
using Roulette.Bets;

namespace UnitTest
{
    [TestClass]
    public class BetTest
    {
        [TestMethod]
        public void CalculatePayout_SingleBet_Success()
        {
            var amount = 10;
            Tile tile = new Tile { Color = "red", Value = "1" };
            Bet bet = new SingleBet(null, amount, tile);
            bet.Tiles.Add(tile);
            var winAmount = bet.CalculatePayout(tile);

            Assert.AreEqual(360, winAmount);
        }


        [TestMethod]
        public void CalculatePayout_SingleBet_Fail()
        {
            var amount = 10;
            Tile tileBetted = new Tile { Color = "red", Value = "1" };
            Bet bet = new SingleBet(null, amount, tileBetted);
            bet.Tiles.Add(tileBetted);

            Tile tileWon = new Tile { Color = "red", Value = "10" };
            var winAmount = bet.CalculatePayout(tileWon);

            Assert.AreEqual(0, winAmount);
        }

    }
}
