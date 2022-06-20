using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectEVOLVE.Core.Data
{
    public class MeshData
    {
        const int USHORT_VALUE = 65536;

        public int[] Triangles;
        public Vector3[] Vertices;
        //public Vector3[] Normals;
        public Vector2[] UVs;
        public Color[] Colors;


        int triIndex;

        public MeshData(int size)
        {
            int tSize = (size - 1) * (size - 1) * 6;

            Triangles = new int[tSize];
            Vertices = new Vector3[size * size];
            UVs = new Vector2[size * size];
            Colors = new Color[size * size];
        }



        /// <summary>
        /// retorna a Mesh pronta
        /// </summary>
        /// <returns></returns>
        public Mesh GetMesh(string chunkId)
        {
            Mesh mesh = new Mesh();
            mesh.name = chunkId;
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.colors = Colors;
            //mesh.uv = UVs;

            mesh.RecalculateNormals();

            return mesh;
        }

        public void AddTriangle(int a, int b, int c)
        {
            Triangles[triIndex] = a;
            Triangles[triIndex + 1] = b;
            Triangles[triIndex + 2] = c;

            triIndex += 3;
        }

        public void AddVertice(Vector3 v, int vIndex, ushort biome)
        {
            int b = biome + 1;

            Vertices[vIndex] = v;
            Colors[vIndex] = new Color()
            {
                r = (float)b / 100000,
                g = 0,
                b = b == USHORT_VALUE ? 1 : 0,
                a = 0
            };
        }

    }
}
