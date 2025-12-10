using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracker_Polling : MonoBehaviour
{
    public ARTrackedImageManager manager;

    [System.Serializable]
    public class ImagePrefab
    {
        public string imageName;
        public GameObject prefab;
    }

    public List<ImagePrefab> prefabs;

    private Dictionary<string, GameObject> prefabMap = new();
    private Dictionary<string, GameObject> spawnedByName = new();

    // Track which ARTrackedImage instances are alive
    private HashSet<ARTrackedImage> trackedSet = new();

    void Awake()
    {
        foreach (var p in prefabs)
            prefabMap[p.imageName] = p.prefab;
    }

    void Update()
    {
        // Build a temporary set of currently tracked images this frame
        HashSet<ARTrackedImage> currentSet = new();

        foreach (var img in manager.trackables)
            currentSet.Add(img);

        // Detect ADDED images
        foreach (var img in currentSet)
        {
            if (!trackedSet.Contains(img))
            {
                trackedSet.Add(img);
                OnImageAdded(img);
            }
        }

        // Detect REMOVED images
        var removed = new List<ARTrackedImage>();

        foreach (var img in trackedSet)
        {
            if (!currentSet.Contains(img))
            {
                removed.Add(img);
                OnImageRemoved(img);
            }
        }

        // Cleanup removed references
        foreach (var img in removed)
            trackedSet.Remove(img);

        // Detect UPDATED images
        foreach (var img in currentSet)
            OnImageUpdated(img);
    }

    // -----------------------------
    // HANDLERS
    // -----------------------------

    void OnImageAdded(ARTrackedImage img)
    {
        string name = img.referenceImage.name;

        if (!prefabMap.TryGetValue(name, out var prefab))
        {
            Debug.LogWarning($"No prefab assigned for {name}");
            return;
        }

        var obj = Instantiate(prefab, img.transform.position, img.transform.rotation);
        obj.transform.SetParent(img.transform);

        spawnedByName[name] = obj;
    }

    void OnImageUpdated(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        if (!spawnedByName.TryGetValue(name, out var obj))
            return;

        bool isTracking = img.trackingState == TrackingState.Tracking;
        obj.SetActive(isTracking);
    }

    void OnImageRemoved(ARTrackedImage img)
    {
        string name = img.referenceImage.name;
        if (spawnedByName.TryGetValue(name, out var obj))
        {
            Destroy(obj);
            spawnedByName.Remove(name);
        }
    }
}
