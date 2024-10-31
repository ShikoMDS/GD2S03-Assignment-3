using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdjuster : MonoBehaviour
{
    public RectTransform rectTransform;

    private void Awake()
    {
        // Try to get the RectTransform component
        rectTransform = GetComponent<RectTransform>();

        // Check if rectTransform is successfully retrieved
        if (rectTransform == null)
        {
            Debug.LogError("SafeAreaAdjuster: RectTransform component is missing on " + gameObject.name);
            return;
        }

        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        // Ensure rectTransform is not null before proceeding
        if (rectTransform == null)
        {
            Debug.LogError("SafeAreaAdjuster: rectTransform is null in ApplySafeArea for " + gameObject.name);
            return;
        }

        Rect safeArea = Screen.safeArea;

        // Convert safe area rectangle to anchor values
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Apply the calculated anchors and reset other properties to prevent scaling issues
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    private void OnRectTransformDimensionsChange()
    {
        // Ensure rectTransform is not null before applying the safe area
        if (rectTransform != null)
        {
            ApplySafeArea();
        }
    }
}
