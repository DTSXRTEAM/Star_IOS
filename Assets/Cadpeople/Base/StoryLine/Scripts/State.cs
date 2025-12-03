using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif



namespace Cadpeople.Storyline
{

    public class State : MonoBehaviour {




        [Space]
        public bool transitionsBooleanAnd = false;
        public List<ITransition> transitions = new List<ITransition>();
        public List<IAction> enterActions = new List<IAction>();
        public List<IAction> exitActions = new List<IAction>();
        public List<IAction> continuousActions = new List<IAction>();
        //-----------------------------------############---------------------------------\\


        protected Renderer rend;
        protected MeshFilter mf;




     //   private float gizmoTimer = 0f;

        //--------------------------------------------------------------------------------\\
        //									Member Functions
        //--------------------------------------------------------------------------------\\

        private void Start()
        {
            Statemachine sm = GetComponentInParent<Statemachine>();

            if (!mf) mf = GetComponent<MeshFilter>();
            if (!mf)
            {
                mf = gameObject.AddComponent<MeshFilter>();
            }

            if (!rend) rend = GetComponent<Renderer>();
            if (!rend) rend = gameObject.AddComponent<MeshRenderer>();

            
        }


        

        private void OnValidate()
        {
         
        }



#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            DrawIcon(gameObject, 5);
        }

        private void DrawIcon(GameObject gameObject, int idx)
        {
            var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            var icon = largeIcons[idx];
            var egu = typeof(EditorGUIUtility);
            var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var args = new object[] { gameObject, icon.image };
            var setIcon = egu.GetMethod("SetIconForObject", flags, null, new System.Type[] { typeof(UnityEngine.Object), typeof(Texture2D) }, null);
            setIcon.Invoke(null, args);
        }
        private GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] array = new GUIContent[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
            }
            return array;
        }
#endif


        //--------------------------------------------------------------------------------\\
        //									Interface
        //--------------------------------------------------------------------------------\\



        public void ExecuteContinuousActions()
        {
            foreach(IAction a in continuousActions)
            {
                a.Execute();
            }
        }
        public void ExecuteEnterActions()
        {
            foreach (IAction a in enterActions) a.Execute();
        }
        public void ExecuteExitActions()
        {
            foreach (IAction a in exitActions) a.Execute();
        }

    }

}

