using ProjectEVOLVE.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace ProjectEVOLVE.Core.Data
{
    public class ChunkData 
    {
        public string chunkId;
        public Vector2Int chunkOffset;
        public int chunkSize;
        int chunkLength;

        public PointData[,] pointsData;

        public List<MeshData> meshData;


        // loop
        internal int loopIndex;
        internal int loopX;
        internal int loopY;


        public ChunkData()
        {
        }

        public ChunkData(int chunkSize, Vector2Int chunkOffset)
        {
            this.chunkSize = chunkSize;
            this.chunkOffset = chunkOffset;
            chunkLength = (chunkSize - 1) * (chunkSize - 1);

            pointsData = new PointData[chunkSize, chunkSize];

            chunkId = F.GetNewGUID();
        }


        public void GenerateMeshes()
        {
            InitLoop();
            //ushort lBiome = 0;
            //Dictionary<ushort, MeshData> dicMeshDatas = new Dictionary<ushort, MeshData>();
            var meshData = new MeshData(chunkSize);

            Stopwatch t = Stopwatch.StartNew();

            int[,] vertexIndicesMap = new int[chunkSize, chunkSize];
            int meshVertexIndex = 0;

            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    vertexIndicesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }
            }
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {

                    int vertexIndex = vertexIndicesMap[x, y];

                    Vector3 percent = new Vector2()
                    {
                        x = x / (float)chunkSize,
                        y = y / (float)chunkSize,
                    };

                    Vector3 vertexPosition = new Vector3()
                    {
                        x = percent.x * (chunkSize),
                        y = pointsData[x, y].heightData * 64 + 32,
                        z = percent.y * (chunkSize)
                    };

                    meshData.AddVertice(vertexPosition, vertexIndex, pointsData[x, y].biome);

                    if (x < chunkSize - 1 && y < chunkSize - 1)
                    {

                        int a = vertexIndicesMap[x, y];
                        int b = vertexIndicesMap[x + 1, y];
                        int c = vertexIndicesMap[x, y + 1];
                        int d = vertexIndicesMap[x + 1, y + 1];

                        //meshData.AddTriangle(a, d, c);
                        //meshData.AddTriangle(d, a, b);
                        meshData.AddTriangle(c, d, a);
                        meshData.AddTriangle(b, a, d);

                    }

                }
            }

            t.Stop();

            Debug.Log($"Time passed: {t.ElapsedMilliseconds}ms");

            this.meshData = new List<MeshData>()
            {
                meshData
            };
        }

        public Mesh[] GetMeshes() => meshData.Select(m => m.GetMesh(chunkId)).ToArray();


        /// <summary>
        /// inicia a interação de loop, resetando seus valores
        /// </summary>
        public void InitLoop()
        {
            loopIndex = 0;
            loopX = 0;
            loopY = 0;
        }

        /// <summary>
        /// interação de loop padrão
        /// </summary>
        /// <returns></returns>
        public bool Loop(int dX = 0, int dY = 0)
        {
            loopIndex++;
            loopX++;
            if (loopX >= chunkSize - (1 + dX))
            {
                loopX = 0;

                loopY++;
                if (loopY >= chunkSize - (1 + dY))
                {
                    loopY = 0;
                    return false;
                }
            }

            return true;
        }

        public class MeshD
        {
            public Mesh mesh;
        }
    }
}