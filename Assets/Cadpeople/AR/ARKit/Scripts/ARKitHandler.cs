using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.AR;
using Cadpeople.Events;
using UnityEngine.XR.iOS;
using System;

namespace Cadpeople.AR.ARKit
{
    public class ARKitHandler : MonoBehaviour, IARHandler
    {
        //--------------------------------------------------------------------------------\\
        //									EDITOR VARIABLES
        //--------------------------------------------------------------------------------\\

        [SerializeField]
        private GameObject arOriginPrefab;
        [SerializeField]
        private GameObject groundPointerPrefab;
        [SerializeField]
        private VectorThreeVariable arOriginPosition;
        [SerializeField]
        private BoolVariable useARCameraScaling;

        //--------------------------------------------------------------------------------\\
        //									GAME EVENTS
        //--------------------------------------------------------------------------------\\

        public GameEvent OnPlaneFoundGameEvent;
        public GameEvent OnModelPlacedGameEvent;
        public GameEvent OnImageFoundGameEvent;
        
        //--------------------------------------------------------------------------------\\
        //									EVENTS
        //--------------------------------------------------------------------------------\\

        public event OnPlaneFound OnPlaneFound;
        public event OnImageFound OnImageFound;
        public event OnModelPlaced OnModelPlaced;
        
        //--------------------------------------------------------------------------------\\
        //									SETTINGS
        //--------------------------------------------------------------------------------\\

        [SerializeField]
        private CadpeopleARKitSettings arSettings;

        //--------------------------------------------------------------------------------\\
        //									TRACKING TYPES
        //--------------------------------------------------------------------------------\\

        [SerializeField]
        private TrackingType imageTrackingType;
        [SerializeField]
        private TrackingType verticalSurfaceTrackingType;
        [SerializeField]
        private TrackingType horizontalSurfaceTrackingType;
        
        //--------------------------------------------------------------------------------\\
        //									PRIVATE VARIABLES
        //--------------------------------------------------------------------------------\\

        private bool isPlacingModel;
        private bool pointTrackingType;
        private ARHitTestResultType[] resultTypes;
        private GameObject groundPointer;
        public GameObject arOrigin;

		private string currentTrackedImageName;

        //--------------------------------------------------------------------------------\\
        //									METHODS
        //--------------------------------------------------------------------------------\\

        private void Start()
        {
            InitializeARScene();
        }

        private void LateUpdate()
        {
            if (isPlacingModel && pointTrackingType)
            {
                CheckPointPlacement();
            }
        }

		public void ClearCurrentTrackedImageName()
		{
			currentTrackedImageName = "";

			GetComponent<UnityARCameraManager>().ResetTracking();
		}

        public void InitializeARScene()
        {
            Debug.Log("Initializes AR Kit");

			currentTrackedImageName = "";

			// Check if ar settings has been set
			if (arSettings == null)
            {
                Debug.LogError("No ar kit settings assigned to the ar kit handler.");
                return;
            }
			// Start listen to AR Kit callbacks based on tracking types
			// Add tracking types to ar kit
			List<ARHitTestResultType> resultTypesList = new List<ARHitTestResultType>();
            var trackingTypes = arSettings.TrackingTypes;
            var verticalTracking = false;
            var horizontalTracking = false;
            for (int i = 0; i < trackingTypes.Length; i++)
            {
                // Listen to image anchor events
                if (trackingTypes[i] == imageTrackingType)
                {
                    UnityARSessionNativeInterface.ARImageAnchorAddedEvent += AddImageAnchor;
                    UnityARSessionNativeInterface.ARImageAnchorUpdatedEvent += UpdateImageAnchor;
                    UnityARSessionNativeInterface.ARImageAnchorRemovedEvent += RemoveImageAnchor;
                }
                // If vertical or horizontal tracking set flag
                if(trackingTypes[i] == verticalSurfaceTrackingType ||
                    trackingTypes[i] == horizontalSurfaceTrackingType)
                {
                    pointTrackingType = true;
                }
                // Add vertical and horizontal tracking types
                if(trackingTypes[i] == verticalSurfaceTrackingType)
                {
                    Debug.Log("Adding vertical plane tracking.");
                    resultTypesList.Add(ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane);
                    verticalTracking = true;
                }
                if (trackingTypes[i] == horizontalSurfaceTrackingType)
                {
                    Debug.Log("Adding horizontal plane tracking.");                    
                    resultTypesList.Add(ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane);
                    horizontalTracking = true;
                }
                // Listen to frame and anchor events
                UnityARSessionNativeInterface.ARAnchorAddedEvent += OnARAnchorAdded;
                UnityARSessionNativeInterface.ARFrameUpdatedEvent += OnARFrameUpdate;
            }
            resultTypesList.Add(ARHitTestResultType.ARHitTestResultTypeExistingPlane);
            resultTypesList.Add(ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
            // Change AR Kit camera manager settings
            if (horizontalTracking && !verticalTracking)
            {
                GetComponent<UnityARCameraManager>().planeDetection = UnityARPlaneDetection.Horizontal;
            } else if(!horizontalTracking && verticalTracking)
            {
                GetComponent<UnityARCameraManager>().planeDetection = UnityARPlaneDetection.Vertical;
            } else if(horizontalTracking && verticalTracking)
            {
                GetComponent<UnityARCameraManager>().planeDetection = UnityARPlaneDetection.HorizontalAndVertical;
            }
            //resultTypesList.Add(ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
            resultTypes = resultTypesList.ToArray();
            // Set reference image set
            GetComponent<UnityARCameraManager>().detectionImages = arSettings.arImageReferenceSet;
            // Determine to use AR Camera scaling or model scaling
            useARCameraScaling.Value = arSettings.useARCameraScaling;
            // Initialize ARKit UnityARCameraManager
            GetComponent<UnityARCameraManager>().enabled = true;
            // Start with placing the model 
            isPlacingModel = true;
        }

        private void CheckPointPlacement()
        {
            // Get center of screen as an AR point
            var centerPosition = Camera.main.ScreenToViewportPoint(
                new Vector2(
                    Screen.width * 0.5f,
                    Screen.height * 0.5f)
                );
            var centerPoint = new ARPoint
            {
                x = centerPosition.x,
                y = centerPosition.y
            };

            Vector3 pos;
            Quaternion rot;
            // Check each resulttype
            foreach (ARHitTestResultType resultType in resultTypes)
            {
                if (HitTestWithResultType(centerPoint, resultType, out pos, out rot))
                {
                    // Plane found, instantiate ground pointer
                    if(groundPointer == null)
                    {
                        groundPointer = Instantiate(groundPointerPrefab);
                    }
                    groundPointer.SetActive(true);
                    // Update groundpointer position
                    groundPointer.transform.position = pos;
                    
                    return;
                }
            }

        }

        /// <summary>
        /// Tries to move HitTransform to a valid position on a AR plane.
        /// </summary>
        /// <param name="point">The point in screen position</param>
        /// <param name="resultTypes">Filter the result based on resultType</param>
        /// <returns></returns>
        private bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes, out Vector3 position, out Quaternion rotation)
        {
            List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
            if (hitResults.Count > 0)
            {
                foreach (var hitResult in hitResults)
                {
                    position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                    rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                    return true;
                }
            }
            position = Vector3.zero;
            rotation = Quaternion.identity;
            return false;
        }

        private void InstantiateAndPlaceAROrigin(Vector3 position, Quaternion rotation)
        {
            if (arOrigin == null)
            {
                arOrigin = Instantiate(arOriginPrefab);
            }
            arOrigin.SetActive(true);

			var script = arOrigin.GetComponent<SmoothMove>();
			if (script != null)
			{
				script.SetTarget(position, rotation);
			}
			else 
			{
				arOrigin.transform.position = position;
				arOrigin.transform.rotation = rotation;
			}
        }

        //--------------------------------------------------------------------------------\\
        //						PUBLIC MODEL INTERACTION METHODS
        //--------------------------------------------------------------------------------\\

        public void PlaceModel()
        {
            if (isPlacingModel && pointTrackingType)
            {
                // Instantiate and/or place ar origin
                if(groundPointer != null)
                    InstantiateAndPlaceAROrigin(groundPointer.transform.position, groundPointer.transform.rotation);

                // Trigger event
                OnModelPlaced?.Invoke();
                OnModelPlacedGameEvent?.Raise(null);

                // Set global variable
                arOriginPosition.Value = arOrigin.transform.position;

                // Hide ground pointer
                groundPointer.SetActive(false);

                isPlacingModel = false;
                Debug.Log("Model placed");
            }
        }

        public void RepositionModel()
        {
            Debug.Log("Reposition model");
            isPlacingModel = true;
            // Deactivate ar origin
            arOrigin.SetActive(false);
        }

        public void ScaleARModel(Vector3 scale)
        {
            throw new System.NotImplementedException();
        }
        
        //--------------------------------------------------------------------------------\\
        //									AR KIT METHODS
        //--------------------------------------------------------------------------------\\

        private void AddImageAnchor(ARImageAnchor arImageAnchor)
        {
			if(!string.IsNullOrWhiteSpace(currentTrackedImageName) && arImageAnchor.referenceImageName != currentTrackedImageName)
			{
				Debug.Log("Image anchor not added for image " + arImageAnchor.referenceImageName + ", current tracking " + currentTrackedImageName);
				return;
			}

            Debug.Log("Image anchor added");
            isPlacingModel = false;

            // Get position and rotation
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

            // Instantiate and/or place ar origin
            InstantiateAndPlaceAROrigin(position, rotation);

            // Trigger events
            OnImageFound?.Invoke(position);
            OnImageFoundGameEvent?.Raise(arImageAnchor.referenceImageName);
            OnModelPlaced?.Invoke();
            OnModelPlacedGameEvent?.Raise(null);

			currentTrackedImageName = arImageAnchor.referenceImageName;
		}

        private void UpdateImageAnchor(ARImageAnchor arImageAnchor)
        {
			if (!string.IsNullOrWhiteSpace(currentTrackedImageName) && arImageAnchor.referenceImageName != currentTrackedImageName)
			{
				Debug.Log("Image anchor update skipped for image " + arImageAnchor.referenceImageName + ", currently tracking " + currentTrackedImageName);
				return;
			}

			Debug.Log("Image anchor updated");

            // Get position and rotation
            Vector3 position = UnityARMatrixOps.GetPosition(arImageAnchor.transform);
            Quaternion rotation = UnityARMatrixOps.GetRotation(arImageAnchor.transform);

            // Instantiate and/or place ar origin
            InstantiateAndPlaceAROrigin(position, rotation);

            // Trigger event
            OnModelPlaced?.Invoke();
            OnModelPlacedGameEvent?.Raise(null);
        }

        private void RemoveImageAnchor(ARImageAnchor arImageAnchor)
        {
            Debug.Log("Image anchor removed");
        }

        private void OnARFrameUpdate(UnityARCamera camera)
        {
            
        }

        private void OnARAnchorAdded(ARPlaneAnchor anchorData)
        {
            OnPlaneFound?.Invoke();
            OnPlaneFoundGameEvent?.Raise(anchorData.center);
            Debug.Log("Plane found");
        }
    }
}

