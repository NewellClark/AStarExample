using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	class TogglablePathFinder : Toggleable
	{
		private readonly BitmapGraph _graph;
		private readonly Control _control;
		private bool _operationInProgress = false;

		public TogglablePathFinder(BitmapGraph graph, Control control)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (control is null) throw new ArgumentNullException(nameof(control));

			_graph = graph;
			_control = control;
			_graph.Start = new IntVector2(2, _graph.Bounds.Height / 2);
			_graph.Goal = new IntVector2(_graph.Bounds.Width - 3, _graph.Bounds.Height / 2);
		}

		protected override IDisposable Enable()
		{
			var graphChanged = _graph.WhenNodeChanged
				.Where(_ => AreEndPointsValid())
				.Where(node => node.IsStartOrGoal || !node.IsPath)
				.Throttle(TimeSpan.FromMilliseconds(40))
				.ObserveOn(_control);

			void onChanged(PixelNode pixelNode)
			{
				if (_operationInProgress)
					return;

				_operationInProgress = true;

				_graph.Path = AStar.FindPath(
					_graph[_graph.Start], _graph[_graph.Goal],
					knownCost(), estimatedCost(),
					CostAdder, node => node.IsPassable);

				_operationInProgress = false;
			}

			CostFunction<PixelNode, float> knownCost() => ApplyCostMultiplier(EuclidianHeuristic);

			CostFunction<PixelNode, float> estimatedCost()
			{
				return _graph.UseManhattanDistance ?
					ApplyCostMultiplier(ManhattanHeuristic) :
					ApplyCostMultiplier(EuclidianHeuristic);
			}

			return graphChanged.Subscribe(onChanged);
		}

		private CostFunction<PixelNode, float> ApplyCostMultiplier(CostFunction<PixelNode, float> costFunction)
		{
			float multiplier = _graph.UseInvertedComparison ? -1 : 1;
			return (left, right) => multiplier * costFunction(left, right);
		}

		private float ManhattanHeuristic(PixelNode left, PixelNode right)
		{
			return left.Position.ManhattanDistance(right.Position);
		}

		private float EuclidianHeuristic(PixelNode left, PixelNode right)
		{
			return left.Position.Distance(right.Position);
		}

		private float CostAdder(float left, float right) => left + right;

		private bool AreEndPointsValid()
		{
			return _graph.Bounds.Contains(_graph.Start) &&
				_graph.Bounds.Contains(_graph.Goal);
		}
	}
}
