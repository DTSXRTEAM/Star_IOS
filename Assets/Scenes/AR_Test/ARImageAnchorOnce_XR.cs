using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARImageAnchorOnce_XR : MonoBehaviour
{
    [Header("Prefab to place on detected image")]
    public GameObject contentPrefab;

    private ARTrackedImageManager imageManager;
    private bool hasPlaced = false;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void Update()
    {
        if (hasPlaced)
            return;

        foreach (var image in imageManager.trackables)
        {
            if (image.trackingState != TrackingState.Tracking)
                continue;

            PlaceAndAnchor(image);
            break;
        }
    }

    private void PlaceAndAnchor(ARTrackedImage image)
    {
        hasPlaced = true;

        // Create an anchor GameObject at image pose
        GameObject anchorGO = new GameObject("ImageAnchor");
        anchorGO.transform.SetPositionAndRotation(
            image.transform.position,
            image.transform.rotation
        );

        // Add ARAnchor component (6.0.6 compatible)
        ARAnchor anchor = anchorGO.AddComponent<ARAnchor>();

        // Instantiate content as child of anchor
        Instantiate(
            contentPrefab,
            anchor.transform.position,
            anchor.transform.rotation,
            anchor.transform
        );

        // Stop image tracking completely
        imageManager.enabled = false;

        Debug.Log("Image detected once and anchored in world space.");
    }
}
