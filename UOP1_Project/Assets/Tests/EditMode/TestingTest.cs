using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditModeTestingTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestingTestSimplePasses()
        {
            // Use the Assert class to test conditions
            Assert.True(true);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestingTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            Assert.True(true);
            yield return null;
        }
    }
}
