using UnityEngine;

public class LookAtAttractor : MonoBehaviour
{
    public Vector3 offset = new Vector3(30, 50, 30);
    // orbit
    public float orbitSpeed = 5f;

    void Update()
    {
        // rotate the offset around the Y axis
        offset = Quaternion.Euler(0, orbitSpeed * Time.deltaTime, 0) * offset;
        
        transform.position = Attractor.POS + offset;
        transform.LookAt(Attractor.POS);
    }
}
