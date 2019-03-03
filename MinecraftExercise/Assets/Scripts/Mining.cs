using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Mining
    {
        public delegate void OnCubeMined(Vector3Int position);

        private readonly Dictionary<CubeType, float> toughnesses;
        public TerrainGenerator Generator { get; set; }
        private readonly OnCubeMined onCubeMined;

        private Vector3Int currentMinedPosition = Vector3Int.zero;
        private CubeType currentMinedType;
        private float miningProgress;

        public Mining(Dictionary<CubeType, float> toughnesses, OnCubeMined onCubeMined, TerrainGenerator terrainGenerator)
        {
            this.toughnesses = toughnesses;
            this.Generator = terrainGenerator;
            this.onCubeMined = onCubeMined;
        }

        public void Mine(Vector3Int position)
        {
            if (!currentMinedPosition.Equals(position))
            {
                currentMinedPosition = position;
                currentMinedType = Generator.GetCubeTypeAtPosition(position.x, position.y, position.z);
                miningProgress = 0;
            }

            miningProgress += Time.deltaTime;

            if (miningProgress > toughnesses[currentMinedType])
            {
                onCubeMined(currentMinedPosition);
            }
        }
    }
}