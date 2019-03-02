using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Experimental.UIElements;

public class NoiseTesting : MonoBehaviour
{
    public GameObject chunk;
    public TerrainHandler terrain;

    private Vector2Int lastPosition;


    // Start is called before the first frame update
    void Start()
    {
        TerrainDefinition factory = new TerrainDefinition();
        factory.AddOption(CubeType.Brown, 0);
        factory.AddOption(CubeType.Green, 3);
        factory.AddOption(CubeType.Grey, 15);
        factory.AddOption(CubeType.White, 20);

        terrain = new TerrainHandler(new TerrainGenerator(factory), chunk, 128);
        
        lastPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Vector3 cubeHitPosition = hit.point - hit.normal * 0.5f;
                //Debug.DrawLine(transform.position,hit.point,Color.red,15);
                terrain.AddChange(CubeType.Air,
                    Mathf.RoundToInt(cubeHitPosition.x),
                    Mathf.RoundToInt(cubeHitPosition.y),
                    Mathf.RoundToInt(cubeHitPosition.z));
            }
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Vector3 cubeHitPosition = hit.point + hit.normal * 0.5f;
                terrain.AddChange(CubeType.Grey,
                    Mathf.RoundToInt(cubeHitPosition.x),
                    Mathf.RoundToInt(cubeHitPosition.y),
                    Mathf.RoundToInt(cubeHitPosition.z));
            }
        }

        Vector2Int currentPosition = new Vector2Int((int) transform.position.x, (int) transform.position.z);
        if (Math.Abs(currentPosition.x - lastPosition.x) > TerrainHandler.ChunkSize)
        {
            terrain.Move(currentPosition - lastPosition);
            lastPosition.x = currentPosition.x;
        }

        if(Math.Abs(currentPosition.y - lastPosition.y) > TerrainHandler.ChunkSize)
        {
            terrain.Move(currentPosition - lastPosition);
            lastPosition.y = currentPosition.y;
        }
    }
}