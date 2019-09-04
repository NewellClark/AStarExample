using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// A tool for setting the start and goal nodes by clicking with the mouse.
	/// </summary>
	class SetStartGoalTool : IDisposable
	{
		private readonly GridGraphViewModel _vm;
		private readonly IMouseProvider _mouseProvider;
		private readonly CompositeDisposable _disposables;

		public SetStartGoalTool(GridGraphViewModel vm, IMouseProvider mouseProvider)
		{
			if (vm is null) throw new ArgumentNullException(nameof(vm));
			if (mouseProvider is null) throw new ArgumentNullException(nameof(mouseProvider));

			_vm = vm;
			_mouseProvider = mouseProvider;
			_disposables = InitializeEventHandlers();
		}

		public MouseButtons SetStartNodeButton => MouseButtons.Left;

		public MouseButtons SetGoalNodeButton => MouseButtons.Right;

		private MouseButtons BothButtons => SetStartNodeButton | SetGoalNodeButton;

		private CompositeDisposable InitializeEventHandlers()
		{
			IObservable<IntVector2> whenMoveWhileDown(MouseButtons button)
			{
				return _mouseProvider.WhenMouseMove
					.Merge(_mouseProvider.WhenMouseDown)
					.Where(e => e.Button.HasFlag(button))
					.Select(e => _vm.ClientPointToCell(e.Location.ToIntVector2()));
			}

			var setStartClicks = whenMoveWhileDown(SetStartNodeButton);
			var setGoalClicks = whenMoveWhileDown(SetGoalNodeButton);

			void onSetStart(IntVector2 cellIndex) => _vm.PathFinder.StartNode = cellIndex;
			void onSetGoal(IntVector2 cellIndex) => _vm.PathFinder.GoalNode = cellIndex;

			return new CompositeDisposable
			{
				setStartClicks.Subscribe(onSetStart),
				setGoalClicks.Subscribe(onSetGoal)
			};
		}

		public void Dispose() => _disposables.Dispose();
	}
}
