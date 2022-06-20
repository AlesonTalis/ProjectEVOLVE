using System.Collections;
using UnityEngine;

using ProjectEVOLVE.Utils;

namespace ProjectEVOLVE.World.Regions
{
    public static class MegaRegionGen
    {
        const int MEGA_REGION_SIZE = RegionGen.REGION_SIZE * 4;
        const int MEGA_REGION_AMOUNT = 4;

        public static MegaRegion GenerateMegaRegion(Vector2Int offset)
        {
            // MegaRegion inicializador
            MegaRegion megaRegion = new MegaRegion(offset);

            // solicita MegaRegion para criar as regions
            megaRegion.CreateRegions(MEGA_REGION_AMOUNT * MEGA_REGION_AMOUNT);

            // 2D loop
            for (int y = 0, i = 0; y < MEGA_REGION_AMOUNT; y++)
            {
                for (int x = 0; x < MEGA_REGION_AMOUNT; x++, i++)
                {
                    // para cada region, executa a criação.

                    // region offset
                    Vector2Int rOffset = offset + 
                        (new Vector2Int(x, y) * RegionGen.REGION_SIZE);

                    // cria a region
                    Region region = RegionGen.GenerateBaseRegion(rOffset);

                    megaRegion.AddRegion(i, region);
                }
            }
        }
    }

    public class MegaRegion
    {
        public Vector2Int regionPosition;
        public Vector2 regionSize = Vector2.one;

        Region[] regions;

        public MegaRegion() { }

        public MegaRegion(Vector2Int regionPosition)
        {
            this.regionPosition = regionPosition;
        }

        /// <summary>
        /// inicia regions
        /// </summary>
        /// <param name="amount"></param>
        public void CreateRegions(int amount)
        {
            regions = new Region[amount];
        }
    }
}
