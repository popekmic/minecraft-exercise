using System;
using System.Collections.Generic;

namespace Terrain
{
    [Serializable]
    public class TerrainDefinition
    {
        public CubeDefinition[] cubes;

        private SortedDictionary<int, CubeType> heightTypes = new SortedDictionary<int, CubeType>();

        public void Initialize()
        {
            foreach (var cubeDefinition in cubes)
            {
                heightTypes[cubeDefinition.heightInTerrain] = cubeDefinition.type;
            }
        }

        public CubeType GetCubeByTerrainHeight(int height)
        {
            CubeType resultType = CubeType.Air;

            foreach (var key in heightTypes.Keys)
            {
                if (height >= key)
                {
                    resultType = heightTypes[key];
                }
            }

            return resultType;
        }

        [Serializable]
        public class CubeDefinition
        {
            public CubeType type;
            public int heightInTerrain;
            public float timeToMine;
        }
    }
}