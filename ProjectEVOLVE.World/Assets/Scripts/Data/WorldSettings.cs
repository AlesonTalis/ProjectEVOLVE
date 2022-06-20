using System;
using UnityEngine;

namespace ProjectEVOLVE.World.Data
{
    [Serializable]
    public class WorldSettings
    {
        [HideInInspector]
        public int worldSeed => m_WorldSeed.GetHashCode();
        [SerializeField]
        private string m_WorldSeed;
    }
}
