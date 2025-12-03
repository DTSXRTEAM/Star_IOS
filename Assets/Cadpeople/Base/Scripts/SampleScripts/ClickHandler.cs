using UnityEngine;
using UnityEngine.EventSystems;
using Cadpeople.Events;

public class ClickHandler : MonoBehaviour, IPointerClickHandler,IPointerDownHandler, IPointerUpHandler
{
    public GameEvent Event;

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Pointer clicked!!");
        Event.Raise(new ColorWrapper{ Color = gameObject.GetComponent<Renderer>().sharedMaterial.color, Info = "Test" });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       // Debug.Log(name + " Pointer down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      //  Debug.Log(name + " Pointer up");
    }
}