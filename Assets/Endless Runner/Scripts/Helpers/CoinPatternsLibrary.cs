using System.Collections.Generic;
using UnityEngine;

public enum CoinPatterns
{
    NONE,
    STRAIGHT,
    ZIGZAG,
    ALLLANES,
    JUMPARC,
    RANDOMSPARSE,
    WAVE
}

public static class CoinPatternsLibrary
{
    private const float LANE_WIDTH = 4f;

    public static List<Vector3> GetPattern(
        CoinPatterns pattern,
        Transform referencePoint,
        float spacing = 0.6f,
        int count = 5
    )
    {
        return pattern switch
        {
            CoinPatterns.STRAIGHT =>
                Straight(referencePoint, spacing, count),

            CoinPatterns.ZIGZAG =>
                ZigZag(referencePoint, spacing, count),

            CoinPatterns.JUMPARC =>
                JumpArc(referencePoint, spacing, count),

            CoinPatterns.RANDOMSPARSE =>
                RandomSparse(referencePoint, spacing, count),

            CoinPatterns.ALLLANES =>
                AllLanes(referencePoint, spacing, count),

            CoinPatterns.WAVE =>
                Wave(referencePoint, spacing, count),

            CoinPatterns.NONE =>
                new List<Vector3>(),

            _ =>
                new List<Vector3>()
        };
    }

    // =========================
    // PATTERNS
    // =========================

    private static List<Vector3> Straight(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();
        for (int i = 0; i < count; i++)
        {
            points.Add(
                t.position + Vector3.forward * i * spacing
            );
        }
        return points;
    }

    private static List<Vector3> JumpArc(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();
        for (int i = 0; i < count; i++)
        {
            float height =
                Mathf.Sin((float)i / (count - 1) * Mathf.PI) * 2f;

            points.Add(
                t.position
                + Vector3.forward * i * spacing
                + Vector3.up * height
            );
        }
        return points;
    }

    private static List<Vector3> ZigZag(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();
        for (int i = 0; i < count; i++)
        {
            float direction = (i % 2 == 0) ? -1f : 1f;

            points.Add(
                t.position
                + Vector3.forward * i * spacing
                + Vector3.right * direction * LANE_WIDTH
            );
        }
        return points;
    }

    private static List<Vector3> RandomSparse(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();
        for (int i = 0; i < count; i++)
        {
            if (Random.value > 0.5f)
                continue;

            points.Add(
                t.position
                + Vector3.forward * i * spacing
            );
        }
        return points;
    }

    private static List<Vector3> AllLanes(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();

        float[] lanes =
        {
            -LANE_WIDTH,
            0f,
            LANE_WIDTH
        };

        for (int i = 0; i < count; i++)
        {
            foreach (float laneOffset in lanes)
            {
                points.Add(
                    t.position
                    + Vector3.forward * i * spacing
                    + Vector3.right * laneOffset
                );
            }
        }
        return points;
    }

    private static List<Vector3> Wave(
        Transform t, float spacing, int count)
    {
        List<Vector3> points = new();
        for (int i = 0; i < count; i++)
        {
            float xOffset =
                Mathf.Sin(i * 0.5f) * LANE_WIDTH;

            points.Add(
                t.position
                + Vector3.forward * i * spacing
                + Vector3.right * xOffset
            );
        }
        return points;
    }
}
