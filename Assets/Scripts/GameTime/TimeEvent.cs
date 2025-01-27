using UnityEngine;
using UnityEngine.Events;
using System;

namespace NTGD124
{
    // TimeEvent: Serializable class for configuring time-based events in the Unity Inspector
    [Serializable]
    public class TimeEvent
    {
        [Tooltip("Time in seconds when this event should trigger")]
        public float TriggerTime;
        
        [Tooltip("Event to trigger at the specified time")]
        public UnityEvent OnTrigger;

        [Tooltip("Has this event been triggered")]
        public bool HasTriggered;
    }
}