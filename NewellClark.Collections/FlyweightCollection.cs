using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// Allows values to be looked up by keys. If the specified key is not in 
	/// the collection, the default value is returned. If a key's value is set to the specified 
	/// default value, then the key is removed from the collection.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public class FlyweightCollection<TKey, TValue> 
	{
		private readonly Dictionary<TKey, TValue> _lookup;

		/// <summary>
		/// Creates a new <see cref="FlyweightCollection{TKey, TValue}"/> with the specified default value.
		/// </summary>
		/// <param name="default">The value that will be returned if a key is not in the collection.</param>
		public FlyweightCollection(TValue @default)
		{
			this.Default = @default;
			_lookup = new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// The value that will be returned if the specified key hasn't been added.
		/// </summary>
		public TValue Default { get; }

		/// <summary>
		/// Gets or sets the value for the specified key. If the specified key does not exist,
		/// the default value is returned.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public TValue this[TKey key]
		{
			get
			{
				return _lookup.TryGetValue(key, out TValue result) ?
					result : Default;
			}

			set
			{
				if (ValueComparer.Equals(value, Default))
					_lookup.Remove(key);
				else
					_lookup[key] = value;
			}
		}

		private static IEqualityComparer<TValue> ValueComparer { get; } = EqualityComparer<TValue>.Default;
	}
}
