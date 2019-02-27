using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class CubeFactoryTests
    {
        [Test]
        public void GeneratingDifferentTypesTest()
        {
            CubeFactory factory = new CubeFactory(2);

            GameObject red = new GameObject {name = "red"};

            GameObject green = new GameObject {name = "green"};

            factory.AddOption(CubeType.Red, red);
            factory.AddOption(CubeType.Green, green);

            Assert.AreEqual("red(Clone)", factory.GetCube(CubeType.Red).name);
            Assert.AreEqual("green(Clone)", factory.GetCube(CubeType.Green).name);
        }
    }
}