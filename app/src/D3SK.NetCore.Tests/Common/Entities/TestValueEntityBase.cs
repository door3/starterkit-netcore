using System;
using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Common.Entities;
using Xunit;

namespace D3SK.NetCore.Tests.Common.Entities
{
    public class TestValueEntityBase
    {
        public class MyTestValueEntity : ValueEntityBase
        {
            public string MyString { get; set; }
            public int MyInt { get; set; }

            public MyTestValueEntity(string myString, int myInt)
            {
                MyString = myString;
                MyInt = myInt;
            }

            public override IEnumerable<object> GetUniqueValues()
            {
                yield return MyString;
                yield return MyInt;
            }
        }

        [Fact]
        public void EqualsTest()
        {
            MyTestValueEntity nullLeft = null;
            MyTestValueEntity nullRight = null;
            var equalLeft = new MyTestValueEntity("Test Equal", 45);
            var equalRight = new MyTestValueEntity("Test Equal", 45);
            var unequalRight = new MyTestValueEntity("Unequal Right", 45);

            Assert.True(nullLeft == nullRight);
            Assert.False(nullLeft == equalRight);
            Assert.False(equalLeft == nullRight);
            Assert.False(equalLeft.Equals(null));
            Assert.True(equalLeft == equalRight);
            Assert.True(equalLeft.Equals(equalRight));
            Assert.Equal(equalLeft, equalRight);
            Assert.False(equalLeft == unequalRight);
            Assert.False(equalLeft.Equals(unequalRight));

            var equalAnonymousRight = new {MyString = "Test Equal", MyInt = 45};
            Assert.False(equalLeft.Equals(equalAnonymousRight));
        }

        [Fact]
        public void NotEqualsTest()
        {
            var equalLeft = new MyTestValueEntity("Test Not Equal", 112);
            var equalRight = new MyTestValueEntity("Test Not Equal", 112);
            var unequalRight = new MyTestValueEntity("Unequal Right", 357);

            Assert.False(equalLeft != equalRight);
            Assert.True(equalLeft != unequalRight);
            Assert.NotEqual(equalLeft, unequalRight);
        }
        
        [Fact]
        public void GetHashCodeTest()
        {
            var testHash = new MyTestValueEntity("Hash Code Test", 421);
            var equalHash = new MyTestValueEntity("Hash Code Test", 421);
            var unequalHash = new MyTestValueEntity("Unequal Hash Code", 998);
            var unequalHashItems = new object[] {"Unequal Hash Code", 998};
            var unequalHashCode = unequalHashItems.Select(x => x.GetHashCode()).Aggregate((x, y) => x ^ y);

            Assert.Equal(testHash.GetHashCode(), equalHash.GetHashCode());
            Assert.NotEqual(testHash.GetHashCode(), unequalHash.GetHashCode());
            Assert.Equal(unequalHash.GetHashCode(), unequalHashCode);
        }

        [Fact]
        public void GetUniqueValuesTest()
        {
            var equalLeft = new MyTestValueEntity("Test Unique Values", 6).GetUniqueValues().ToList();
            var equalRight = new MyTestValueEntity("Test Unique Values", 6).GetUniqueValues().ToList();
            var unequalRight = new MyTestValueEntity("Unequal Values", 7).GetUniqueValues().ToList();

            Assert.Equal(2, equalLeft.Count);
            Assert.Equal(equalLeft.Count, equalRight.Count);
            Assert.Equal(equalLeft.Count, unequalRight.Count);

            Assert.Collection(equalLeft,
                i => Assert.Equal("Test Unique Values", i),
                i => Assert.Equal(6, i));

            for (var i = 0; i < equalLeft.Count; i++)
            {
                Assert.Equal(equalLeft[i], equalRight[i]);
                Assert.NotEqual(equalLeft[i], unequalRight[i]);
            }
        }
    }
}