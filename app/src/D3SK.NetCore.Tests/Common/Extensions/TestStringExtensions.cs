using D3SK.NetCore.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace D3SK.NetCore.Tests.Common.Extensions
{
    public class TestStringExtensions
    {
        [Fact]
        public void QuoteEncodeTest()
        {
            const string testString = "This is a test string!";

            // test double quotes
            var doubleQuotedString = testString.QuoteEncode(true);
            Assert.Equal($"\"{testString}\"", doubleQuotedString);

            // test single quotes
            var singleQuotedString = testString.QuoteEncode(false);
            Assert.Equal($"'{testString}'", singleQuotedString);
        }

        [Fact]
        public void StripTest()
        {
            const string normalString = "This is a normal string.";
            const string extraWhiteSpaceString = "   This string has extra white space     ";
            const string allWhiteSpaceString = "     \t   ";
            const string testDefaultString = "Test Default String";
            const string nullString = null;

            // test normal string
            var normalStringResult = normalString.Strip();
            Assert.Equal(normalString, normalStringResult);

            // test extra whitespace string
            var extraWhiteSpaceStringResult = extraWhiteSpaceString.Strip();
            Assert.Equal(extraWhiteSpaceString.Trim(), extraWhiteSpaceStringResult);

            // test all whitespace string, default = null
            var allWhiteSpaceStringNullResult = allWhiteSpaceString.Strip();
            Assert.Null(allWhiteSpaceStringNullResult);

            // test all whitespace string, default = "Test Default String"
            var allWhiteSpaceStringDefaultResult = allWhiteSpaceString.Strip(testDefaultString);
            Assert.Equal(testDefaultString, allWhiteSpaceStringDefaultResult);

            // test null
            var nullStringResult = nullString.Strip();
            Assert.Null(nullStringResult);
        }
    }
}
