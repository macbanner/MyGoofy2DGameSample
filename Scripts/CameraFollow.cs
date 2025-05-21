using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Takip edilecek nesne (karakter)
    public Vector3 offset = new Vector3(0f, 1f, -10f); // Kamera uzaklığı

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
