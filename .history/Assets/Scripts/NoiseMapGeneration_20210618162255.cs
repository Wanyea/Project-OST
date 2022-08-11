﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour {

	[System.Serializable]
	public class Wave {
		public float seed;
		public float frequency;
		public float amplitude;
	}


 
	public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ) {

        // create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];
 
		for (int zIndex = 0; zIndex < mapDepth; zIndex ++) {
			for (int xIndex = 0; xIndex < mapWidth; xIndex++) {

                // calculate sample indices based on the coordinates and the scale
				float sampleX = (xIndex + offsetX) / scale;
				float sampleZ = (zIndex + offsetZ) / scale;

				float noise  = 0f;
				float normalization = 0f;
				foreach(Wave wave in waves) {
					noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed , sampleZ * wave.frequency + wave.seed);
					normalization += wave.amplitude;
				}
 
                // generate noise value using PerlinNoise
				float noise = Mathf.PerlinNoise (sampleX, sampleZ);
 
				noiseMap [zIndex, xIndex] = noise;
			}
		}
 
		return noiseMap;
	}
}
