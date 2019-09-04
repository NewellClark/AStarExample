using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public struct GridNodeViewModel
	{
		private readonly GridGraphViewModel _owner;

		public GridNodeViewModel(GridGraphViewModel owner, GridNode node)
		{
			if (owner is null) throw new ArgumentNullException(nameof(owner));

			_owner = owner;
			this.Node = node;
		}

		public GridNode Node { get; }

		/// <summary>
		/// Gets the bounds of the current <see cref="GridNodeViewModel"/> in client coordinates.
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				Point topLeft = _owner.CellToClientPoint(Node.Position).ToPoint();
				Size size = _owner.CellSize.ToSize();

				return new Rectangle(topLeft, size);
			}
		}

		/// <summary>
		/// Gets the position of the center, in client coordinates.
		/// </summary>
		public IntVector2 ClientCenterPosition
		{
			get => Bounds.Center();
		}

		public Color? HighlightColor
		{
			get => _owner?.HighlightColor[this];
			set => _owner.HighlightColor[this] = value;
		}
	}
}
