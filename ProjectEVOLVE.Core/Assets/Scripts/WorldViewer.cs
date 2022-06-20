using ProjectEVOLVE.Utils;
using ProjectEVOLVE.Core.Data;
using ProjectEVOLVE.Core.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Linq;
using Stopwatch = System.Diagnostics.Stopwatch;
using UnityEditor;

namespace ProjectEVOLVE.Core
{
    public class WorldViewer : MonoBehaviour
    {
        class GenThread
        {
            public Vector2Int chunk;
            public Thread thread;
        }

        public WorldSettings worldSettings;
        public GameSettings gameSettings;

        public Transform viewPosition;
        private Vector3 viewLastPosition;

        public Vector2Int singleChunkOffsetTarget;

        ChunkData chunkData;

        List<Vector2Int> generatedChunks;
        List<Vector2Int> loadedChunks;

        Queue<ChunkData> chunkQueue;
        List<GenThread> generatingThreads;


        private void Awake()
        {
            EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    if (generatingThreads.Count > 0)
                    {
                        for (int i = 0; i < generatingThreads.Count; i++)
                        {
                            if (generatingThreads[i].thread.IsAlive) generatingThreads[i].thread.Abort();
                            Debug.Log("Stoped");
                        }
                    }
                }
            };
        }

        private void Start()
        {
            chunkQueue = new Queue<ChunkData>();
            generatedChunks = new List<Vector2Int>();
            generatingThreads = new List<GenThread>();

            viewLastPosition = new Vector3(float.MinValue, float.MaxValue, float.MinValue);
        }

        private void Update()
        {
            float dst = Vector3.Distance(viewPosition.position, viewLastPosition);

            if (dst > gameSettings.minUpdateMoveDistance)
            {
                // moveDirection
                //Vector3 dir = (viewPosition.position - viewLastPosition).normalized;

                viewLastPosition = viewPosition.position;

                int c = worldSettings.chunkSize;

                Vector2Int center = new Vector2Int()
                {
                    x = (int)(viewLastPosition.x / c) * c,
                    y = (int)(viewLastPosition.z / c) * c
                };

                StartCoroutine(GenerateChunks(center));

            }

            ShowMeshes();

            //SetVisibility();
        }



        IEnumerator GenerateChunks(Vector2Int center)
        {
            List<F.Espiral> espiralLoop = F.EspiralLoop(gameSettings.visibleChunks + 1);

            List<Vector2Int> chunksToGenerate = new List<Vector2Int>();

            for (int i = 0; i < espiralLoop.Count; i++)
            {
                Vector2Int chunk = center + espiralLoop[i].GetV2I(worldSettings.chunkSize);

                if (generatedChunks.Contains(chunk)) continue;

                generatedChunks.Add(chunk);
                chunksToGenerate.Add(chunk);

            }

            GenThread thread = new GenThread()
            {
                chunk = center
            };

            ThreadStart genThread = delegate
            {
                Stopwatch threadTime = Stopwatch.StartNew();

                for (int i = 0; i < chunksToGenerate.Count; i++)
                {

                    // gera o chunk e retorna o ChunkData
                    ChunkData chunkData = GenerateSingleChunk(chunksToGenerate[i]);

                    lock (chunkQueue)
                    {
                        chunkQueue.Enqueue(chunkData);
                    }

                }

                var gThread = generatingThreads.Where(g => g.chunk == center).FirstOrDefault();

                if (gThread != null)
                {
                    generatingThreads.Remove(gThread);
                }

                threadTime.Stop();

                Debug.Log($"Thread for center {center} ended with {threadTime.ElapsedMilliseconds}ms");
            };

            thread.thread = new Thread(genThread);

            thread.thread.Start();

            generatingThreads.Add(thread);

            Debug.Log("thread ended");

            yield return null;
        }


        public void GenerateSingleChunk()
        {
            chunkData = WorldGenerator.GenerateChunk(worldSettings, singleChunkOffsetTarget);

            chunkData.GenerateMeshes();

            Mesh[] meshes = chunkData.GetMeshes();

            Clear();
            ShowMeshes(meshes);

            //Debug.Log(chunkData.pointsData.Length);
        }

        ChunkData GenerateSingleChunk(Vector2Int offset)
        {

            ChunkData chunkData = WorldGenerator.GenerateChunk(worldSettings, offset);

            chunkData.GenerateMeshes();

            return chunkData;

        }


        //private void OnDrawGizmos()
        //{
        //    if (chunkData != null && chunkData.pointsData != null)
        //    {
        //        Gizmos.color = Color.yellow;
        //        for (int y = 0; y < chunkData.chunkSize; y++)
        //        {
        //            for (int x = 0; x < chunkData.chunkSize; x++)
        //            {
        //                Gizmos.DrawCube(new Vector3(x, chunkData.pointsData[x,y].height, y), Vector3.one * 0.75f);
        //            }
        //        }
        //    }
        //}

        void Clear()
        {
            foreach(Transform t in transform)
            {
                DestroyImmediate(t.gameObject);
            }
        }

        void ShowMeshes()
        {
            if (chunkQueue.Count > 0)
            {

                ChunkData chunk = chunkQueue.Dequeue();

                StartCoroutine(ShowMeshes(chunk));

            }
        }

        void ShowMeshes(Mesh[] meshes, Vector2Int offset = default)
        {
            for (int i = 0; i < meshes.Length; i++)
            {
                GameObject go = new GameObject($"CHUNK_{meshes[i].name}");

                go.transform.parent = transform;

                if (offset != default)
                {
                    go.transform.localPosition = new Vector3()
                    {
                        x = offset.x,
                        y = 0,
                        z = offset.y
                    };
                }

                var mr = go.AddComponent<MeshRenderer>();
                var mf = go.AddComponent<MeshFilter>();
                var mc = go.AddComponent<MeshCollider>();

                mf.mesh = meshes[i];
                mc.sharedMesh = mf.mesh;
                mr.sharedMaterial = new Material(Shader.Find("Standard"));
            }
        }

        IEnumerator ShowMeshes(ChunkData chunk)
        {

            ShowMeshes(chunk.GetMeshes(), chunk.chunkOffset);

            yield return null;

        }
    }
}