using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestFixture]
    public class NUnitTest1
    {
        [Test]
        public void NTestMethod1()
        {
            SomeMethod();
        }


        private void SomeMethod()
        {
            Assert.Fail();
        }
    }
}
