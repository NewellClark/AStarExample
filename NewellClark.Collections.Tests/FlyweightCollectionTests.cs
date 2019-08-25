using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Collections.Tests
{
	[TestFixture]
	public class FlyweightCollectionTests
	{
		private FlyweightCollection<object, int> Create(int @default)
		{
			return new FlyweightCollection<object, int>(@default);
		}

		[Test]
		public void MissingKey_YieldsDefaultValue()
		{
			const int @default = -79;
			var fly = Create(@default);
			var key = new object();

			Assert.AreEqual(fly[key], @default);
		}

		[Test]
		public void AddedKey_YieldsAddedValue()
		{
			const int @default = 1032;
			const int nonDefault = 51;
			var fly = Create(@default);
			var key = new object();

			fly[key] = nonDefault;

			Assert.AreEqual(nonDefault, fly[key]);
		}
	}
}
