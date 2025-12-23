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

    public GameEvent OnImageFoundGameEvent;

    public GameObject spawnedObjectPrefab;

    public GameObject AROriginModel;

    [Header("Do not assign")]
    public GameObject spawnedObject;

    // Internal state
    private HashSet<ARTrackedImage> trackedSet = new();
    private bool canScan = true;

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

        canScan = false; // lock scanning

        string imageName = img.referenceImage.name;
        Debug.Log("Image Found: " + imageName);
        OnImageFoundGameEvent?.Raise(imageName);

        spawnedObject = Instantiate(
            spawnedObjectPrefab,
            img.transform.position,
            img.transform.rotation,
            img.transform
        );

        SetArOriginPosition(spawnedObject.transform.position, spawnedObject.transform.rotation);

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

        //rescanPopup.SetActive(false);

        // Clear tracked images
        trackedSet.Clear();

        // Allow scanning again
        canScan = true;

        SetArOriginPosition(Vector3.zero, quaternion.identity);
    }

    public void SetArOriginPosition(Vector3 position, quaternion rotation)
    {
        if (AROriginModel != null)
        {
            AROriginModel.transform.position = position;
            AROriginModel.transform.rotation = rotation;
        }
    }
}
