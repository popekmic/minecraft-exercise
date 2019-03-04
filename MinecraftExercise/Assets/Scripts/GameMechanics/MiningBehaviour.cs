using System.Collections.Generic;
using Terrain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics
{
    public class MiningBehaviour : MonoBehaviour
    {
        public TerrainHandler terrain;
        public TerrainDefinition terrainDefinition;
        public IMiningInput input;
        public GameManager.IsBuildModeActive isBuildModeActive;
        private Mining mining;

        private void Start()
        {
            Dictionary<CubeType, float> timesToMine = new Dictionary<CubeType, float>();
            foreach (var cube in terrainDefinition.cubes)
            {
                timesToMine.Add(cube.type, cube.timeToMine);
            }

            mining = new Mining(timesToMine, CubeMined, terrain);
        }

        private void CubeMined(Vector3Int position)
        {
            terrain.AddChange(CubeType.Air, position.x, position.y, position.z);
        }

        private void Update()
        {
            if (!input.IsMiningKeyActive() || EventSystem.current.IsPointerOverGameObject() || isBuildModeActive())
            {
                return;
            }

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, out hit))
            {
                return;
            }

            Vector3 cubeHitPosition = hit.point - hit.normal * 0.5f;
            Vector3Int cubePosition = new Vector3Int(
                Mathf.RoundToInt(cubeHitPosition.x),
                Mathf.RoundToInt(cubeHitPosition.y),
                Mathf.RoundToInt(cubeHitPosition.z));
            mining.Mine(cubePosition);
        }
    }
}