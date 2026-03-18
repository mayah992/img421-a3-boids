using UnityEngine;

public class LookAtAttractor : MonoBehaviour
{
    public Vector3 offset = new Vector3(30, 50, 30);

    void Update()
    {
        transform.position = Attractor.POS + offset;
        transform.LookAt(Attractor.POS);
    }
}
