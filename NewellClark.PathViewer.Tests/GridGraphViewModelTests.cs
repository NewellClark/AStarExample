using NewellClark.PathViewer.MvvmGridGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.Tests
{
	[TestFixture]
	public class GridGraphViewModelTests
	{
		private GridGraph graph;
		private GridGraphViewModel vm;

		private IEnumerable<T> Iterate<T>(params T[] items) => items.AsEnumerable();
		

		[SetUp]
		public void Initialize()
		{
			graph = new GridGraph();
			vm = new GridGraphViewModel(graph, new GridGraphPathFinder(graph));
		}

		[TestCase(5, 5, -13, -7, -3, -2)]
		[TestCase(5, 5, 13, -7, 2, -2)]
		[Test]
		public void ClientPointToCell_RoundsCorrectly(
			int cellSizeX, int cellSizeY, 
			int clientX, int clientY, 
			int expectCellX, int expectCellY)
		{
			vm.CellSize = new IntVector2(cellSizeX, cellSizeY);
			var client = new IntVector2(clientX, clientY);

			var expected = new IntVector2(expectCellX, expectCellY);
			var actual = vm.ClientPointToCell(client);

			Assert.AreEqual(expected, actual);
		}

		[TestCase(5, 5, -3, -2, -15, -10)]
		[TestCase(5, 8, 4, -3, 20, -24)]
		[Test]
		public void CellToClientPoint_ProducesTopLeftCorner(
			int cellSizeX, int cellSizeY,
			int cellX, int cellY,
			int expectClientX, int expectClientY)
		{
			vm.CellSize = new IntVector2(cellSizeX, cellSizeY);
			var cell = new IntVector2(cellX, cellY);

			var expected = new IntVector2(expectClientX, expectClientY);
			var actual = vm.CellToClientPoint(cell);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void LatticePoints_SinglularityRectangle()
		{
			var rect = new Rectangle(0, 0, 0, 0);

			var expected = Iterate(new IntVector2(0, 0));
			var actual = rect.LatticePoints();

			CollectionAssert.AreEquivalent(expected, actual);
		}
	}
}
