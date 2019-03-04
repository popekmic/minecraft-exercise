using System.Collections.Generic;
using UnityEngine;

namespace Terrain
{
    public class MeshGenerator
    {
        public delegate CubeType GetChunkType(int x, int y, int z);
        
        private const float TextureUnit = 0.5f;

        private readonly Dictionary<CubeType, Vector2> materialCoordinates = new Dictionary<CubeType, Vector2>()
        {
            {CubeType.Brown, new Vector2(0,0)},
            {CubeType.Green, new Vector2(0,1)},
            {CubeType.Grey, new Vector2(1,1)},
            {CubeType.White, new Vector2(1,0)},
        };

        public List<Vector3> vertices { get; private set; }
        public List<int> triangles { get; private set; }
        public List<Vector2> uvs { get; private set; }
        private int facesCount = 0;
        private Vector2 currentTextureCoords;

        public MeshGenerator()
        {
            this.materialCoordinates = materialCoordinates;
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uvs = new List<Vector2>();
        }

        public void Reset()
        {
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            facesCount = 0;
        }

        public void GenerateCubeMesh(GetChunkType getChunkType, int x, int y, int z)
        {
            CubeType type = getChunkType(x, y, z);
            if (type.Equals(CubeType.Air))
            {
                return;
            }

            currentTextureCoords = materialCoordinates[type];

            if (getChunkType(x, y + 1, z).Equals(CubeType.Air))
            {
                GenerateTopFace(x, y, z);
                facesCount++;
            }

            if (getChunkType(x, y, z + 1).Equals(CubeType.Air))
            {
                GenerateFrontFace(x, y, z);
                facesCount++;
            }

            if (getChunkType(x - 1, y, z).Equals(CubeType.Air))
            {
                GenerateRightFace(x, y, z);
                facesCount++;
            }

            if (getChunkType(x, y, z - 1).Equals(CubeType.Air))
            {
                GenerateBackFace(x, y, z);
                facesCount++;
            }

            if (getChunkType(x + 1, y, z).Equals(CubeType.Air))
            {
                GenerateLeftFace(x, y, z);
                facesCount++;
            }

            if (getChunkType(x, y - 1, z).Equals(CubeType.Air))
            {
                GenerateBottomFace(x, y, z);
                facesCount++;
            }
        }

        private void GenerateTopFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateBottomFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateFrontFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateBackFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateLeftFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateRightFace(int x, int y, int z)
        {
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
            vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

            GenerateTriangle();
            GenerateUVs();
        }

        private void GenerateTriangle()
        {
            triangles.Add(facesCount * 4);
            triangles.Add(facesCount * 4 + 1);
            triangles.Add(facesCount * 4 + 2);
            triangles.Add(facesCount * 4);
            triangles.Add(facesCount * 4 + 2);
            triangles.Add(facesCount * 4 + 3);
        }

        private void GenerateUVs()
        {
            uvs.Add(new Vector2(TextureUnit * currentTextureCoords.x + TextureUnit,
                TextureUnit * currentTextureCoords.y));
            uvs.Add(new Vector2(TextureUnit * currentTextureCoords.x + TextureUnit,
                TextureUnit * currentTextureCoords.y + TextureUnit));
            uvs.Add(new Vector2(TextureUnit * currentTextureCoords.x,
                TextureUnit * currentTextureCoords.y + TextureUnit));
            uvs.Add(new Vector2(TextureUnit * currentTextureCoords.x, TextureUnit * currentTextureCoords.y));
        }
    }
}