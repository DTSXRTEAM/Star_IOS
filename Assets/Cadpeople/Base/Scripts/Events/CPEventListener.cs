using UnityEngine;
using UnityEngine.Events;

namespace Cadpeople.Events
{
    public abstract class CPEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public abstract void OnEventRaised(object parameter);
    }
}
