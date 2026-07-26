using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Game Bootstrap started.");
        
        CreateCamera();
        CreateLight();
    }

    void Update()
    {
    }

    private void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        
        Camera camera = cameraObject.AddComponent<Camera>();
        
        cameraObject.transform.position = new Vector3(0, 10, -10);
        cameraObject.transform.rotation = Quaternion.Euler(45, 0, 0);
        
    }

    private void CreateLight()
    {
        GameObject lightObject = new GameObject("Sun");
        
        Light light = lightObject.AddComponent<Light>();
        
        light.type = LightType.Directional;
        lightObject.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Start Game"))
        {
            Debug.Log("Start Game button clicked.");
            // Add logic to start the game here
        }
    }
}