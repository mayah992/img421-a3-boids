    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Boid : MonoBehaviour
    {
        [Header("Set Dynamically")]
        public Rigidbody rigid;
        private Neighborhood neighborhood;

        [Header("Obstacle Sensors")]
        public float sensorDist = 12f;
        public float sensorRadius = 2f;
        public float sensorAngle = 30f;

        void Awake()
        {
            neighborhood = GetComponent<Neighborhood>();
            rigid = GetComponent<Rigidbody>();

            // set a random initial position
            pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

            // set a random initial velocity
            Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;
            rigid.velocity = vel;

            LookAhead();

            // give boid a random color, but make sure it isn't too dark
            Color randColor = Color.black;
            while( randColor.r + randColor.g + randColor.b < 1.0f)
            {
                randColor = new Color(Random.value, Random.value, Random.value);
            }
            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in rends)
            {
                r.material.color = randColor;
            }

            TrailRenderer tRend = GetComponent<TrailRenderer>();
            tRend.material.SetColor("_TintColor", randColor);
        }

        void LookAhead()
        {
            // orients boid to look at the direction it is flying
            transform.LookAt(pos + rigid.velocity);
        }

        public Vector3 pos
        {
            get{ return transform.position;}
            set{ transform.position = value;}
        }

        void FixedUpdate()
        {
            Vector3 vel = rigid.velocity;
            Spawner spn = Spawner.S;

            // COLLISION AVOIDANCE - avoid neighbors who are too close
            Vector3 velAvoid = Vector3.zero;
            Vector3 tooClosePos = neighborhood.avgClosePos;
            // no need to react if response is Vector3.zero
            if(tooClosePos != Vector3.zero)
            {
                velAvoid = pos - tooClosePos;
                velAvoid.Normalize();
                velAvoid *= spn.velocity;
            }

            //VELOCITY MATCHING - try to match velocity with neighbors
            Vector3 velAlign = neighborhood.avgVel;
            // only do more if the velalign is not vector3.zero
            if(velAlign != Vector3.zero)
            {
                velAlign.Normalize();
                velAlign *= spn.velocity;
            }

            //FLOCK CENTERING - move towards center of local neighbors
            Vector3 velCenter = neighborhood.avgPos;
            if(velCenter != Vector3.zero)
            {
                velCenter -= transform.position;
                velCenter.Normalize();
                velCenter *= spn.velocity;
            }

            // ATTRACTION - Move towards the Attractor
            Vector3 delta = Attractor.POS - pos;
            // check wethere attracted to avoiding attractor
            bool attracted = (delta.magnitude > spn.attractPushDist);
            Vector3 velAttract = delta.normalized * spn.velocity;

            // apply all velocities
            float fdt = Time.fixedDeltaTime;

            if(velAvoid != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid*fdt);
            }
            else
            {
                if(velAlign != Vector3.zero)
                {
                    vel = Vector3.Lerp(vel, velAlign, spn.velMatching*fdt);
                }
                if(velCenter != Vector3.zero)
                {
                    vel = Vector3.Lerp(vel, velAlign, spn.flockCentering*fdt);
                }
                if(velAttract != Vector3.zero)
                {
                    if(attracted)
                    {
                        vel = Vector3.Lerp(vel, velAttract, spn.attractPull*fdt);
                    }
                    else
                    {
                        vel = Vector3.Lerp(vel, -velAttract, spn.attractPush*fdt);
                    }
                }
            }

            // TERRAIN AVOIDANCE
            Terrain terrain = Terrain.activeTerrain;

            if (terrain != null)
            {
                float terrainHeight = terrain.SampleHeight(pos) + terrain.transform.position.y;
                float minHeight = terrainHeight + 5f;

                if (pos.y < minHeight)
                {
                    pos = new Vector3(pos.x, minHeight, pos.z);
                    vel += Vector3.up * spn.velocity * 0.5f; // upward correction
                }
            }

            // OBSTACLE AVOIDANCE
            RaycastHit hit;

            Vector3 forward = rigid.velocity.normalized;
            Vector3 left = Quaternion.AngleAxis(-sensorAngle, Vector3.up) * forward;
            Vector3 right = Quaternion.AngleAxis(sensorAngle, Vector3.up) * forward;

            Vector3 avoidForce = Vector3.zero;

            if (Physics.SphereCast(pos, sensorRadius, forward, out hit, sensorDist))
                avoidForce += hit.normal;

            if (Physics.SphereCast(pos, sensorRadius, left, out hit, sensorDist))
                avoidForce += hit.normal;

            if (Physics.SphereCast(pos, sensorRadius, right, out hit, sensorDist))
                avoidForce += hit.normal;

            if (avoidForce != Vector3.zero)
            {
                avoidForce.Normalize();
                vel = Vector3.Lerp(vel, avoidForce * spn.velocity, 4f * fdt);
            }

            // set vel to velocity set on Spawner singleton
            vel = vel.normalized * spn.velocity;
            // assign to rigidbody
            rigid.velocity = vel;
            //look in direction of new velocity
            LookAhead();

            Debug.DrawRay(pos, forward * sensorDist, Color.red);
            Debug.DrawRay(pos, left * sensorDist, Color.blue);
            Debug.DrawRay(pos, right * sensorDist, Color.green);
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
