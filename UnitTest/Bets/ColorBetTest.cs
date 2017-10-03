using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette;
using Roulette.Bets;

namespace UnitTest.Bets
{
    [TestClass]
    public class ColorBetTest
    {
        [TestMethod]
        public void CreateColorBet_AddAllRedTiles_18TilesAdded()
        {
            var amount = 10;
            Game game = new Game();
            Player player = new Player(game, 500, "Test");
            Bet bet = new ColorBet(player, amount, "red");

            Assert.AreEqual(18, bet.Tiles.Count);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Bet bet = new ColorBet(null, 0, "red");

            Assert.AreEqual("color bet on red", bet.ToString());
        }
    }
}
