using UnityEngine;

// Attach to Main Camera. Drag the Player into "player". (a)
public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;  // initial offset
    }

    void LateUpdate()
    {
        if (player.activeInHierarchy)
            transform.position = player.transform.position + offset;
    }
}
