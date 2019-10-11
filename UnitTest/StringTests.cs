using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JiraFormatter.Extentions;
namespace UnitTest
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void FormatIDReturnsOnlyCharAndNumber()
        {
            string parseme = "@ <p>h78. \\ ";
            string actual = parseme.FormatID();
            string expected = "h78";
            Assert.AreEqual(expected, actual);
        }
    }
}
