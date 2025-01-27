using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace NTGD124
{
    // Input-based trigger with configurable press type, timing, and delayed actions
    public class ButtonPressTrigger : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] private string _componentDescription = "Triggers immediate and delayed actions based on button press type (down/up/hold). For hold, you can set how often the action repeats.";

        ///// Public Variables /////
        [Header("Input Settings")]
        public KeyCode TriggerKey = KeyCode.Space;

        AudioManager audioManager;
        public enum PressType
        {
            OnPress,    // Triggers when button is first pressed
            OnRelease,  // Triggers when button is released
            WhileHold   // Triggers repeatedly while button is held
        }

        [Tooltip("When should the action trigger?")]
        public PressType TriggerType = PressType.OnPress;

        [Header("Hold Settings")]
        [Tooltip("How often to trigger while holding (in seconds)")]
        [Range(0.1f, 5.0f)]
        public float HoldRepeatInterval = 0.5f;

        [Header("Delay Settings")]
        [Tooltip("Time to wait before triggering delayed action (in seconds)")]
        public float ActionDelay = 1.0f;

        [Tooltip("If true, cancels delayed action if button is released early")]
        public bool CancelDelayedOnRelease = false;

        [Header("Events")]
        [Tooltip("Triggered immediately on button input")]
        public UnityEvent OnTrigger;

        [Tooltip("Triggered after the specified delay")]
        public UnityEvent OnDelayedTrigger;

        ///// Private Variables /////
        private float _nextHoldActionTime = 0f;
        private Coroutine _delayedActionCoroutine = null;

        ///// Unity Methods /////
        private void Update()
        {
            CheckButtonInput();  // Trigger: Input-Based
        }

        private void OnDisable()
        {
            // Clean up any pending delayed actions when disabled
            if (_delayedActionCoroutine != null)
            {
                StopCoroutine(_delayedActionCoroutine);
                _delayedActionCoroutine = null;
            }
        }

        ///// Trigger Methods /////
        private void CheckButtonInput()
        {
            switch (TriggerType)
            {
                case PressType.OnPress:
                    
                    if (Input.GetKeyDown(TriggerKey))
                    { 
                        TriggerAction();
                    }
                    break;

                case PressType.OnRelease:
                    if (Input.GetKeyUp(TriggerKey))
                    {
                        TriggerAction();
                    }
                    break;

                case PressType.WhileHold:
                    if (Input.GetKey(TriggerKey) && Time.time >= _nextHoldActionTime)
                    {
                        TriggerAction();
                        _nextHoldActionTime = Time.time + HoldRepeatInterval;
                    }
                    break;
            }

            // Handle cancellation of delayed action
            if (CancelDelayedOnRelease && Input.GetKeyUp(TriggerKey))
            {
                CancelDelayedAction();
            }
        }

        ///// Action Methods /////
        private void TriggerAction()
        {
            // Trigger immediate action
            OnTrigger?.Invoke();
            Debug.Log($"Button action triggered: {TriggerType}");

            // Start delayed action
            if (ActionDelay > 0 && OnDelayedTrigger.GetPersistentEventCount() > 0)
            {
                if (_delayedActionCoroutine != null)
                {
                    StopCoroutine(_delayedActionCoroutine);
                }
                _delayedActionCoroutine = StartCoroutine(DelayedActionCoroutine());
            }
        }

        private IEnumerator DelayedActionCoroutine()
        {
            Debug.Log($"Delayed action scheduled for {ActionDelay} seconds");
            yield return new WaitForSeconds(ActionDelay);

            OnDelayedTrigger?.Invoke();
            Debug.Log("Delayed action triggered");
            _delayedActionCoroutine = null;
        }

        private void CancelDelayedAction()
        {
            if (_delayedActionCoroutine != null)
            {
                StopCoroutine(_delayedActionCoroutine);
                _delayedActionCoroutine = null;
                Debug.Log("Delayed action cancelled");
            }
        }

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }
    }
}

/*
Implementation Steps:
1. Attach this script to any GameObject
2. In the Inspector:
   - Set the TriggerKey (e.g., Space, E, R)
   - Choose the TriggerType:
     * OnPress: Triggers once when first pressed
     * OnRelease: Triggers once when released
     * WhileHold: Triggers repeatedly while held
   - For WhileHold, set HoldRepeatInterval (how often to trigger)
   - Set ActionDelay for delayed triggers
   - Enable CancelDelayedOnRelease if needed
3. Set up both events:
   - OnTrigger: Immediate response
   - OnDelayedTrigger: Delayed response
4. Test the different modes and timing

Example Uses:
- Charging attacks (immediate wind-up, delayed release)
- Delayed door mechanisms
- Time-bomb placement
- Staged animations
- Power-up activation sequences

Tips:
- Use CancelDelayedOnRelease for interruptible actions
- Combine with animations to show charging/delay progress
- Consider adding visual/audio feedback during delay
- Use different delays for different effect combinations
*/