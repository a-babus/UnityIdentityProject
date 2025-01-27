using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace NTGD124
{
    // Monitors multiple conditions and triggers event when all are met
    public class ConditionalTrigger : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)] [SerializeField] private string _componentDescription = "Monitors required conditions and triggers event when all are met. Continuously checks condition state in Update. View completed conditions in inspector.";


        ///// Public Variables /////
        [Header("Required Conditions")]
        [Tooltip("Names of all required conditions")]
        public string[] RequiredConditions;

        [Header("Completed Conditions")]
        [Tooltip("Shows which conditions are currently completed")]
        public HashSet<string> CompletedConditions = new HashSet<string>();

        [Header("Events")]
        [Tooltip("Triggered when all conditions become met")]
        public UnityEvent ConditionalEvent;


        ///// Private Variables /////
        private bool _hasTriggered;


        ///// Unity Methods /////
        private void Update()
        {
            CheckConditions();
        }


        ///// Action Methods /////
        public void CompleteCondition(string conditionName)
        {
            CompletedConditions.Add(conditionName);
            Debug.Log($"Condition completed: {conditionName} ({CompletedConditions.Count}/{RequiredConditions.Length})");
        }

        public void ResetCondition(string conditionName)
        {
            CompletedConditions.Remove(conditionName);
            _hasTriggered = false;
            Debug.Log($"Condition reset: {conditionName} ({CompletedConditions.Count}/{RequiredConditions.Length})");
        }

        private void CheckConditions()
        {
            if (_hasTriggered) return;

            foreach (string condition in RequiredConditions)
            {
                if (!CompletedConditions.Contains(condition))
                {
                    return;
                }
            }

            // All conditions are met!
            _hasTriggered = true;
            ConditionalEvent?.Invoke();
            Debug.Log("All conditions met - conditional event triggered!");
        }
    }
}

/*
Implementation Steps:
1. Setup:
   - Add ConditionalTrigger to empty GameObject
   - Add required condition names in inspector
   - Watch CompletedConditions directly in inspector
   - Set up ConditionalEvent in inspector

2. For Each Condition:
   - Create trigger zones with AreaTrigger
   - In trigger events call:
     * CompleteCondition("ConditionName")
     * ResetCondition("ConditionName") if needed

Example Scene Setup:
```
Scene
├── ConditionalTrigger
│   RequiredConditions = ["Switch1", "Switch2", "Switch3"]
│   CompletedConditions = {View in Inspector!}
│   ConditionalEvent → [Your Actions Here]
└── Triggers
    ├── Switch1
    │   └── AreaTrigger
    │       OnEnter → CompleteCondition("Switch1")
    ├── Switch2
    │   └── AreaTrigger
    │       OnEnter → CompleteCondition("Switch2")
    └── Switch3
        └── AreaTrigger
            OnEnter → CompleteCondition("Switch3")
```

Tips:
- Watch CompletedConditions directly in inspector
- Make sure condition names match exactly
- Use clear, descriptive condition names
- Group related triggers in hierarchy
*/