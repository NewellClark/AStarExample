using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Disposables;
using NewellClark.DataStructures.Graphs;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public partial class GridDisplayPanelUC : UserControl
	{
		private readonly CompositeDisposable _disposables;

		public GridDisplayPanelUC()
		{
			InitializeComponent();

			_disposables = InitiializeEventHandlers();
		}

		private CompositeDisposable InitiializeEventHandlers()
		{
			var vm = gridGraphViewPanel.ViewModel;
			var mouseProvider = gridGraphViewPanel.ToMouseProvider();

			var blockUnblock = Toggleable.Create(() => new NodePassabilityTool(vm, mouseProvider), "Block/unblock node");
			blockUnblock.Enabled = true;
			toolSelectorBox.AddToggleable(blockUnblock);

			var startGoal = Toggleable.Create(() => new SetStartGoalTool(vm, mouseProvider), "Set start/goal node");
			toolSelectorBox.AddToggleable(startGoal);

			void pathFinder_HeuristicChanged(object sender, EventArgs e)
			{
				heuristicSelectorBox.Heuristic = vm.PathFinder.Heuristic;
			}
			void heuristicSelector_HeuristicChanged(object sender, EventArgs e)
			{
				vm.PathFinder.Heuristic = heuristicSelectorBox.Heuristic;
			}
			vm.PathFinder.HeuristicChanged += pathFinder_HeuristicChanged;
			heuristicSelectorBox.HeuristicChanged += heuristicSelector_HeuristicChanged;

			void graph_AllowedMovementsChanged(object sender, EventArgs e)
			{
				allowedMovementSelectorBox.Value = vm.Graph.AllowedMovements;
			}
			void allowedMovementSelectorBox_ValueChanged(object sender, EventArgs e)
			{
				vm.Graph.AllowedMovements = allowedMovementSelectorBox.Value;
			}
			vm.Graph.AllowedMovementsChanged += graph_AllowedMovementsChanged;
			allowedMovementSelectorBox.ValueChanged += allowedMovementSelectorBox_ValueChanged;

			InitializePathDisplayerEventHandlers();

			Disposed += HandleDisposed;

			return new CompositeDisposable
			{
				blockUnblock,
				startGoal
			};
		}

		private void InitializePathDisplayerEventHandlers()
		{
			var vm = gridGraphViewPanel.ViewModel;
			var finder = vm.PathFinder;
			var display = pathInfoDisplayer;
			
			void onPathChanged(object sender, EventArgs e)
			{
				bool startGoalNotNull = finder.StartNode != null && finder.GoalNode != null;
				bool pathNotEmpty = !finder.ShortestPath.IsEmpty;

				display.EuclidianDistance = startGoalNotNull && pathNotEmpty ?
					finder.StartNode.Value.Distance(finder.GoalNode.Value) : (float?)null;

				display.ManhattanDistance = startGoalNotNull && pathNotEmpty ?
					finder.StartNode.Value.ManhattanDistance(finder.GoalNode.Value) : (int?)null;

				display.NodeCount = pathNotEmpty ? finder.ShortestPath.Nodes.Count() : (int?)null;

				display.PathCost = pathNotEmpty ? finder.ShortestPath.Cost : (float?)null;
			}

			finder.ShortestPathChanged += onPathChanged;
		}

		private void HandleDisposed(object sender, EventArgs e)
		{
			_disposables.Dispose();
		}

		private void GridDisplayPanelUC_Load(object sender, EventArgs e)
		{
			var vm = gridGraphViewPanel.ViewModel;
			IntVector2 topLeft = vm.ViewBounds.Location.ToIntVector2();
			IntVector2 topRight = topLeft + vm.ViewBounds.Width * IntVector2.UnitX;
			IntVector2 middleOffset = IntVector2.UnitY * vm.ViewBounds.Height / 2;

			vm.PathFinder.StartNode = vm.ClientPointToCell(topLeft + middleOffset) + IntVector2.UnitX;
			vm.PathFinder.GoalNode = vm.ClientPointToCell(topRight + middleOffset) - IntVector2.UnitX;
		}
	}
}
