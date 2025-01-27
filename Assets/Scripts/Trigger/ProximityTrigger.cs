using UnityEngine;
using UnityEngine.Events;

namespace NTGD124
{
    // Simple trigger that activates when objects enter a specified range
    public class ProximityTrigger : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] private string _componentDescription = "Activates when the player or specified objects enter a certain range. Good for starting sequences or activating objects when the player gets close.";


        ///// Public Variables /////
        [Header("Trigger Settings")]
        public string TagToDetect = "Player";
        public float TriggerDistance = 5.0f;
        public bool CanTriggerMultipleTimes = false;

        [Header("Events")]
        public UnityEvent OnTriggerActivated;


        ///// Private Variables /////
        private bool _hasTriggered = false;


        ///// Unity Methods /////
        private void Update()
        {
            CheckProximity();
        }


        ///// Trigger Methods /////
        private void CheckProximity()
        {
            // Don't check if already triggered (unless multiple triggers allowed)
            if (_hasTriggered && !CanTriggerMultipleTimes) return;

            // Find target object
            GameObject target = GameObject.FindWithTag(TagToDetect);
            if (target == null) return;

            // Check distance
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= TriggerDistance)
            {
                TriggerAction();
            }
        }


        ///// Action Methods /////
        private void TriggerAction()
        {
            _hasTriggered = true;
            OnTriggerActivated?.Invoke();
            Debug.Log($"Proximity trigger activated! Distance reached to {TagToDetect}");
        }
    }
}

/*
Implementation Steps:
1. Attach this script to any GameObject that should detect nearby objects
2. Set TagToDetect to the tag you want to detect (usually "Player")
3. Set TriggerDistance to how close the player needs to be
4. In the Unity Inspector, add actions to OnTriggerActivated:
   - Drag objects that should be affected
   - Select their component and function to call
5. Set CanTriggerMultipleTimes if the trigger should work more than once
*/