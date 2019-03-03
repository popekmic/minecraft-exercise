using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;

public class NoiseTesting : MonoBehaviour
{
    private const string SaveFilename = "game_save.dat";
    public GameObject chunk;
    public TerrainHandler terrain;

    public GameObject targetingCube;
    public GameObject cubeSelection;

    private Vector2Int lastPosition;
    private bool isBuildModeActive;
    private CubeType selectedType = CubeType.Grey;
    private Mining mining;

    private TerrainDefinition factory;
    private TerrainGenerator terrainGen;

    private Vector3 playerStartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerStartingPosition = transform.parent.position;
        Cursor.lockState = CursorLockMode.Locked;
        factory = new TerrainDefinition();
        factory.AddOption(CubeType.Brown, 0);
        factory.AddOption(CubeType.Green, 3);
        factory.AddOption(CubeType.Grey, 15);
        factory.AddOption(CubeType.White, 20);
        terrainGen = new TerrainGenerator(factory);
        terrain = new TerrainHandler(terrainGen, chunk, 128, 48);

        lastPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);
        mining = new Mining(new Dictionary<CubeType, float>()
        {
            {CubeType.Brown, 1.5f},
            {CubeType.Green, 1},
            {CubeType.Grey, 2},
            {CubeType.White, 0.5f}
        }, new Mining.OnCubeMined(CubeMined), terrainGen);
    }

    public void SelectType(string type)
    {
        selectedType = (CubeType) Enum.Parse(typeof(CubeType), type);
        cubeSelection.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CubeMined(Vector3Int position)
    {
        terrain.AddChange(CubeType.Air, position.x, position.y, position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && !isBuildModeActive)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Vector3 cubeHitPosition = hit.point - hit.normal * 0.5f;
                    Vector3Int cubePosition = new Vector3Int(
                        Mathf.RoundToInt(cubeHitPosition.x),
                        Mathf.RoundToInt(cubeHitPosition.y),
                        Mathf.RoundToInt(cubeHitPosition.z));
                    mining.Mine(cubePosition);
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isBuildModeActive = !isBuildModeActive;
            if (isBuildModeActive)
            {
                cubeSelection.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (Input.GetButtonDown("Fire3"))
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

        if (isBuildModeActive)
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

        Vector2Int currentPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);
        if (Math.Abs(currentPosition.x - lastPosition.x) > TerrainHandler.ChunkSize)
        {
            terrain.Move(currentPosition - lastPosition);
            lastPosition.x = currentPosition.x;
        }

        if (Math.Abs(currentPosition.y - lastPosition.y) > TerrainHandler.ChunkSize)
        {
            terrain.Move(currentPosition - lastPosition);
            lastPosition.y = currentPosition.y;
        }

        if (Input.GetButtonDown("Load"))
        {
            LoadFromFile();
        }

        if (Input.GetButtonDown("Save"))
        {
            SaveToFile();
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
            /*terrain.AddChange(CubeType.Grey,
                Mathf.RoundToInt(cubeHitPosition.x),
                Mathf.RoundToInt(cubeHitPosition.y),
                Mathf.RoundToInt(cubeHitPosition.z));*/
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

    private void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + SaveFilename, FileMode.OpenOrCreate);
        bf.Serialize(file, new TerrainSerialization(terrainGen.Information));
        file.Close();
    }

    private void LoadFromFile()
    {
        string path = Application.persistentDataPath + SaveFilename;
        if (!File.Exists(path))
        {
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path, FileMode.Open);
        TerrainSerialization savedTerrain = (TerrainSerialization) bf.Deserialize(file);
        file.Close();

        transform.parent.gameObject.GetComponent<CharacterController>().enabled = false;
        transform.parent.position = playerStartingPosition;
        transform.parent.gameObject.GetComponent<CharacterController>().enabled = true;

        terrainGen = new TerrainGenerator(factory, savedTerrain.ToTerrainInfo());
        mining.Generator = terrainGen;
        terrain.Recreate(terrainGen);
    }
}