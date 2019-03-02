using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class CubeFactoryTests
    {
        [Test]
        public void GeneratingDifferentTypesTest()
        {
            //TerrainDefinition factory = new TerrainDefinition(2);

            GameObject red = new GameObject {name = "red"};

            GameObject green = new GameObject {name = "green"};

            /*factory.AddOption(CubeType.Red, 0, red);
            factory.AddOption(CubeType.Green, 5, green);

            Assert.AreEqual("red(Clone)", factory.GetCube(CubeType.Red).name);
            Assert.AreEqual("green(Clone)", factory.GetCube(CubeType.Green).name);
            
            Assert.AreEqual("red(Clone)", factory.GetCubeByTerrainHeight(2).name);
            Assert.AreEqual("green(Clone)", factory.GetCubeByTerrainHeight(5).name);
            Assert.AreEqual("green(Clone)", factory.GetCubeByTerrainHeight(25).name);*/

        }
    }
}