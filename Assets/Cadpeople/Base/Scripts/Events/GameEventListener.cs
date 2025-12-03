using UnityEngine;
using UnityEngine.Events;

namespace Cadpeople.Events
{
    public class GameEventListener : CPEventListener
    {
        [Tooltip("Response to invoke when Event is raised.")]
         public ObjectEvent Response;

        public override void OnEventRaised(object parameter)
        {
            if (Response == null)
            {
                Debug.Log("No response attached!!");
            }
            else
            {
                Response.Invoke(parameter);
            }
        }
    }
}
