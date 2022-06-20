using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEVOLVE.Core.Data
{
    public class PointData
    {
        public int X;
        public int Y;

        public ushort height;
        public float heightData;

        public float temperature;
        public float moisture;

        public ushort biome;
        public ushort terrain;

        public ushort vegetation;
        public ushort decoration;

        /// <summary>
        /// inicia todas a variageis "negaveis"
        /// </summary>
        public PointData()
        {
            // temperatura e umidade neutras
            temperature = 0;
            moisture = 0;

            // define o bioma para 1 - planice
            biome = 1;
            terrain = 1;

            // sem vegetação ou decoração
            vegetation = 0;
            decoration = 0;
        }

        /// <summary>
        /// posição inicial
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointData(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
    }
}
