using UnityEngine;

[ExecuteAlways]
public class FacePlayer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //look at scene view camera if it exists
        if (Camera.current != null)
        {
            transform.LookAt(Camera.current.transform.position);
        }
        else
        {
            // If no camera is available, look at the main camera
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                transform.LookAt(mainCamera.transform.position);
            }
        }
    }
}
