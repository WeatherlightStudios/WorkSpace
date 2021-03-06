﻿using UnityEngine;
using System.Collections;

public static class Noise {

    public enum NormalizeMode {Local, Global};
    
    
	public static float[,] NoiseMapGenerator(int mapWidth, int mapHeight, float spacing, float scale, int seed, int octaves,float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode){
        
        float[,] noiseMap = new float[mapWidth +1, mapHeight +1];

        
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // float halfWidth = mapWidth / 2f;
        // float halfHeight = mapHeight / 2f;

        for(int x = 0; x <= mapWidth; x++)
        {
            for(int y = 0; y <= mapHeight; y++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;
                
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x*spacing + octaveOffsets[i].x) / scale * frequency ;
                    float sampleY = (y*spacing + octaveOffsets[i].y) / scale * frequency ;
                    
                    float perlinValue = 0;
                    if(i==-1){
                        perlinValue = 1 -Mathf.Abs(Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1);
                    } 
                    else{
                        perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    }
                    
                    
                    noiseHeight += perlinValue * amplitude;
                    
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if(noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y <= mapHeight; y++)
        {
            for(int x = 0; x <= mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }

            }
        }

        return noiseMap;
    }
}
