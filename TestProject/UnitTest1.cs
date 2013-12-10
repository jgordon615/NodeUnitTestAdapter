using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SomeMethod("jojo");
        }

        private void SomeMethod(string name)
        {
            Action act = () => Assert.Fail();
            act();
        }
    }
}
