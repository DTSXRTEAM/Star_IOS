using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Events;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif




namespace Cadpeople.Storyline
{


    //--------------------------------------------------------------------------------\\
    // The Basic Action interface for the State Machine
    //--------------------------------------------------------------------------------\\
    [ExecuteInEditMode]
    public abstract class IAction : MonoBehaviour
    {
        [Header("Debug")]
        public bool drawLabel = false;
        

        public enum UseType
        {
            Enter,
            Continuous,
            Exit,
            NONE
        }

        public abstract void Execute();

        protected virtual void OnDestroy()
        {
            State parentState = GetComponentInParent<State>();
            if (parentState)
            {
                parentState.enterActions.Remove(this);
                parentState.enterActions.TrimExcess();

                parentState.continuousActions.Remove(this);
                parentState.continuousActions.TrimExcess();

                parentState.exitActions.Remove(this);
                parentState.exitActions.TrimExcess();
            }
        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if(drawLabel)
                DrawIcon(gameObject, 6);

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

    }

    //--------------------------------------------------------------------------------\\
    //The Basic Transition interface for the State Machine
    //--------------------------------------------------------------------------------\\    
    [ExecuteInEditMode]
    public abstract class ITransition : MonoBehaviour
    {
        [Header("Debug")]
        bool drawLabel = false;

        [Space]
        public State targetState;
        public State parentState;
        [Space]
        public List<ITransition> booleanAndTransitions = new List<ITransition>();


        protected bool areBooleanAndTransitionsTrue = false;

        public virtual bool ShouldTransition()
        {
            areBooleanAndTransitionsTrue = true;
            
            foreach( ITransition t in booleanAndTransitions )
            {
                if (!t.ShouldTransition()) areBooleanAndTransitionsTrue = false;
                break;
            }
            return areBooleanAndTransitionsTrue;

        }
        public State GetTargetState() { return targetState; }
        public State GetTransitionLocation() { return parentState; }

        protected virtual void OnDestroy()
        {
            if(parentState)
            {
                parentState.transitions.Remove(this);
                parentState.transitions.TrimExcess();
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (drawLabel)
                DrawIcon(gameObject, 1);
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

    }
    //-----------------------------------############---------------------------------\\





    /// <summary>
    /// The Main Class in this Script: The State Machine:
    /// </summary>
    public class Statemachine : MonoBehaviour {


        public State startState;
        [Tooltip("The time in seconds between checking if the transitions in a state are true. -1 = every frame")]
        public float checkTransitionsInterval = -1f;
        [Tooltip("time in second between continuous actions are executed. -1 means every frame.")]
        public float executeContinuousActionsInterval = 2f;



        [Space(30)]
        public UnityEvent OnStateChange;
        [Tooltip("Send the reference to the state the statemachine changed to.")]
        public Cadpeople.Events.GameEvent OnStateChangeEvent;


        [Space(30)]

        //     Editor Variables:
        [HideInInspector]
        public GameObject newState;


        [HideInInspector]
        public Color stateColor;
        [HideInInspector]
        public Color transitionColor;
        [HideInInspector]
        public Color enterActionColor;
        [HideInInspector]
        public Color continuousActionColor;
        [HideInInspector]
        public Color exitActionColor;
        [Space(30)]
        public bool debug = false;
        public AudioClip transitionSound    = null;


        //-----------------------------------############---------------------------------\\


        protected State currentState           = null;
        protected float transitionsTimer       = 0f;
        protected float continuousActionsTimer = 0f;
        protected Coroutine blinkingCoroutine  = null;
        protected new AudioSource audio;
        protected float timer;

        //--------------------------------------------------------------------------------\\
        //									    Interface
        //--------------------------------------------------------------------------------\\





        //--------------------------------------------------------------------------------\\
        //									Member Functions
        //--------------------------------------------------------------------------------\\
        private void Start()
        {

        }

        private void OnEnable()
        {
            currentState = startState;
            audio = GetComponent<AudioSource>();
            if (!audio) audio = gameObject.AddComponent<AudioSource>();
            OnStateChangeEvent.Raise(currentState);
        }


        // Update is called once per frame
        void Update () {

            // if the currentState is for some reason a null-reference, we can't do anything.
            if (!currentState) return;

            //Check at the desired interval whether we should transition from the current state into another state
            if(checkTransitionsInterval != -1)
            {
                transitionsTimer += Time.deltaTime;
                if(transitionsTimer > checkTransitionsInterval)
                {
                    transitionsTimer = 0f;
                    CheckTransitions();
                }
            }
            else
            {
                CheckTransitions();
            }

            // Run the continuous actions in the current state at the desired interval
            if (executeContinuousActionsInterval != -1)
            {
                continuousActionsTimer += Time.deltaTime;
                if (continuousActionsTimer > executeContinuousActionsInterval)
                {
                    continuousActionsTimer = 0f;
                    currentState.ExecuteContinuousActions();
                }


            }
            else
            {
                currentState.ExecuteContinuousActions();
            }


            BlinkState();

        }


        void CheckTransitions()
        {

            {
                foreach(ITransition t in currentState.transitions)
                {
                    if(t.ShouldTransition())
                    {
                        // This transition is true, so we execute the exitActions in the currentState,
                        // set the currentState to the target state and then run the enterActions on the new currentState

                        if(t.GetTargetState() != null)
                        {
                            currentState.ExecuteExitActions();
                            currentState.GetComponent<Renderer>().material.color = stateColor;
                            currentState= t.GetTargetState();
                            currentState.ExecuteEnterActions();

                            if (OnStateChange != null) OnStateChange.Invoke();
                            OnStateChangeEvent.Raise(currentState);

                            Blink(t);

                            if (debug && audio && transitionSound) { audio.clip = transitionSound; audio.Play(); } 
                            break;
                        }
                        else
                        {
                            Debug.LogError("Target state in " + t.ToString() + " in " + currentState.ToString() + " is null.");
                            continue;
                        }


                    }
                }

            }

        }

        void BlinkState()
        {
            timer += Time.deltaTime;
            if(timer > 0.2f)
            {
                timer = 0f;
                Material mat = currentState.GetComponent<Renderer>().material;
                if (mat.color == stateColor) mat.color = Color.white;
                else if (mat.color == Color.white) mat.color = stateColor;
            }
            

        }

        void Blink(State state)
        {
            GameObject go = state.gameObject;
            if (go)
            {
                Renderer r = go.GetComponent<Renderer>();

                if (r)
                {
                    StartCoroutine(CoBlink(r, 3f, 0.2f, true));
                }

            }
        }

        void Blink(ITransition transition)
        {
            GameObject go = transition.gameObject;
            if(go)
            {
                Renderer r = go.GetComponent<Renderer>();
                
                if(r)
                {
                    StartCoroutine(CoBlink(r, 1, 2, true));
                }

            }
        }

        IEnumerator CoBlink(Renderer rend, float time, float interval, bool startWithBlinkColor = true )
        {
            if (!rend) yield break;

            Color originalColor = rend.material.color;

            float startTime = Time.time;

            while (Time.time - startTime < time)
            {
                if(startWithBlinkColor)
                {
                    rend.material.color = Color.white;
                    for (float t = 0f; t < interval; t += Time.deltaTime)
                    {
                        if (Time.time - startTime > time) { rend.material.color = originalColor; yield break; }
                        yield return null;
                    }
                    rend.material.color = originalColor;
                    for (float t = 0f; t < interval; t += Time.deltaTime)
                    {
                        if (Time.time - startTime > time) { rend.material.color = originalColor; yield break; }
                        yield return null;
                    }
                }
                else
                {
                    for(float t = 0f; t<interval; t+=Time.deltaTime )
                    {
                        if (Time.time - startTime > time) { rend.material.color = originalColor; yield break; }
                        yield return null;
                    }
                    rend.material.color = Color.white;
                    for (float t = 0f; t < interval; t += Time.deltaTime)
                    {
                        if (Time.time - startTime > time) { rend.material.color = originalColor; yield break; }
                        yield return null;
                    }
                    rend.material.color = originalColor;

                }
                yield return null;

            }



            yield break;
        }


    }

}

