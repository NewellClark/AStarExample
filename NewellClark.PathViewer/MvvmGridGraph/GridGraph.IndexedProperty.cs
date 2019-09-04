using NewellClark.DataStructures.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	partial class GridGraph
	{
		/// <summary>
		/// Looks up a property in a <see cref="FlyweightCollection{IntVector2, T}"/> based
		/// on the position of the <see cref="GridNode"/> passed into the indexer.
		/// Meant to be used to allow <see cref="GridNode"/> to easily expose the property.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		internal class IndexedProperty<T>
		{
			private readonly GridGraph _graph;
			private readonly FlyweightCollection<IntVector2, T> _lookup;

			public IndexedProperty(GridGraph graph, FlyweightCollection<IntVector2, T> lookup)
			{
				Debug.Assert(graph != null);
				Debug.Assert(lookup != null);

				_graph = graph;
				_lookup = lookup;
			}
			
			public T this[GridNode node]
			{
				get
				{
					Debug.Assert(node.Graph == _graph);

					return _lookup[node.Position];
				}
				set
				{
					Debug.Assert(node.Graph == _graph);

					if (EqualityComparer<T>.Default.Equals(_lookup[node.Position], value))
						return;

					_lookup[node.Position] = value;
					_graph._NodeChanged.OnNext(node);
				}
			}
		}
	}
}
