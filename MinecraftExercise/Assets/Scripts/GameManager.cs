using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameMechanics;
using Terrain;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private const string SaveFilename = "game_save.dat";

    public delegate bool IsBuildModeActive();

    public delegate void ChangeBuildModeActive();

    public GameObject chunk;
    public Vector3 playerStartingPosition;
    
    [Header("Terrain parameters")]
    public TerrainDefinition terrainDefinition;
    public int viewDistance = 128;
    public int maximumHeight = 48;
    public int groundLevel = 10;
    public int maxHeight = 35;
    
    public PlayerControls playerControls;
    public MiningBehaviour miningBehaviour;
    public BuildingBehaviour buildingBehaviour;

    private bool buildModeActive;
    private TerrainHandler terrain;
    private Vector2Int lastPosition;

    private void Awake()
    {
        lastPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);

        InputHandler inputHandler = new InputHandler();
        playerControls.input = inputHandler;

        terrainDefinition.Initialize();
        terrain = new TerrainHandler(new TerrainGenerator(terrainDefinition,groundLevel,maxHeight), chunk, viewDistance, maximumHeight);

        InitializeMining(inputHandler);
        InitializeBuilding(inputHandler);
    }

    private void InitializeBuilding(IBuildingInput inputHandler)
    {
        buildingBehaviour.input = inputHandler;
        buildingBehaviour.terrain = terrain;
        buildingBehaviour.isBuildModeActive = GetBuildModeActive;
        buildingBehaviour.changeBuildModeActive = ChangeBuildMode;
    }

    private void InitializeMining(IMiningInput inputHandler)
    {
        miningBehaviour.isBuildModeActive = GetBuildModeActive;
        miningBehaviour.input = inputHandler;
        miningBehaviour.terrainDefinition = terrainDefinition;
        miningBehaviour.terrain = terrain;
    }

    private bool GetBuildModeActive()
    {
        return buildModeActive;
    }

    private void ChangeBuildMode()
    {
        buildModeActive = !buildModeActive;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Load"))
        {
            LoadFromFile();
        }

        if (Input.GetButtonDown("Save"))
        {
            SaveToFile();
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
    }

    private void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + SaveFilename, FileMode.OpenOrCreate);
        bf.Serialize(file, new TerrainSerialization(terrain.Generator.Information));
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
        StartCoroutine(EnablePlayer());
        
        TerrainGenerator terrainGen = new TerrainGenerator(terrainDefinition, savedTerrain.ToTerrainInfo());
        terrain.Recreate(terrainGen, playerStartingPosition);

        lastPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);
    }

    private IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(5);
        transform.parent.gameObject.GetComponent<CharacterController>().enabled = true;
    }
}