using ProjectEVOLVE.Core.Data;
using ProjectEVOLVE.Core.Noise;
using ProjectEVOLVE.Core.Settings;
using ProjectEVOLVE.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = System.Random;


namespace ProjectEVOLVE.Core
{
    public partial class WorldGenerator
	{

		const float SMALL_NUMBER = 0.1f;
		const int LARGE_NUMBER = 100_000;
		const ushort MAX_WORLD_HEIGHT = 256;

		public static ChunkData GenerateChunk(WorldSettings settings, Vector2Int offset)
        {
			int chunkSize = settings.chunkSize + 2;
			ChunkData cData = new ChunkData(chunkSize, offset);
			var r = new Rand(settings.WorldSeed);
			settings.Validate(r.Next());// valida as configurações do WorldSettings

			Vector2Int globalOffset = new Vector2Int()
			{
				x = r.Next(-LARGE_NUMBER, LARGE_NUMBER),
				y = r.Next(-LARGE_NUMBER, LARGE_NUMBER)
			};

			FNoise fNoise = new FNoise(r.Next());
			//fNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
			//fNoise.SetFrequency(0.0015f);

			//fNoise.SetFractalOctaves(4);
			//fNoise.SetFractalType(FastNoiseLite.FractalType.FBm);

			// custom loop para todas as funções de mapa:
			for (int y = 0; y < chunkSize; y++)
			{
				for (int x = 0; x < chunkSize; x++)
				{

					Vector2Int pointOffset = new Vector2Int(x, y) + offset + globalOffset;

					cData.pointsData[x,y] = GeneratePoint(settings, fNoise, pointOffset);

				}
			}
			
			return cData;
        }

		public static PointData GeneratePoint(WorldSettings settings, FNoise fNoise, Vector2Int point)
        {
			PointData data = new PointData(point.x, point.y);

			switch (settings.worldType)
			{
				case WorldSettings.WorldType.FlatWorld:

					data.biome = ushort.MaxValue;

					data.heightData = settings.flatWorldElevation / MAX_WORLD_HEIGHT;

					data.height = settings.flatWorldElevation;

					data.temperature = 0;

					data.moisture = 0;

					data.decoration = 0;

					break;

				case WorldSettings.WorldType.DebugWorld:

					data.biome = ushort.MaxValue - 1;

					data.heightData = settings.debugWorldElevation / MAX_WORLD_HEIGHT;

					data.height = settings.debugWorldElevation;

					data.temperature = 0;

					data.moisture = 0;

					data.decoration = 0;

					break;

				case WorldSettings.WorldType.NormalWorld:

					float x = point.x * SMALL_NUMBER * settings.worldScale;
					float y = point.y * SMALL_NUMBER * settings.worldScale;


					// altura base do terreno
					float lx = x;
					float ly = y;

					fNoise.heightMapWarp.DomainWarp(ref lx, ref ly);
					float baseHeight = Mathf.InverseLerp(-1, 1, fNoise.heightMap.GetNoise(lx, ly));

					baseHeight *= baseHeight;


					// temperatura - baseada na altura base
					float temperature = Mathf.InverseLerp(-1, 1, fNoise.temperatureMap.GetNoise(x, y));


					// umidade - baseada na altura base + temperatura
					float moisture = Mathf.InverseLerp(-1, 1, fNoise.moistureMap.GetNoise(x, y));


					// biomas + elevação do bioma
					ushort biome = 0;
					float biomeElevation = 0;
					for (int i = 0; i < settings.biomeSettings.Length; i++)
					{
						if (temperature.InsideRange(settings.biomeSettings[i].temperatureRange)
							&&
							moisture.InsideRange(settings.biomeSettings[i].moistureRange))
						{
							// inside range
							biome = settings.biomeSettings[i].biomeId;
							biomeElevation = settings.biomeSettings[i].ValidateElevation(x, y);
						}
					}


					// elevação final
					float finalHeight = baseHeight + (biomeElevation * baseHeight);

					data.height = System.Convert.ToUInt16(Mathf.CeilToInt(finalHeight * 64 + 32));
					data.heightData = finalHeight;

					data.temperature = temperature;
					data.moisture = moisture;

					data.biome = biome;

					break;
			}

			return data;
        }
	}
}