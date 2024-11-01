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
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * panSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
