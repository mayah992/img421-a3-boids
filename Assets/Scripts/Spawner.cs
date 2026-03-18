using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static public Spawner S;
    static public List<Boid> boids;

    // adjust spawning behavior of boids
    [Header("Set in Inspector: Spawning")]
    public GameObject boidPrefab;
    public Transform boidAnchor;
    public int numBoids = 100;
    public float spawnRadius = 25f;
    public float spawnDelay = 0.1f;

    // adjust flocking behavior of boids
    [Header("Set in Inspector: Boids")]
    public float velocity = 30f;
    public float neighborDist = 30f;
    public float collDist = 4f;
    public float velMatching = 0.25f;
    public float flockCentering = 0.2f;
    public float collAvoid = 2f;
    public float attractPull = 2f;
    public float attractPush = 2f;
    public float attractPushDist = 5f;

    void Awake()
    {
        S = this;
        
        // start instantiation of boids
        boids =  new List<Boid>();
        InstantiateBoid();
    }

    public void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();

        b.transform.SetParent(boidAnchor);

        // Random spawn position around anchor
        Vector3 spawnPos = boidAnchor.position + Random.insideUnitSphere * spawnRadius;

        b.transform.position = spawnPos;

        boids.Add(b);

        if (boids.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
