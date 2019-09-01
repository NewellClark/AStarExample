using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace NewellClark.PathViewer
{
	static class Extensions
	{
		public static IEnumerable<Edge<TNode>> Edges<TNode>(this TNode @this)
			where TNode : IHasNeighbors<TNode>
		{
			foreach (var neighbor in @this.Neighbors)
				yield return new Edge<TNode>(@this, neighbor);
		}

		public static IEnumerable<Edge<TNode>> Edges<TNode, TCost>(this Path<TNode, TCost> @this)
		{
			if (@this is null) throw new ArgumentNullException(nameof(@this));

			using (var e = @this.GetEnumerator())
			{
				if (!e.MoveNext())
					yield break;

				TNode previous = e.Current;
				while (e.MoveNext())
				{
					yield return new Edge<TNode>(previous, e.Current);
					previous = e.Current;
				}
			}
		}

		public static IObservable<T> WhenItemAdded<T>(this ObservableSet<T> @this)
		{
			return Observable.FromEventPattern<SetChangedEventArgs<T>>(
				h => @this.SetChanged += h,
				h => @this.SetChanged -= h)
				.SelectMany(evp => evp.EventArgs.Added);
		}

		public static IObservable<T> WhenItemRemoved<T>(this ObservableSet<T> @this)
		{
			return Observable.FromEventPattern<SetChangedEventArgs<T>>(
				h => @this.SetChanged += h,
				h => @this.SetChanged -= h)
				.SelectMany(evp => evp.EventArgs.Removed);
		}

		public static IObservable<ObservableNode<T>> WhenNeighborAdded<T>(this ObservableNode<T> @this)
		{
			var result = Observable.FromEventPattern<NodeChangedEventArgs<ObservableNode<T>>>(
				h => @this.NodesAdded += h,
				h => @this.NodesAdded -= h)
				.SelectMany(evp => evp.EventArgs.Neighbors);

			return result;
		}

		public static IObservable<ObservableNode<T>> WhenNeighborRemoved<T>(this ObservableNode<T> @this)
		{
			var result = Observable.FromEventPattern<NodeChangedEventArgs<ObservableNode<T>>>(
				h => @this.NodesRemoved += h,
				h => @this.NodesRemoved -= h)
				.SelectMany(evp => evp.EventArgs.Neighbors);

			return result;
		}

		/// <summary>
		/// Gets an observable that pushes events when the current node gains an edge.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IObservable<Edge<ObservableNode<T>>> WhenEdgeAdded<T>(this ObservableNode<T> @this)
		{
			return Observable.FromEventPattern<NodeChangedEventArgs<ObservableNode<T>>>(
				h => @this.NodesAdded += h,
				h => @this.NodesAdded -= h)
					.SelectMany(evp =>
					{
						var sender = evp.EventArgs.Sender;
						return evp.EventArgs.Neighbors
							.Select(neighbor => new Edge<ObservableNode<T>>(sender, neighbor));
					});
		}

		public static IObservable<Edge<ObservableNode<T>>> WhenEdgeRemoved<T>(this ObservableNode<T> @this)
		{
			return Observable.FromEventPattern<NodeChangedEventArgs<ObservableNode<T>>>(
				h => @this.NodesRemoved += h,
				h => @this.NodesRemoved -= h)
					.SelectMany(evp =>
					{
						var sender = evp.EventArgs.Sender;
						return evp.EventArgs.Neighbors
							.Select(neighbor => new Edge<ObservableNode<T>>(sender, neighbor));
					});
		}

		public static IObservable<Edge<ObservableNode<T>>> WhenEdgeAdded<T>(this ObservableSet<ObservableNode<T>> @this)
		{
			if (@this is null) throw new ArgumentNullException(nameof(@this));


			return @this.WhenItemAdded()
				.SelectMany(node => node.WhenEdgeAdded());
		}

		public static IObservable<Edge<ObservableNode<T>>> WhenEdgeRemoved<T>(this ObservableSet<ObservableNode<T>> @this)
		{
			if (@this is null) throw new ArgumentNullException(nameof(@this));

			return @this.WhenItemAdded()
				.SelectMany(node => node.WhenEdgeRemoved());
		}
	}
}
