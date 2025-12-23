using Cadpeople.Events;
using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenInteraction : MonoBehaviour
{
    public GameEvent showInteractionMenuEvent;
    public GameEvent highlightComponentEvent;

    [Header("AR Foundation")]
    [SerializeField] private Camera arCamera; // NEW – assign AR Camera here

    void Update()
    {
        // ---------- TOUCH ----------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Over UI");
                return;
            }

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position); // CHANGED
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    var clickScript = hit.collider.GetComponent<Interactable>();
                    if (clickScript != null)
                    {
                        clickScript.Click();
                        return;
                    }
                }

                // same behaviour as before
                highlightComponentEvent?.Raise("");
                showInteractionMenuEvent?.Raise(new List<MenuAction>());
            }
        }

        // ---------- MOUSE (EDITOR TESTING) ----------
        else if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Over UI");
                return;
            }

            Camera cameraToUse = arCamera;

#if UNITY_EDITOR
            // Preserve your existing editor camera logic
            var cc = GameObject.FindGameObjectWithTag("EditorCamera");
            if (cc != null)
            {
                cameraToUse = cc.GetComponent<Camera>();
            }
#endif

            if (cameraToUse == null)
                return;

            Ray ray = cameraToUse.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var clickScript = hit.collider.GetComponent<Interactable>();
                if (clickScript != null)
                {
                    clickScript.Click();
                    return;
                }
            }

            highlightComponentEvent?.Raise("");
            showInteractionMenuEvent?.Raise(new List<MenuAction>());
        }
    }
}
