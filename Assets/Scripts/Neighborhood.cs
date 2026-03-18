using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
    [Header("Set Dynamically")]
    public List<Boid> neighbors;
    private SphereCollider coll;

    // Start is called before the first frame update
    void Start()
    {
        neighbors = new List<Boid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = Spawner.S.neighborDist/2;
    }

    void FixedUpdate()
    {
        float nearRadius = Spawner.S.neighborDist * 0.5f;

        if (Mathf.Approximately(coll.radius, nearRadius))
        {
            coll.radius = nearRadius;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if(b != null)
        {
            if (!neighbors.Contains(b))
            {
                neighbors.Add(b);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if(b != null)
            {
                neighbors.Remove(b);
            }
        }

    }

    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if(neighbors.Count == 0) return avg;
            for(int i=0; i<neighbors.Count; i++)
            {
                avg += neighbors[i].pos;
            }

            avg /= neighbors.Count;
            return avg;
        }
    }

    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if(neighbors.Count == 0) return avg;

            for(int i=0; i<neighbors.Count; i++)
            {
                avg+=neighbors[i].rigid.velocity;
            }
            avg /= neighbors.Count;

            return avg;
        }
    }

    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i=0; i<neighbors.Count; i++)
            {
                delta = neighbors[i].pos - transform.position;
                if (delta.magnitude <= Spawner.S.collDist)
                {
                    avg += neighbors[i].pos;
                    nearCount++;
                }
            }

            // if there are no neighbors too close, return vector3.zero
            if(nearCount == 0) return Vector3.zero;

            // otherwise, average location
            avg/=nearCount;
            return avg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
