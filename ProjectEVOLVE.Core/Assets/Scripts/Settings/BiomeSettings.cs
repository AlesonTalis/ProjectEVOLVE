using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectEVOLVE.Core.Settings
{
    [Serializable]
    public class BiomeSettings
    {
        public string biomeName;
        public ushort biomeId;

        // settings
        public Vector2 temperatureRange;
        public Vector2 moistureRange;

        public float heightMultiplier;
        public AnimationCurve heightMultiplierEvaluate;

        public string[] decorationsSettings;

        // FastNoise Settings
        public BiomeSettingsNoise noiseSettings;

        public float ValidateElevation(float x, float y)
        {
            float wx = x;
            float wy = y;

            if (noiseSettings.useWarpMode) noiseSettings.fNoiseWarp.DomainWarp(ref wx, ref wy);

            float elevation = Mathf.InverseLerp(-1, 1, noiseSettings.fNoise.GetNoise(wx, wy));

            elevation = heightMultiplierEvaluate.Evaluate(elevation) * heightMultiplier;

            return elevation;
        }
    }

    [Serializable]
    public class BiomeSettingsNoise
    {
        private int seed;
        private int warpSeed;

        // General
        public FastNoiseLite.NoiseType noiseType = FastNoiseLite.NoiseType.OpenSimplex2;
        public float noiseFrequency = 0.02f;
        // Fractal
        public FastNoiseLite.FractalType fractalType = FastNoiseLite.FractalType.FBm;
        public int fractalOctaves = 5;
        public float fractalLacunarity = 2.0f;
        public float fractalGain = 0.5f;
        // Cellular
        public FastNoiseLite.CellularDistanceFunction cellularDistanceFunction = FastNoiseLite.CellularDistanceFunction.EuclideanSq;
        public FastNoiseLite.CellularReturnType cellularReturnType = FastNoiseLite.CellularReturnType.Distance;
        public float cellularJitter = 1.0f;

        // Warp
        public bool useWarpMode;

        // Domain Warp
        public FastNoiseLite.DomainWarpType domainWarpType = FastNoiseLite.DomainWarpType.OpenSimplex2;
        public float domainWarpAmplitude = 30.0f;
        public float domainWarpFrequency = 0.005f;

        // Domain Warp Fractal
        public FastNoiseLite.FractalType domainWarpFractalType = FastNoiseLite.FractalType.None;
        public int domainWarpFractalOctaves = 5;
        public float domainWarpFractalLacunarity = 2.0f;
        public float domainWarpFractalGain = 0.5f;


        private FastNoiseLite _fNoise;
        private FastNoiseLite _fNoiseWarp;

        public FastNoiseLite fNoise { get { return _fNoise; } }
        public FastNoiseLite fNoiseWarp { get { return _fNoiseWarp; } }

        /// <summary>
        /// noise para NormalMode
        /// </summary>
        /// <param name="seed"></param>
        public void SetNoiseNormal(int seed)
        {
            if (_fNoise != null && seed == this.seed) return;

            this.seed = seed;

            _fNoise = new FastNoiseLite(seed);
            _fNoise.SetNoiseType(noiseType);
            _fNoise.SetFrequency(noiseFrequency);

            _fNoise.SetFractalType(fractalType);
            _fNoise.SetFractalOctaves(fractalOctaves);
            _fNoise.SetFractalLacunarity(fractalLacunarity);
            _fNoise.SetFractalGain(fractalGain);

            _fNoise.SetCellularDistanceFunction(cellularDistanceFunction);
            _fNoise.SetCellularReturnType(cellularReturnType);

            _fNoise.SetCellularJitter(cellularJitter);

            //return fNoise;
        }

        /// <summary>
        /// noise para WarpMode
        /// </summary>
        /// <param name="seed"></param>
        public void SetNoiseWarp(int seed)
        {
            if (_fNoiseWarp != null && seed == warpSeed) return;

            warpSeed = seed;

            _fNoiseWarp = new FastNoiseLite(seed);

            _fNoiseWarp.SetDomainWarpType(domainWarpType);
            _fNoiseWarp.SetDomainWarpAmp(domainWarpAmplitude);

            _fNoiseWarp.SetFractalType(domainWarpFractalType);
            _fNoiseWarp.SetFractalOctaves(domainWarpFractalOctaves);
            _fNoiseWarp.SetFractalLacunarity(domainWarpFractalLacunarity);
            _fNoiseWarp.SetFractalGain(domainWarpFractalGain);

            //return fNoiseWarp;
        }

        /// <summary>
        /// inicia as classes de noise
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="warpSeed"></param>
        public void Init(int seed, int warpSeed)
        {
            SetNoiseNormal(seed);

            if (useWarpMode) SetNoiseWarp(warpSeed);
        }
    }
}
