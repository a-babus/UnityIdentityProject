using UnityEngine;
using UnityEngine.Events;

namespace NTGD124
{
    // TimeTrigger: Triggers events after a countdown reaches zero
    public class TimeTrigger : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] private string _componentDescription = "Simple countdown timer that triggers events when it reaches zero. You can reset it to use it again.";


        ///// Public Variables/Editor Properties /////
        [Header("Timer Settings")]
        [Tooltip("Time in seconds before the event triggers")]
        public float DelayTime = 3.0f;

        [Tooltip("Should the timer start over after triggering?")]
        public bool ResetAfterTrigger = false;

        [Header("Events")]
        [Tooltip("Events triggered when countdown reaches zero")]
        public UnityEvent OnTimeComplete;


        ///// Private Variables /////
        private float _currentTime;
        private bool _isRunning;


        ///// Unity Methods /////
        private void Start()
        {
            StartTimer();
        }

        private void Update()
        {
            UpdateTimer();
        }


        ///// Trigger Methods /////
        public void StartTimer()
        {
            _currentTime = DelayTime;
            _isRunning = true;
        }

        public void PauseTimer()
        {
            _isRunning = false;
        }

        public void ResumeTimer()
        {
            _isRunning = true;
        }


        ///// Action Methods /////
        private void UpdateTimer()
        {
            if (!_isRunning) return;

            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0f)
            {
                _currentTime = 0f;
                _isRunning = false;
                
                OnTimeComplete?.Invoke();

                if (ResetAfterTrigger)
                {
                    StartTimer();
                }
            }
        }
    }
}

// Implementation Steps:
// 1. Attach this script to any GameObject
// 2. Set up in inspector:
//    - DelayTime: How long to wait before triggering (in seconds)
//    - ResetAfterTrigger: Turn on if you want the timer to restart after triggering
//    - OnTimeComplete: Add what should happen when timer reaches zero
// 3. The timer starts automatically
// 4. Use PauseTimer() to pause and ResumeTimer() to continue
// 5. Use StartTimer() to reset and start over at any time