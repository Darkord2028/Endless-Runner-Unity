using System.Collections.Generic;
using UnityEngine;

public static class CoinPatterns
{
    // Straight line forward
    public static List<Vector3> Straight(Vector3 start, int count, float spacing)
    {
        List<Vector3> positions = new();

        for (int i = 0; i < count; i++)
        {
            positions.Add(start + Vector3.forward * i * spacing);
        }

        return positions;
    }

    // ZigZag between two lanes
    public static List<Vector3> ZigZag(Vector3 start, int count, float spacing, float laneWidth)
    {
        List<Vector3> positions = new();

        for (int i = 0; i < count; i++)
        {
            float xOffset = (i % 2 == 0) ? -laneWidth : laneWidth;
            positions.Add(start + new Vector3(xOffset, 0f, i * spacing));
        }

        return positions;
    }

    // All lanes at same Z
    public static List<Vector3> AllLanes(Vector3 center, float laneWidth)
    {
        return new List<Vector3>
        {
            center + Vector3.left * laneWidth,
            center,
            center + Vector3.right * laneWidth
        };
    }

    // Jump arc (sin wave)
    public static List<Vector3> JumpArc(Vector3 start, int count, float spacing, float height)
    {
        List<Vector3> positions = new();

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            float yOffset = Mathf.Sin(t * Mathf.PI) * height;

            positions.Add(start + new Vector3(0f, yOffset, i * spacing));
        }

        return positions;
    }

    // Random sparse coins forward
    public static List<Vector3> RandomSparse(Vector3 start, int count, float spacing,float chance)
    {
        List<Vector3> positions = new();

        for (int i = 0; i < count; i++)
        {
            if (Random.value > chance)
                continue;

            positions.Add(start + Vector3.forward * i * spacing);
        }

        return positions;
    }

    // Wave pattern (side to side)
    public static List<Vector3> Wave(Vector3 start, int count, float spacing, float amplitude)
    {
        List<Vector3> positions = new();

        for (int i = 0; i < count; i++)
        {
            float x = Mathf.Sin(i * 0.5f) * amplitude;
            positions.Add(start + new Vector3(x, 0f, i * spacing));
        }

        return positions;
    }
}
