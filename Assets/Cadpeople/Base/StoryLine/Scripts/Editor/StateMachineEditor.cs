using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System;

namespace Cadpeople.Storyline
{

    [CustomEditor(typeof(Statemachine))]
    public class StateMachineEditor : Editor {


        Statemachine statemachine;
        bool editModeEnabled = false;
        bool addingNewState = false;


        //public Color stateColor;
        //public Color transitionColor;
        //public Color enterActionColor;
        //public Color continuousActionColor;
        //public Color exitActionColor;




        //GameObject statemachine.newState;
        float startMouseY;
        float elevation;
        Vector3 startPos;

        State clickedState;
        State chosenState;
        ITransition transitionChosen;
        private GameObject actionGO;
        IAction actionChosen;
        IAction.UseType actionUseType = IAction.UseType.NONE;
        State hoveringState = null;

        public List<MonoScript> transitionsList = new List<MonoScript>();
        public List<MonoScript> actionsList = new List<MonoScript>();

        GameObject transitionGO;

        Mesh transitionArrowStraightMesh;
        Mesh transitionArrowHomeMesh;


        //-----------------------------------############---------------------------------\\

        private void OnEnable()
        {
            statemachine = target as Statemachine;
            transitionArrowStraightMesh = (Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(x => x.name == "RoundArrow"));
            transitionArrowHomeMesh = (Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(x => x.name == "RoundHomeArrow"));
        }

        private void OnDisable()
        {
            if (statemachine.newState != null)
            {
                DestroyImmediate(statemachine.newState);
                statemachine.newState = null;
            }
            clickedState = null;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("------------------------------------------------------------------------------------------");
            EditorGUILayout.LabelField("                       ---~~¤¤##O    State Machine Editor    O##¤¤~~--- ");
            EditorGUILayout.LabelField("-------------------------------------------------------------------------------------------");
            EditorGUILayout.Space();

            //editModeEnabled = EditorGUILayout.Toggle("Edit Mode Enabled", editModeEnabled);
            
            //if(editModeEnabled)
            {
                GUIStyle style = new GUIStyle();
                style.name = "New State";

                //addingstatemachine.newState = EditorGUILayout.Toggle("Create New State", addingstatemachine.newState);
                if( GUILayout.Button("Add New State" ) )
                {
                    addingNewState= true;
                    if (statemachine.newState != null) DestroyImmediate(statemachine.newState);



                    statemachine.newState = new GameObject("New State");
                    statemachine.newState.transform.localScale = Vector3.one * 0.8f;
                    statemachine.newState.transform.parent = statemachine.transform;

                    statemachine.newState.AddComponent<State>();

                    MeshFilter mf = statemachine.newState.AddComponent<MeshFilter>();
                    mf.mesh = (Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(x => x.name == "Sphere"));

                    Renderer rend = statemachine.newState.AddComponent<MeshRenderer>();
                    Material mat = new Material(Shader.Find("Standard"));
                    mat.color = statemachine.stateColor;
                    rend.material = mat;

                    statemachine.newState.layer = LayerMask.NameToLayer("Editor");

                    

                    Debug.Log("Adding new state...");

                    //Repaint();
                }
                
                

            }

            EditorGUILayout.Space();

            statemachine.stateColor = EditorGUILayout.ColorField("State Color", statemachine.stateColor);
            statemachine.transitionColor = EditorGUILayout.ColorField("Transition Color", statemachine.transitionColor);
            statemachine.enterActionColor = EditorGUILayout.ColorField("Enter Action Color", statemachine.enterActionColor);
            statemachine.continuousActionColor = EditorGUILayout.ColorField("Continuous Action Color", statemachine.continuousActionColor);
            statemachine.exitActionColor = EditorGUILayout.ColorField("Exit Action Color", statemachine.exitActionColor);


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("!!!  MANUAL  !!!");
            EditorGUILayout.LabelField("Right click a state to add a state or transition.");
            EditorGUILayout.Space();

            EditorUtility.SetDirty(statemachine);
        }

        private void OnSceneGUI()
        {

            if (transitionChosen && transitionGO)
            {
                Vector2 mousePos = Event.current.mousePosition;

                if(hoveringState) Handles.Label(hoveringState.transform.position, "Right-click to use this state as target for your transition!");

                if (Event.current.type == EventType.MouseUp)
                {

                    GameObject go = HandleUtility.PickGameObject(Event.current.mousePosition, false);
                    if (go)
                    {
                        State targetState = go.GetComponent<State>();
                        if (targetState)
                        {
                            

                            Vector3 dir = go.transform.position - transitionGO.transform.position;
                            float mag = dir.magnitude;

                            if (targetState == chosenState)
                            {
                                transitionGO.GetComponent<MeshFilter>().mesh = transitionArrowHomeMesh;
                                transitionGO.transform.localScale = Vector3.one;
                                
                            }
                            else
                            {
                                transitionGO.transform.forward = dir;
                                Vector3 locScale = transitionGO.transform.localScale;
                                locScale.z = mag * 1.2f;
                                transitionGO.transform.localScale = locScale;

                                transitionChosen.targetState = targetState;

                                chosenState.transitions.Add(transitionChosen);

                            }


                            if (chosenState.transitions.Count > 1)
                            {

                                for(int i = 0; i< chosenState.transitions.Count; i++)
                                {
                                    List<ITransition> sameStateTransitions = chosenState.transitions.FindAll(x=>x.targetState == chosenState.transitions[i].targetState ) ;
                                    Debug.Log("Same state transitions = " + sameStateTransitions.Count);
                                    if(sameStateTransitions.Count > 1)
                                    {


                                        for (int k = 0; k < sameStateTransitions.Count; k++)
                                        {
                                            sameStateTransitions[k].transform.localPosition = Vector3.up * 0.3f;
                                            sameStateTransitions[k].transform.RotateAround(chosenState.transform.position,
                                                                                                sameStateTransitions[k].transform.forward,
                                                                                                (360f / sameStateTransitions.Count) * k);

                                            if (targetState != chosenState)
                                            {
                                                Vector3 scale = sameStateTransitions[k].transform.localScale;
                                                scale.x = (1f / sameStateTransitions.Count) / 0.3f;
                                                scale.y = (1f / sameStateTransitions.Count) / 0.3f;
                                                sameStateTransitions[k].transform.localScale = scale;
                                            }

                                        }

                                    }


                                }

                            }

                            transitionChosen = null;
                            transitionGO = null;

                            Event.current.Use();
                        }
                    }
                }

                if (Event.current.type == EventType.MouseMove)
                {



                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    GameObject go = HandleUtility.PickGameObject(Event.current.mousePosition, false);
                    if (go)
                    {
                        State targetState = go.GetComponent<State>();
                        if (targetState)
                        {
                            if (targetState == chosenState)
                            {
                                transitionGO.GetComponent<MeshFilter>().mesh = transitionArrowHomeMesh;
                                transitionGO.transform.localScale = Vector3.one;
                                return;
                            }
                            else if(transitionGO.GetComponent<MeshFilter>().sharedMesh.name != "RoundArrow")
                            {
                                transitionGO.GetComponent<MeshFilter>().mesh = transitionArrowStraightMesh;
                            }

                            Vector3 dir = go.transform.position - transitionGO.transform.position;
                            float mag = dir.magnitude;


                            transitionGO.transform.forward = dir;
                            Vector3 locScale = transitionGO.transform.localScale;
                            locScale.z = mag * 1.2f;
                            transitionGO.transform.localScale = locScale;

                            //Handles.Label(go.transform.position, "Right-click to use this state as target for your transition!");

                            hoveringState = targetState;
                        }
                        else
                        {
                            if (transitionGO.GetComponent<MeshFilter>().sharedMesh.name != "RoundArrow")
                            {
                                transitionGO.GetComponent<MeshFilter>().mesh = transitionArrowStraightMesh;
                            }

                            Plane worldPlane = new Plane(Vector3.up, statemachine.transform.position);
                            float dist = 0f;
                            if (worldPlane.Raycast(ray, out dist))
                            {
                                Vector3 pos = ray.GetPoint(dist);

                                Vector3 dir = pos - transitionGO.transform.position;
                                float mag = dir.magnitude;


                                transitionGO.transform.forward = dir;
                                Vector3 locScale = transitionGO.transform.localScale;
                                locScale.z = mag * 1.2f;
                                transitionGO.transform.localScale = locScale;

                                hoveringState = null;

                            }
                        }
                    }
                    else
                    {
                        if (transitionGO.GetComponent<MeshFilter>().sharedMesh.name != "RoundArrow")
                        {
                            transitionGO.GetComponent<MeshFilter>().mesh = transitionArrowStraightMesh; 
                        }

                        Plane worldPlane = new Plane(Vector3.up, statemachine.transform.position);
                        float dist = 0f;
                        if (worldPlane.Raycast(ray, out dist))
                        {
                            Vector3 pos = ray.GetPoint(dist);

                            Vector3 dir = pos - transitionGO.transform.position;
                            float mag = dir.magnitude;


                            transitionGO.transform.forward = dir;
                            Vector3 locScale = transitionGO.transform.localScale;
                            locScale.z = mag * 1.2f;
                            transitionGO.transform.localScale = locScale;

                            hoveringState = null;

                        }
                    }



                }


                return;

            }

            if(actionChosen && actionGO)
            {
                if(actionUseType == IAction.UseType.NONE)
                {
                    PickStateMachineObjectWindow PickActUseWindow = (PickStateMachineObjectWindow)EditorWindow.GetWindow(typeof(PickStateMachineObjectWindow), true, "Pick the usage of this action...");
                    PickActUseWindow.InitPickActionUsage(this);

                }
                else
                {
                    bool validStateChosen = false;

                    Material mat = new Material(Shader.Find("Standard"));

                    switch(actionUseType)
                    {
                        case IAction.UseType.Continuous:
                            chosenState.continuousActions.Add(actionChosen);

                            actionChosen.transform.localPosition = (Vector3.up * chosenState.continuousActions.Count * 0.4f) + (Vector3.up * 0.3f)  ;
                            
                            mat.color = statemachine.continuousActionColor;
                            actionChosen.GetComponent<Renderer>().material = mat;

                            validStateChosen = true;
                         
                            break;
                        case IAction.UseType.Enter:
                            chosenState.enterActions.Add(actionChosen);

                            actionChosen.transform.localPosition = Vector3.up * chosenState.enterActions.Count * 0.4f + (Vector3.up * 0.3f);
                            actionChosen.transform.localPosition += Vector3.right * 0.5f;
                            mat.color = statemachine.enterActionColor;
                            actionChosen.GetComponent<Renderer>().material = mat;

                            validStateChosen = true;
                            break;
                        case IAction.UseType.Exit:
                            chosenState.exitActions.Add(actionChosen);


                            actionChosen.transform.localPosition = Vector3.up * chosenState.enterActions.Count * 0.4f + (Vector3.up * 0.3f);
                            actionChosen.transform.localPosition -= Vector3.right * 0.5f;
                            mat.color = statemachine.exitActionColor;
                            actionChosen.GetComponent<Renderer>().material = mat;

                            validStateChosen = true;
                            break;
                    }

                    if(validStateChosen)
                    {




                        actionChosen = null;
                        actionGO = null;
                        actionUseType = IAction.UseType.NONE;


                    }
                    

                }
            }
            

            if (statemachine.newState != null)
            {
                if(Event.current.clickCount == 2)
                {
                    addingNewState = false;
                    statemachine.newState = null;
                    return;
                }

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Plane worldPlane = new Plane(Vector3.up, statemachine.transform.position);

                Handles.DrawLine(startPos, statemachine.transform.position);
                Handles.DrawLine(startPos, statemachine.newState.transform.position);


                if (Event.current.shift) // is the RMB down ?
                {
                    //elevation = (Event.current.mousePosition.y - startMouseY) * 0.1f;
                    elevation -= Event.current.delta.y * Time.deltaTime * 0.5f;
                    statemachine.newState.transform.position = startPos + (Vector3.up * elevation );
                    
                    return;
                }


                float distanceAlongRay = 0.0f;
                if( worldPlane.Raycast(ray, out distanceAlongRay) )
                {
                    
                    statemachine.newState.transform.position = ray.GetPoint(distanceAlongRay) + (Vector3.up * elevation);

                    //Handles.CubeHandleCap(100, ray.GetPoint(distanceAlongRay), Quaternion.identity, 1.2f, EventType.MouseDrag);
                    Handles.Label(statemachine.newState.transform.position + (Vector3.up * 0.7f), "Double-right-click\nto place.\n\nHold Shift to raise!");

                    //startMouseY = Event.current.mousePosition.y;
                    startPos = ray.GetPoint(distanceAlongRay);
                }

                return;
            }





            //if(clickedState)
            {
                //Handles.Label(Event.current.mousePosition, "dupidup!");
                Event evt = Event.current;
                Vector2 mousePos = evt.mousePosition;

               


                Rect contextRect = new Rect(evt.mousePosition.x, evt.mousePosition.y, 100, 100);
                if (Event.current.type == EventType.MouseUp)
                {
                    GameObject go =    HandleUtility.PickGameObject(evt.mousePosition, false);
                    if(go)
                        clickedState = go.GetComponent<State>();
                    
                    if (contextRect.Contains(mousePos) && clickedState)
                    {
                        //EditorUtility.DisplayPopupMenu(contextRect, "Cadpeople/State Machine/", null);


                        int choice = EditorUtility.DisplayDialogComplex("Create new...", "Would you like to create new Transition or Action?",
                        "Transition", "Action", "Cancel");

                        switch(choice)
                        {
                            case 0:
                                // 0 == Transition,
                                Debug.Log("Transition Chosen!");
                                //EditorGUIUtility.ShowObjectPicker<ITransition>(transitionChosen, true, "", 0);



                                MonoScript[] trans = GetScriptAssetsOfType<ITransition>();
                                transitionsList.Clear();
                                transitionsList.AddRange(trans);

                                //EditorUtility.DisplayPopupMenu(contextRect, "Cadpeople/Storyline", null);
                                PickStateMachineObjectWindow transWindow = (PickStateMachineObjectWindow)EditorWindow.GetWindow(typeof(PickStateMachineObjectWindow), true, "Pick an ITransition Script");
                                transWindow.InitPickTransition(this);


                                // Create Transition Object that goes from the selected state into the 
                                chosenState = clickedState;



                                clickedState = null;
                                break;
                            case 1:

                                Debug.Log("Action Chosen!");

                                MonoScript[] acts = GetScriptAssetsOfType<IAction>();
                                transitionsList.Clear();
                                transitionsList.AddRange(acts);

                                //EditorUtility.DisplayPopupMenu(contextRect, "Cadpeople/Storyline", null);
                                PickStateMachineObjectWindow actsWindow = (PickStateMachineObjectWindow)EditorWindow.GetWindow(typeof(PickStateMachineObjectWindow), true, "Pick an IAction Script");
                                actsWindow.InitPickAction(this);


                                chosenState = clickedState;


                                clickedState = null;
                                break;
                            case 2:
                                Debug.Log("Cancel Chosen!");
                                clickedState = null;
                                break;
                        }


                        //evt.Use();
                    }
                }
            }


         

        //    /// When the user hasn't anything active:
        //    /// 

        //    if(Event.current.button == 1) // right clicked:
        //    {
        //        if(Event.current.type == EventType.MouseUp)
        //        {
        //            GameObject go =  HandleUtility.PickGameObject(Event.current.mousePosition, true);
        //            if( go )
        //            {
        //                State state = go.GetComponent<State>();
        //                if(state)
        //                {

        //                    clickedState = state;

                           
        //                }
        //            }

        //        }

        //    }


        }



        public void PickTransitionScript(MonoScript t)
        {
            transitionGO = CreateTransitionObject(t);
            transitionChosen = transitionGO.GetComponent<ITransition>();
        }
        public void PickActionScript(MonoScript t)
        {
            Debug.Log("PickActionScript()");
            //PickStateMachineObjectWindow PickActUseWindow = (PickStateMachineObjectWindow)EditorWindow.GetWindow(typeof(PickStateMachineObjectWindow), true, "Pick the usage of this action...");
            //PickActUseWindow.InitPickActionUsage(this);
            actionGO = CreateActionObject(t);
            actionChosen = actionGO.GetComponent<IAction>();

        }
        public void PickActionUsage(IAction.UseType use)
        {
            actionUseType = use;
            Debug.Log("Usage " + use.ToString() + " chosen!");

        }

        public static MonoScript[] GetScriptAssetsOfType<T>()
        {
            MonoScript[] scripts = (MonoScript[])UnityEngine.Object.FindObjectsOfTypeAll(typeof(MonoScript));

            List<MonoScript> result = new List<MonoScript>();

            foreach (MonoScript m in scripts)
            {
                if (m.GetClass() != null && m.GetClass().IsSubclassOf(typeof(T)))
                {
                    result.Add(m);
                }
            }
            return result.ToArray();
        }


        protected GameObject CreateActionObject(MonoScript s)
        {
            GameObject go = new GameObject(s.name);

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = (Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(x => x.name == "Sphere"));

            Renderer rend = go.AddComponent<MeshRenderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = statemachine.continuousActionColor;
            rend.material = mat;


            IAction act = (IAction)go.AddComponent(s.GetClass());
            

            go.transform.parent = chosenState.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one * 0.25f;
            go.layer = LayerMask.NameToLayer("Editor");

            return go;
        }

        protected GameObject CreateTransitionObject(MonoScript s)
        {
            GameObject go = new GameObject(s.name);

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = (Resources.FindObjectsOfTypeAll<Mesh>().FirstOrDefault(x => x.name == "RoundArrow"));

            Renderer rend = go.AddComponent<MeshRenderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = statemachine.transitionColor;
            rend.material = mat;


            ITransition trans = (ITransition)go.AddComponent(s.GetClass());
            trans.parentState = chosenState;

            go.transform.parent = chosenState.transform;
            go.transform.localPosition = Vector3.zero;
            go.layer = LayerMask.NameToLayer("Editor");

            return go;

        }


        private void DrawIcon(GameObject gameObject, int idx)
        {
            var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            var icon = largeIcons[idx];
            var egu = typeof(EditorGUIUtility);
            var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var args = new object[] { gameObject, icon.image };
            var setIcon = egu.GetMethod("SetIconForObject", flags, null, new Type[] { typeof(UnityEngine.Object), typeof(Texture2D) }, null);
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


    }

}
