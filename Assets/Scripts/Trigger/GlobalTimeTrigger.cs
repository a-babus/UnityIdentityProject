using UnityEngine;
using System.Collections.Generic;

namespace NTGD124
{
    // GlobalTimeTrigger: Handles triggering events at specific times based on a GameTime component
    public class GlobalTimeTrigger : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] private string _componentDescription = "Triggers events at specific times based on a GameTime component. Configure time events in the inspector and they will automatically trigger when the time is reached.";


        ///// Public Variables/Editor Properties /////
        [Header("Timer Reference")]
        [Tooltip("Reference to the GameTime component")]
        public GameTime GameTimeComponent;

        [Header("Time Events")]
        [Tooltip("List of events that trigger at specific times")]
        public List<TimeEvent> TimeEvents = new List<TimeEvent>();


        ///// Unity Methods /////
        private void Start()
        {
            ResetTriggers();
        }

        private void Update()
        {
            CheckEvents();
        }


        ///// Action Methods /////
        private void CheckEvents()
        {
            if (!GameTimeComponent || !GameTimeComponent.IsRunning()) return;

            float currentTime = GameTimeComponent.GetCurrentTime();

            foreach (TimeEvent timeEvent in TimeEvents)
            {
                if (timeEvent.HasTriggered) continue;

                bool shouldTrigger = GameTimeComponent.IsCountdown() 
                    ? currentTime <= timeEvent.TriggerTime 
                    : currentTime >= timeEvent.TriggerTime;

                if (shouldTrigger)
                {
                    timeEvent.OnTrigger?.Invoke();
                    timeEvent.HasTriggered = true;
                }
            }
        }


        ///// Trigger Methods /////
        public void ResetTriggers()
        {
            foreach (TimeEvent timeEvent in TimeEvents)
            {
                timeEvent.HasTriggered = false;
            }
        }
    }
}

// Implementation Steps:
// 1. Make sure you have a GameTime component set up in your scene
// 2. Create an empty GameObject
// 3. Add this GlobalTimeTrigger component to it
// 4. Drag the GameTime component reference to the GameTimeComponent field
// 5. Add TimeEvents in the inspector:
//    - Set TriggerTime in seconds
//    - Add desired Unity Events to trigger at that time
// 6. Events will automatically trigger when their time is reached