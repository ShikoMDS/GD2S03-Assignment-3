using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioManager : MonoBehaviour
{
    public float targetAspectWidth = 19f; // Width of the target aspect ratio
    public float targetAspectHeight = 9f; // Height of the target aspect ratio

    private void Start()
    {
        AdjustCameraViewport();
    }

    private void AdjustCameraViewport()
    {
        // Calculate the target aspect ratio
        float targetAspect = targetAspectWidth / targetAspectHeight;

        // Calculate the current screen aspect ratio
        float screenAspect = (float)Screen.width / Screen.height;

        // Calculate the scaling factor to fit the screen
        float scaleHeight = screenAspect / targetAspect;

        Camera camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor; // Ensure the background is cleared
        camera.backgroundColor = Color.black; // Set black background color

        if (scaleHeight < 1.0f) // Letterbox: black bars at top and bottom
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else // Pillarbox: black bars at left and right
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    private void OnPreRender()
    {
        // Clear the background explicitly
        GL.Clear(true, true, Color.black);
    }
}