using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Roulette;
using Roulette.Exceptions;

namespace UnitTest
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void SpinRoulette_GetTile_Success()
        {
            Table table = new Table();
            Tile tile = table.SpinRoulette();

            Assert.IsNotNull(tile);
        }

        [TestMethod]
        public void GetWinningTile_GetTile1_Success()
        {
            PrivateObject table = new PrivateObject(typeof(Table));
            Tile tile = (Tile) table.Invoke("GetWinningTile", 1);

            Assert.AreEqual("1", tile.Value);
        }

        [TestMethod]
        public void GetWinningTile_GetTile1_Failed()
        {
            PrivateObject table = new PrivateObject(typeof(Table));
            Tile tile = (Tile)table.Invoke("GetWinningTile", 5);

            Assert.AreNotEqual("1", tile.Value);
        }

        [TestMethod]
        public void TotalLimit_Get()
        {
            Table table = new Table();
            table.TotalLimit = 500;

            Assert.AreEqual(500, table.TotalLimit);
        }

        [TestMethod]
        public void MinimumBet_Get()
        {
            Table table = new Table();
            table.MinimumBet = 10;

            Assert.AreEqual(10, table.MinimumBet);
        }
    }
}
