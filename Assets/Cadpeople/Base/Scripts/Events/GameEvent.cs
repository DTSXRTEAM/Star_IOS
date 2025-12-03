using System.Collections.Generic;
using UnityEngine;

namespace Cadpeople.Events
{
    [CreateAssetMenu(menuName = "Events/Create new...", order = 1)]
    public class GameEvent : ScriptableObject
    {

        public string Parameter;

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<CPEventListener> eventListeners = 
            new List<CPEventListener>();

        public void Raise(object par)
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(par);
        }

        public void RegisterListener(CPEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(CPEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }
}