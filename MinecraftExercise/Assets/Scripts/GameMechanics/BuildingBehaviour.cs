using System;
using System.Collections;
using Terrain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameMechanics
{
    public class BuildingBehaviour : MonoBehaviour
    {
        public GameObject targetingCube;
        public GameObject cubeSelection;
        public TerrainHandler terrain;
        public GameManager.IsBuildModeActive isBuildModeActive;
        public GameManager.ChangeBuildModeActive changeBuildModeActive;
        public IBuildingInput input;
        private CubeType selectedType = CubeType.Brown;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    
        public void SelectType(string type)
        {
            selectedType = (CubeType) Enum.Parse(typeof(CubeType), type);
            cubeSelection.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (input.IsBuildModeKeyPressed())
            {
                changeBuildModeActive();
                if (isBuildModeActive())
                {
                    cubeSelection.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                }
            }

            if (input.IsBuildCubeKeyPressed())
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (targetingCube.activeSelf)
                    {
                        terrain.AddChange(selectedType,
                            Mathf.RoundToInt(targetingCube.transform.position.x),
                            Mathf.RoundToInt(targetingCube.transform.position.y),
                            Mathf.RoundToInt(targetingCube.transform.position.z));
                    }
                }
            }
        
            if (isBuildModeActive())
            {
                StartCoroutine(UpdateTargetingCube());
            }
            else
            {
                if (targetingCube.activeSelf)
                {
                    targetingCube.SetActive(false);
                }
            }
        }
    
        private IEnumerator UpdateTargetingCube()
        {
            yield return new WaitForSeconds(0.2f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Vector3 cubeHitPosition = hit.point + hit.normal * 0.5f;
                cubeHitPosition.x = Mathf.RoundToInt(cubeHitPosition.x);
                cubeHitPosition.y = Mathf.RoundToInt(cubeHitPosition.y);
                cubeHitPosition.z = Mathf.RoundToInt(cubeHitPosition.z);

                if (!targetingCube.activeSelf)
                {
                    targetingCube.SetActive(true);
                }

                targetingCube.transform.position = cubeHitPosition;
            }
            else
            {
                if (targetingCube.activeSelf)
                {
                    targetingCube.SetActive(false);
                }
            }
        }
    }
}