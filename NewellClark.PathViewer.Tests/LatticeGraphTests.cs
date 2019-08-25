using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace NewellClark.PathViewer.Tests
{
	[TestFixture]
	public class LatticeGraphTests
	{
		private LatticeGraph graph;
		private IntVector2 vector;
		private Listener listener;

		private void AddHandler(Action<LatticeUpdateEventArgs> action)
		{
			graph.NodeUpdated += (s, e) => action(e);
		}

		private class Listener
		{
			public Listener(LatticeGraph graph)
			{
				graph.NodeUpdated += (_, e) => Args = e;
			}

			/// <summary>
			/// Gets the event args from the last captured event.
			/// </summary>
			public LatticeUpdateEventArgs Args { get; private set; }

			public bool WasRaised => Args != null;
		}

		[SetUp]
		public void Initialize()
		{
			graph = new LatticeGraph();
			vector = new IntVector2(4, -7);
			listener = new Listener(graph);
		}


		[Test]
		public void EventRaised_WhenPassableChanged()
		{
			var node = graph[vector];
			node.IsPassable = false;

			Assert.AreEqual(node, listener.Args.Node);
		}

		[Test]
		public void EventNotRaised_WhenPassableNotChanged()
		{
			var node = graph[vector];
			node.IsPassable = true;

			Assert.IsNull(listener.Args);
		}
	}
}
