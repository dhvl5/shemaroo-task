using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Quaternion originalRot;

    private void Start()
    {
        originalRot = transform.rotation;
    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation * originalRot;
    }
}