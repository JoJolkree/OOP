using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    [TestFixture]
    class ObservableStack_should
    {
        [Test]
        public void WorkCorrectly()
        {
            var stack = new ObservableStack<int>();
            var helper = new TestHandler();
            helper.Initialize(stack);
            stack.Push(1);
            stack.Push(2);
            stack.Pop();
            Assert.AreEqual("+1+2-2", helper.GetLog());

        }
    }
}
