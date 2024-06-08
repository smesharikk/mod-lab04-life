using Microsoft.VisualStudio.TestTools.UnitTesting;
using cli_life;
using System;
using System.IO;

namespace NET {
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example1.txt")));
            Assert.AreEqual(cells.Iters, 47);
        }

        [TestMethod]
        public void TestMethod2()
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example2.txt")));
            Assert.AreEqual(cells.aliveCells, 24);
        }

        [TestMethod]
        public void TestMethod3()
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example2.txt")));
            Assert.AreEqual(LifeGame.board.BoxesAmount(), 0);
        }

        [TestMethod]
        public void TestMethod4()
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example3.txt")));
            Assert.AreEqual(LifeGame.board.HivesAmount(), 1);
        }

        [TestMethod]
        public void TestMethod5()
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example3.txt")));
            Assert.AreEqual(cells.allCells - cells.aliveCells, 994);
        }
    }
}