using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Base
{
    [RequireComponent(typeof(Renderer))]
    public class Highlightable : MonoBehaviour {

        [SerializeField]
        private Material highlightMaterial;

        private bool isHighlighted;
        private Material normalMaterial;
        private Renderer render;

        private void Awake()
        {
            render = GetComponent<Renderer>();
            normalMaterial = render.material;
        }

        public void SetHighlight(object mode)
        {
            if ((bool)mode)
                EnableHighlight();
            else
                DisableHighlight();
        }

        public void ToggleHighlight()
        {
            if (isHighlighted)
                DisableHighlight();
            else
                EnableHighlight();
        }

        public void EnableHighlight()
        {
            Texture texture = render.material.mainTexture;   
            render.material = highlightMaterial;

            if (texture != null)
                render.material.mainTexture = texture;

            isHighlighted = true;
        }

        public void DisableHighlight()
        {
            render.material = normalMaterial;
            isHighlighted = false;
        }
    }
}


