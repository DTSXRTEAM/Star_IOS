using Cadpeople.Events;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageRescanController : MonoBehaviour
{
    [Header("AR")]
    public ARTrackedImageManager trackedImageManager;
    public ARAnchorManager anchorManager; // NEW

    public GameEvent OnImageFoundGameEvent;

    public GameObject spawnedObjectPrefab;

    public GameObject AROriginModel;

    [Header("Do not assign")]
    public GameObject spawnedObject;

    // Internal state
    private HashSet<ARTrackedImage> trackedSet = new();
    private bool canScan = true;

    private ARAnchor imageAnchor; // NEW

    void Start()
    {
        //rescanPopup.SetActive(false);
    }

    void Update()
    {
        if (!canScan)
            return;

        HashSet<ARTrackedImage> currentSet = new();

        foreach (var img in trackedImageManager.trackables)
            currentSet.Add(img);

        // Detect NEW images
        foreach (var img in currentSet)
        {
            if (!trackedSet.Contains(img) &&
                img.trackingState == TrackingState.Tracking)
            {
                trackedSet.Add(img);
                OnImageAdded(img);
                break; // detect only ONE image per scan
            }
        }
    }

    // -----------------------------
    // IMAGE FOUND
    // -----------------------------

    void OnImageAdded(ARTrackedImage img)
    {
        if (!canScan)
            return;

        canScan = false;

        string imageName = img.referenceImage.name;
        Debug.Log("Image Found: " + imageName);
        OnImageFoundGameEvent?.Raise(imageName);

        // ---- CREATE ANCHOR AT IMAGE POSE ----
        Pose anchorPose = new Pose(img.transform.position,Quaternion.Euler(0, img.transform.eulerAngles.y, 0)
);

        // Optional: stabilize floor images (lock tilt)
        anchorPose.rotation = Quaternion.Euler(
            0,
            anchorPose.rotation.eulerAngles.y,
            0
        );

        GameObject anchorGO = new GameObject("ImageAnchor");
        anchorGO.transform.SetPositionAndRotation(
            anchorPose.position,
            anchorPose.rotation
        );

        imageAnchor = anchorGO.AddComponent<ARAnchor>();

        if (imageAnchor == null)
        {
            Debug.LogError("Failed to create anchor");
            return;
        }

        // ---- SPAWN MODEL ON ANCHOR (NOT IMAGE) ----
        spawnedObject = Instantiate(
            spawnedObjectPrefab,
            imageAnchor.transform
        );

        // Keep your existing AR Origin logic
        SetArOriginPosition(
            imageAnchor.transform.position,
            imageAnchor.transform.rotation
        );

        // OPTIONAL: stop further image tracking
        trackedImageManager.enabled = false;
    }



    // -----------------------------
    // RESCAN BUTTON
    // -----------------------------

    public void OnRescanPressed()
    {
        Debug.Log("Rescan pressed");

        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }

        if (imageAnchor != null)
        {
            Destroy(imageAnchor.gameObject);
            imageAnchor = null;
        }

        trackedSet.Clear();
        canScan = true;

        trackedImageManager.enabled = true; // re-enable tracking

        SetArOriginPosition(Vector3.zero, Quaternion.identity);
    }


    public void SetArOriginPosition(Vector3 position, Quaternion rotation)
    {
        if (AROriginModel != null)
        {
            AROriginModel.transform.position = position;
            AROriginModel.transform.rotation = rotation;
        }
    }
}
