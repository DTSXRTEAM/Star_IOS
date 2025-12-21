using Cadpeople.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageRescanController : MonoBehaviour
{
    [Header("AR")]
    public ARTrackedImageManager trackedImageManager;

    public GameEvent OnImageFoundGameEvent;

    [Tooltip("do not assign")]
    public GameObject spawnedObject;

    public GameObject spawnedObjectPrefab;

    [Header("UI")]
    //public GameObject rescanPopup;

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


    }
}
