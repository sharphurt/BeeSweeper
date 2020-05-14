using System.Collections.Generic;
using System.Drawing;
using BeeSweeper.Architecture;
using BeeSweeper.model;
using NUnit.Framework;

namespace BeeSweeper.Tests
{
    [TestFixture]
    public class BeeSweeperTest
    {
        private static Field CreateFieldWithCustomMines(Size size, IEnumerable<Point> mines)
        {
            var field = new Field(size, 0);
            foreach (var mine in mines)
                field[mine.X, mine.Y].CellType = CellType.Bee;
            MapCreator.CountNeighbours(field);
            return field;
        }

        [Test]
        public void ChangeAttrTest()
        {
            var size = new Size(3, 3);
            var model = new GameModel(new Level("", size, 0));
            var checkedAttrs = new[] {CellAttr.Flagged, CellAttr.Questioned, CellAttr.None};
            var testedPoint = new Point(1, 1);
            foreach (var attr in checkedAttrs)
            {
                model.ChangeAttr(testedPoint);
                Assert.AreEqual(model.Field[testedPoint].CellAttr, attr);
            }
        }

        [Test]
        public void FinishGameOnEmptyFieldTest()
        {
            var model = new GameModel(new Level("", new Size(3, 3), 0));
            model.OpenCell(new Point(0, 0));
            Assert.IsTrue(model.GameOver);
            Assert.AreEqual(Winner.Player, model.Winner);
        }

        [Test]
        public void FinishGameSuccessfullyTest()
        {
            var size = new Size(5, 5);
            var model = new GameModel(new Level("", size, 4));
            var customMines = new[] {new Point(2, 2)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(0, 4));
            Assert.IsTrue(model.GameOver);
            Assert.AreEqual(Winner.Player, model.Winner);
        }

        [Test]
        public void GameFailedTest()
        {
            var size = new Size(5, 5);
            var model = new GameModel(new Level("", size, 4));
            var customMines = new[] {new Point(2, 2)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(2, 2));
            Assert.IsTrue(model.GameOver);
            Assert.AreEqual(Winner.Computer, model.Winner);
        }

        [Test]
        public void NeighboursCountingCorrectnessTest()
        {
            var size = new Size(3, 3);
            var model = new GameModel(new Level("", size, 0));
            var customMines = new[] {new Point(2, 1), new Point(1, 0)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            var fieldCellsShouldBe = new Dictionary<Point, int>
            {
                {new Point(0, 0), 1},
                {new Point(0, 1), 1},
                {new Point(0, 2), 0},
                {new Point(1, 0), -1},
                {new Point(1, 1), 2},
                {new Point(1, 2), 0},
                {new Point(2, 0), 2},
                {new Point(2, 1), -1},
                {new Point(2, 2), 1}
            };
            foreach (var cell in fieldCellsShouldBe)
            {
                var cellLoc = cell.Key;
                Assert.AreEqual(cell.Value, model.Field[cellLoc.X, cellLoc.Y].BeesAround);
            }
        }

        [Test]
        public void OpenEmptyTerritoryTest1()
        {
            var size = new Size(3, 3);
            var model = new GameModel(new Level("", size, 0));
            var customMines = new[] {new Point(1, 0), new Point(1, 1), new Point(2, 2)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(0, 2));
            var fieldCellsShouldBe = new Dictionary<Point, CellAttr>
            {
                {new Point(0, 0), CellAttr.None},
                {new Point(0, 1), CellAttr.Opened},
                {new Point(0, 2), CellAttr.Opened},
                {new Point(1, 0), CellAttr.None},
                {new Point(1, 1), CellAttr.None},
                {new Point(1, 2), CellAttr.Opened},
                {new Point(2, 0), CellAttr.None},
                {new Point(2, 1), CellAttr.None},
                {new Point(2, 2), CellAttr.None}
            };
            foreach (var cell in fieldCellsShouldBe)
            {
                var cellLoc = cell.Key;
                Assert.AreEqual(cell.Value, model.Field[cellLoc.X, cellLoc.Y].CellAttr);
            }
        }

        [Test]
        public void OpenEmptyTerritoryTest2()
        {
            var size = new Size(3, 3);
            var model = new GameModel(new Level("", size, 0));
            var customMines = new[] {new Point(2, 0), new Point(2, 1), new Point(2, 2)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(0, 2));
            var fieldCellsShouldBe = new Dictionary<Point, CellAttr>
            {
                {new Point(0, 0), CellAttr.Opened},
                {new Point(0, 1), CellAttr.Opened},
                {new Point(0, 2), CellAttr.Opened},
                {new Point(1, 0), CellAttr.Opened},
                {new Point(1, 1), CellAttr.Opened},
                {new Point(1, 2), CellAttr.Opened},
                {new Point(2, 0), CellAttr.None},
                {new Point(2, 1), CellAttr.None},
                {new Point(2, 2), CellAttr.None}
            };
            foreach (var cell in fieldCellsShouldBe)
            {
                var cellLoc = cell.Key;
                Assert.AreEqual(cell.Value, model.Field[cellLoc.X, cellLoc.Y].CellAttr);
            }
        }

        [Test]
        public void ScoreTest1()
        {
            var size = new Size(5, 5);
            var model = new GameModel(new Level("", size, 0));
            var customMines = new[] {new Point(2, 2)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(0, 0));
            Assert.AreEqual(6, model.Score);
        }

        [Test]
        public void ScoreTest2()
        {
            var size = new Size(5, 5);
            var model = new GameModel(new Level("", size, 0));
            var customMines = new[] {new Point(2, 2), new Point(2, 3)};
            model.Field.Map = CreateFieldWithCustomMines(size, customMines).Map;
            model.OpenCell(new Point(0, 0));
            Assert.AreEqual(10, model.Score);
        }
    }
}