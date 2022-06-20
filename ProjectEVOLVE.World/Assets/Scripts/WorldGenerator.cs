using ProjectEVOLVE.World.Data;
using ProjectEVOLVE.World.Regions;
using System.Collections;
using UnityEngine;

namespace ProjectEVOLVE.World
{ 
    public class WorldGenerator : MonoBehaviour
    {
        public static WorldGenerator Instance;
        public static WorldSettings worldSettings = Instance != null ? Instance.m_WorldSettings : null;

        private void Awake()
        {
            Instance = this;
        }


        [SerializeField]
        private WorldSettings m_WorldSettings;

        IEnumerator Start()
        {
            // mega region setup
            var megaRegion = MegaRegionGen.GenerateMegaRegion(Vector2Int.zero);
            yield return new WaitForFixedUpdate();




            yield return null;
        }
    }
}