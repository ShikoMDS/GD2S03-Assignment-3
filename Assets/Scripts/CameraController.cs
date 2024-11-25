using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController instance; // Singleton instance for easy access
    public Transform[] stagePositions; // Camera positions for each stage
    public float panSpeed = 2f;

    private int currentStageIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void MoveToNextStage(int stageIndex)
    {
        if (stageIndex < stagePositions.Length)
        {
            currentStageIndex = stageIndex;
            StartCoroutine(PanToPosition(stagePositions[stageIndex].position));
        }
    }

    private IEnumerator PanToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;

        // Preserve the camera's Z position
        targetPosition.z = -10;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * panSpeed;

            // Interpolate position and maintain z = -10
            transform.position = Vector3.Lerp(
                startPosition,
                new Vector3(targetPosition.x, targetPosition.y, -10), // Ensure Z is always -10
                elapsed
            );

            yield return null;
        }

        // Snap to the final position to ensure accuracy
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -10);
    }
}
