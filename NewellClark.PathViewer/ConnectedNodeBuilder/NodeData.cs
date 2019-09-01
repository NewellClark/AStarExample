using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace NewellClark.PathViewer
{
	struct NodeData 
	{
		public NodeData(Vector2 position)
		{
			this.Position = position;
		}

		public Vector2 Position { get; }

		public Vector2 Size => 40 * Vector2.One;

		public RectangleF Bounds => Geometry.CenteredRectangle(Position, Size);

		public Circle Circle
		{
			get
			{
				float radius = Math.Max(Size.X, Size.Y) / 2;
				return new Circle(Position, radius / 2);
			}
		}
	}
}
