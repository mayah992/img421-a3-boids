using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 100f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;
    public float moveSpeed = 1f;

    public Terrain terrain;

    Vector3 center;

    void Start()
    {
        if (terrain == null)
            terrain = Terrain.activeTerrain;

        Vector3 terrainPos = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        center = terrainPos + terrainSize / 2f;

        // Start height
        center.y = 35f;

        transform.position = center;
    }

    void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        Vector3 tPos = center;

        float t = Time.time * moveSpeed;

        tPos.x += Mathf.Sin(xPhase * t) * radius;
        tPos.y += Mathf.Sin(yPhase * t) * radius;
        tPos.z += Mathf.Sin(zPhase * t) * radius;

        // get terrain height at position
        float terrainHeight = terrain.SampleHeight(tPos) + terrain.transform.position.y;

        // keep attractor above terrain
        float minHeight = terrainHeight + 5f;
        tPos.y = Mathf.Max(tPos.y, minHeight);

        transform.position = tPos;
        POS = tPos;
    }
}
