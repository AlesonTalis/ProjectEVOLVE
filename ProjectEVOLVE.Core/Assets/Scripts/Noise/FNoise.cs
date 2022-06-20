using Rand = System.Random;

namespace ProjectEVOLVE.Core.Noise
{
    public class FNoise
    {
		public FastNoiseLite heightMap;
		public FastNoiseLite heightMapWarp;

		public FastNoiseLite temperatureMap;
		public FastNoiseLite moistureMap;

		// configurações por biomas - para alturas dos biomas
		// plains
		public FastNoiseLite plainsHeightMap;
		public FastNoiseLite plainsHeightMapWarp;
		// desert
		public FastNoiseLite desertHeightMap;
		public FastNoiseLite desertHeightMapWarp;

		public FNoise(int seed)
        {
			Rand random = new Rand(seed);

			// height map
			SetupBaseHeightMap(random.Next(), random.Next());

			SetupTemperatureMap(random.Next());

			SetupMoistureMap(random.Next());

			// biomas
			// plains
			//SetupPlains(random.Next(), random.Next());
			// desert
			//SetupDesert(random.Next(), random.Next());
        }

		void SetupBaseHeightMap(int seed, int warpSeed)
        {
			heightMap = new FastNoiseLite(seed);
			heightMapWarp = new FastNoiseLite(warpSeed);

			heightMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			heightMap.SetFrequency(0.002f);
			heightMap.SetFractalType(FastNoiseLite.FractalType.FBm);
			heightMap.SetFractalOctaves(3);

			heightMapWarp.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
			heightMapWarp.SetDomainWarpAmp(50f);
			heightMapWarp.SetFrequency(0.015f);
			heightMapWarp.SetFractalType(FastNoiseLite.FractalType.DomainWarpProgressive);
			heightMapWarp.SetFractalOctaves(2);
			heightMapWarp.SetFractalLacunarity(1.8f);
		}

		void SetupTemperatureMap(int seed)
        {
			temperatureMap = new FastNoiseLite(seed);

			temperatureMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			temperatureMap.SetFrequency(0.005f);

			temperatureMap.SetFractalType(FastNoiseLite.FractalType.FBm);
			temperatureMap.SetFractalOctaves(4);
        }

		void SetupMoistureMap(int seed)
		{
			moistureMap = new FastNoiseLite(seed);

			moistureMap.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			moistureMap.SetFrequency(0.02f);

			moistureMap.SetFractalType(FastNoiseLite.FractalType.FBm);
			moistureMap.SetFractalOctaves(3);
			moistureMap.SetFractalGain(0.3f);
		}

		void SetupPlains(int seed, int warpSeed)
        {
			plainsHeightMap = new FastNoiseLite(seed);
			plainsHeightMapWarp = new FastNoiseLite(warpSeed);

			plainsHeightMap.SetNoiseType(FastNoiseLite.NoiseType.Value);
			plainsHeightMap.SetFrequency(0.005f);
			plainsHeightMap.SetFractalType(FastNoiseLite.FractalType.FBm);
			plainsHeightMap.SetFractalOctaves(1);

			plainsHeightMapWarp.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
			plainsHeightMapWarp.SetDomainWarpAmp(150);
			plainsHeightMapWarp.SetFrequency(0.005f);
			plainsHeightMapWarp.SetFractalType(FastNoiseLite.FractalType.DomainWarpProgressive);
			plainsHeightMapWarp.SetFractalOctaves(1);
        }

		void SetupDesert(int seed, int warpSeed)
        {
			desertHeightMap = new FastNoiseLite(seed);
			desertHeightMapWarp = new FastNoiseLite(warpSeed);

			desertHeightMap.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
			desertHeightMap.SetFrequency(0.01f);
			desertHeightMap.SetFractalType(FastNoiseLite.FractalType.FBm);
			desertHeightMap.SetFractalOctaves(1);
			desertHeightMap.SetCellularDistanceFunction(FastNoiseLite.CellularDistanceFunction.EuclideanSq);
			desertHeightMap.SetCellularReturnType(FastNoiseLite.CellularReturnType.Distance2Mul);
			desertHeightMap.SetCellularJitter(0.5f);

			desertHeightMapWarp.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
			desertHeightMapWarp.SetDomainWarpAmp(90);
			desertHeightMapWarp.SetFrequency(0.01f);
			desertHeightMap.SetFractalType(FastNoiseLite.FractalType.DomainWarpProgressive);
			desertHeightMap.SetFractalOctaves(2);
        }
    }
}