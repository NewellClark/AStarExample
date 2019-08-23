using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	public class SetWrapper<T> : SetBase<T>
	{
		public SetWrapper(IEqualityComparer<T> comparer) : base()
		{
			_Items = new HashSet<T>(comparer);
		}

		protected ISet<T> Items => _Items;
		private readonly HashSet<T> _Items;

		protected override bool AddItem(T item) => Items.Add(item);

		protected override bool RemoveItem(T item) => Items.Remove(item);

		protected override void ClearItems() => Items.Clear();

		protected override bool ContainsItem(T item) => Items.Contains(item);

		public override IEqualityComparer<T> Comparer => _Items.Comparer;

		public override IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

		public override int Count => Items.Count;

		public override bool IsReadOnly => Items.IsReadOnly;


	}
}
