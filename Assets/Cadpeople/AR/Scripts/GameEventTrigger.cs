using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cadpeople.Events;

namespace Cadpeople.AR
{
    public class GameEventTrigger : MonoBehaviour
    {

        [SerializeField]
        private GameEvent gameEvent;

        public void TriggerEvent()
        {
            if (gameEvent != null)
                gameEvent.Raise(null);
        }
    }

}
