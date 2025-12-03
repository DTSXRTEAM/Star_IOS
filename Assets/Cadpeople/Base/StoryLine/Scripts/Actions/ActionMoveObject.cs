using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Storyline;


public class ActionMoveObject : IAction {


    public enum MovementUpdateType
    {
        WithUpdate,
        InitCoroutine,
        Instant,
        NONE
    }

    public enum MovementType
    {
        Linear,
        EaseIn

    }

    //-----------------------------------############---------------------------------\\

    public MovementUpdateType type;
    public MovementType moveType;
    public GameObject objectToMove;
    public Vector3 moveToPosition;
    public float speed;
    public float proximity = 0.01f;
    public bool enable;


    //-----------------------------------############---------------------------------\\

    protected Coroutine coroutineMovement;


    //-----------------------------------############---------------------------------\\

    private void OnDrawGizmosSelected()
    {
        if (!objectToMove) return;

        Gizmos.DrawLine(objectToMove.transform.position, moveToPosition);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(moveToPosition, 0.3f);

    }


    public override void Execute()
    {
        switch(type)
        {
            case MovementUpdateType.WithUpdate:
                if (enable) enabled = true;
                else enable = false;


                break;

            case MovementUpdateType.InitCoroutine:

                if (coroutineMovement != null) StopCoroutine(coroutineMovement);
                coroutineMovement = StartCoroutine(CoStartMoveTo());

                break;

            case MovementUpdateType.Instant:

                if(objectToMove)
                    objectToMove.transform.position = moveToPosition;

                break;

            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator CoStartMoveTo()
    {
        if (!objectToMove) yield break;


        float sqrDistance = (objectToMove.transform.position - moveToPosition).sqrMagnitude;
        Vector3 velocity = moveToPosition - objectToMove.transform.position;

        while (sqrDistance > proximity)
        {
            

            switch(moveType)
            {
                case MovementType.EaseIn:
                    objectToMove.transform.position += velocity * speed * Time.deltaTime;

                    break;

                case MovementType.Linear:
                    objectToMove.transform.position += velocity.normalized * speed * Time.deltaTime;
                    break;
            }

            velocity = moveToPosition - objectToMove.transform.position;
            sqrDistance = velocity.sqrMagnitude;

            yield return null;
        }


        yield break;
    }


}
