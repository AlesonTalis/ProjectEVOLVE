using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//using ProjectEVOLVE.Utils;

namespace ProjectEVOLVE.World.Regions
{
    public static class RegionGen
    {
        public const int REGION_SIZE = 2048;
        public const int REGION_RESOLUTION = 8;
        public const int REGION_AMOUNT = REGION_SIZE / REGION_RESOLUTION;

        public static Region GenerateBaseRegion(Vector2Int offset)
        {
            Region region = new Region(offset);

            for (int y = 0, i = 0; y < REGION_AMOUNT; y++)
            {
                for (int x = 0; x < REGION_AMOUNT; x++, i++)
                {
                    
                }
            }

            return region;
        }
    }



    public class Region
    {
        public string regionId;
        public Vector2Int regionPosition;
        public Vector2 regionSize = Vector2.one * RegionGen.REGION_SIZE;

        float[,] regionBaseHeight;
        float[,] regionBaseTemperature;
        float[,] regionBaseMoisture;
        int[,] regionBaseBiome;

        public Region()
        {
        }

        public Region(Vector2 regionSize)
        {
            this.regionSize = regionSize;
        }
    }
}
