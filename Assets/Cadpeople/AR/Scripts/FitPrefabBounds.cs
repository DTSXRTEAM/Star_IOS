using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.AR
{
    public class FitPrefabBounds : MonoBehaviour
    {

        [SerializeField]
        private GameObject prefabTarget;


        private void Awake()
        {
            var bounds = new Bounds(transform.position, Vector3.zero);

            var renderers = prefabTarget.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            transform.localScale = new Vector3(bounds.extents.x * 0.2f, 1f, bounds.extents.z * 0.2f);
        }
    }
}


