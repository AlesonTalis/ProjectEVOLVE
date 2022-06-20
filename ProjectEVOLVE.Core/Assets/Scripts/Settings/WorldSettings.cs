using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rand = System.Random;
using Newtonsoft.Json;

namespace ProjectEVOLVE.Core.Settings
{
    [Serializable]
    public class WorldSettings
    {
        public enum WorldType
        {
            DebugWorld,
            FlatWorld,
            NormalWorld,
            SingleBiomeWorld,
            LargeBiomesWorld
        }

        [SerializeField]
        private string worldSeed;
        public float worldScale;
        public int WorldSeed { get { return worldSeed.GetHashCode(); } }

        public int chunkSize = 128;
        public int regionSize = 64;

        //public string worldSettings = @"{""generationSettings"":{""worldType"":""flat"",""worldTypeData"":{""biome"":""plains"",""elevation"":64,""decoration"":null}}}";// dictionaries with string and a object

        public BiomeSettings[] biomeSettings;

        private List<SeedSettings> seedDatas = new List<SeedSettings>();

        // hidden configs
        public WorldType worldType = WorldType.FlatWorld;
        
        [Header("Flat World settings")]
        public string flatWorldBiome = "plains";
        public ushort flatWorldElevation = 64;

        [Header("Normal World settings")]
        public string normalWorldAceptedBiomes = "all";
        public string normalWorldDeniedBiomes = "none";
        public float normalWorldScale = 1.0f;

        [Header("Debug World settings")]
        public ushort debugWorldElevation = 8;
        public Material debugWorldMaterial;

        public void Validate(int seed)
        {
            Rand rand = new Rand(seed);

            for (int i = 0; i < biomeSettings.Length; i++)
            {
                // inicia as instancias do fnoise
                biomeSettings[i].noiseSettings.Init(rand.Next(), rand.Next());
            }

            // load world settings from worldSettings
        }
    }
}
