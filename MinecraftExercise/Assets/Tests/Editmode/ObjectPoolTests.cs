using NUnit.Framework;
using UnityEngine;
using Utilities;


namespace Tests
{
    public class ObjectPoolTests
    {
        private ObjectPool objectPool;

        [SetUp]
        public void Before()
        {
            objectPool = new ObjectPool(new GameObject(), 2);
        }

        [Test]
        public void RecycleTest()
        {
            GameObject firstObject = objectPool.GetObject();
            GameObject secondObject = objectPool.GetObject();

            objectPool.ReturnObject(firstObject);
            GameObject nextObject = objectPool.GetObject();

            Assert.AreSame(firstObject, nextObject);

            objectPool.ReturnObject(secondObject);
            nextObject = objectPool.GetObject();

            Assert.AreSame(secondObject, nextObject);
        }
    }
}