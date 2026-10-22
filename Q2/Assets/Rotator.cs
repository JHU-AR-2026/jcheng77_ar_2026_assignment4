using UnityEngine;

// Optional: attach to collectibles / power-up so they spin.
public class Rotator : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
