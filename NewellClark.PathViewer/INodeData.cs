using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace NewellClark.PathViewer
{
	interface INodeData
	{
		Vector2 Position { get; }

		Vector2 Size { get; }

		RectangleF Bounds { get; }
	}
}
