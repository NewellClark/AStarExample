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
			void bindToggleableToRadioButton(Toggleable toggleable, RadioButton radioButton)
			{
				toggleable.EnabledChanged += (s, e) => radioButton.Checked = toggleable.Enabled;
				radioButton.CheckedChanged += (s, e) => toggleable.Enabled = radioButton.Checked;
			}

			var vm = gridGraphViewPanel.ViewModel;
			var mouseProvider = gridGraphViewPanel.ToMouseProvider();

			var blockUnblock = Toggleable.Create(() => new NodePassabilityTool(vm, mouseProvider));
			bindToggleableToRadioButton(blockUnblock, blockUnblockRadioButton);

			var startGoal = Toggleable.Create(() => new SetStartGoalTool(vm, mouseProvider));
			bindToggleableToRadioButton(startGoal, setStartGoalRadioButton);

			Disposed += (s, e) => _disposables.Dispose();

			return new CompositeDisposable
			{
				blockUnblock,
				startGoal
			};
		}
	}
}
