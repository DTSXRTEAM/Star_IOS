using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Cadpeople.Storyline
{
    public class PickStateMachineObjectWindow : EditorWindow {

        StateMachineEditor smPickTranition;
        StateMachineEditor smPickAction;
        StateMachineEditor smPickActionUsage;

        //-----------------------------------############---------------------------------\\

        [MenuItem("Cadpeople/Storyline")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(PickStateMachineObjectWindow), true, "Pick an ITransition Script");
        }



        void OnGUI()
        {
            if(smPickTranition)
            {
                List<MonoScript> transitionsList = smPickTranition.transitionsList;
                for (int i = 0; i < transitionsList.Count; i++)
                {
                    if(GUILayout.Button(transitionsList[i].name))
                    {
                        smPickTranition.PickTransitionScript(transitionsList[i]);
                        smPickTranition = null;
                        Close();
                    }
                    
                }

            }
            else if (smPickAction)
            {
                List<MonoScript> transitionsList = smPickAction.transitionsList;
                for (int i = 0; i < transitionsList.Count; i++)
                {
                    if (GUILayout.Button(transitionsList[i].name))
                    {
                        smPickAction.PickActionScript(transitionsList[i]);
                        smPickAction = null;
                        Close();
                    }

                }

            }
            else if(smPickActionUsage)
            {
                if(GUILayout.Button("Use as Enter Action"))
                {
                    smPickActionUsage.PickActionUsage(IAction.UseType.Enter);
                    smPickActionUsage = null;
                    Close();
                }
                if (GUILayout.Button("Use as Continuous Action"))
                {
                    smPickActionUsage.PickActionUsage(IAction.UseType.Continuous);
                    smPickActionUsage = null;
                    Close();
                }
                if (GUILayout.Button("Use as Exit Action"))
                {
                    smPickActionUsage.PickActionUsage(IAction.UseType.Exit);
                    smPickActionUsage = null;
                    Close();
                }
            }
            else
            {
                GUILayout.Label("This menu doesn't work without a StateMachine connected!");
            }


        }

        public void InitPickTransition(StateMachineEditor  stateMachineEditorRef)
        {
            smPickTranition = stateMachineEditorRef;
        }
        public void InitPickAction(StateMachineEditor stateMachineEditorRef)
        {
            smPickAction = stateMachineEditorRef;
        }
        public void InitPickActionUsage(StateMachineEditor stateMachineEditorRef)
        {
            smPickActionUsage = stateMachineEditorRef;
        }

    }

}

