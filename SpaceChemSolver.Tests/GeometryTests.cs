using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceChemSolver.Simulation;
using System.Collections.Generic;

namespace SpaceChemSolver.Tests
{
    [TestClass]
    public class GeometryTests
    {
        [TestMethod]
        public void PointProjectionTest()
        {
            Point r = new Point(1, 1);
            Assert.AreEqual(new Point(1, 2), r.Project(Direction.Down));
            Assert.AreEqual(new Point(1, 0), r.Project(Direction.Up));
            Assert.AreEqual(new Point(2, 1), r.Project(Direction.Right));
            Assert.AreEqual(new Point(0, 1), r.Project(Direction.Left));
        }

        [TestMethod]
        public void PointRotationTest()
        {
            Point origin;
            Point r;

            origin = new Point(2, 1);
            r = new Point(2, 0);
            Assert.AreEqual(new Point(1, 1), r.Rotate(origin, clockwise: true));

            r = new Point(1, 0);
            Assert.AreEqual(new Point(1, 2), r.Rotate(origin, clockwise: true));

            r = new Point(0, 0);
            Assert.AreEqual(new Point(1, 3), r.Rotate(origin, clockwise: true));

            r = new Point(5, 0);
            Assert.AreEqual(new Point(3, 4), r.Rotate(origin, clockwise: false));
        }

        [TestMethod]
        public void RectangleContainsTest()
        {
            var points = new List<Point>();
            points.Add(new Point(0, 0));
            points.Add(new Point(0, 3));
            points.Add(new Point(3, 3));

            var rect = new Rectangle(0, 0, 4, 4);
            Assert.IsTrue(rect.ContainsAll(points));

            rect = new Rectangle(0, 0, 3, 3);
            Assert.IsFalse(rect.ContainsAll(points));

            rect = new Rectangle(0, 0, 3, 4);
            Assert.IsFalse(rect.ContainsAll(points));

            points = new List<Point>();
            Assert.IsTrue(rect.ContainsAll(points));
        }
    }
}
