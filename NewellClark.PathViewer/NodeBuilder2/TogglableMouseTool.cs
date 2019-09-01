using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.NodeBuilder2
{
	abstract class TogglableMouseTool : TogglableWidget
	{
		protected TogglableMouseTool(IMouseProvider mouseProvider, MouseButtons mouseButton)
		{
			if (mouseProvider is null) throw new ArgumentNullException(nameof(mouseProvider));

			this.MouseProvider = mouseProvider;
			this.MouseButton = mouseButton;
		}

		protected IMouseProvider MouseProvider { get; }

		public virtual string Name => "Tool";

		public virtual string Tooltip => "No tooltip available";

		public MouseButtons MouseButton
		{
			get => _MouseButton;
			set
			{
				if (_MouseButton == value)
					return;

				_MouseButton = value;
				OnMouseButtonChanged(EventArgs.Empty);
			}
		}
		private MouseButtons _MouseButton;

		public event EventHandler MouseButtonChanged;

		protected virtual void OnMouseButtonChanged(EventArgs e)
		{
			MouseButtonChanged?.Invoke(this, e);
		}
	}
}
