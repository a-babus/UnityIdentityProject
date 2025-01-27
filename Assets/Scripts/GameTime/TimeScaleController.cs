using UnityEngine;
using UnityEngine.Events;

namespace NTGD124
{
    // TimeScaleController: Provides smooth time scale transitions for slow-motion and speed-up effects
    public class TimeScaleController : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] 
        private string _componentDescription = @"Educational time control system that demonstrates:
        1. Smooth slow motion with proper physics
        2. Speed up effects
        3. Pause/unpause functionality
        4. Smooth transitions between different time states
        Controls: I (slow), O (fast), P (pause/unpause)";


        ///// Public Variables/Editor Properties /////
        [Header("Time Scale Settings")]
        [Tooltip("How slow the game runs in slow motion (0.2 = 20% speed)")]
        [Range(0.05f, 0.5f)]
        public float SlowMotionScale = 0.2f;

        [Tooltip("How fast the game runs in fast motion (2 = 200% speed)")]
        [Range(1.5f, 10f)]
        public float FastMotionScale = 2f;

        [Header("Transition Settings")]
        [Tooltip("How quickly the time scale changes (higher = faster transitions)")]
        [Range(1f, 5f)]
        public float TransitionSpeed = 2f;
        
        [Header("Control Keys")]
        // Using const for keys to prevent magic strings/values
        public KeyCode Slowdown_Key = KeyCode.I;
        public KeyCode Fastforward_Key = KeyCode.O;
        public KeyCode Pause_Key = KeyCode.P;

        [Header("Events")]
        [Tooltip("Called whenever time scale changes")]
        public UnityEvent<float> OnTimeScaleChanged;
        
        [Tooltip("Called when game is paused")]
        public UnityEvent OnGamePaused;
        
        [Tooltip("Called when game is unpaused")]
        public UnityEvent OnGameUnpaused;


        ///// Private Variables /////
        private float _defaultFixedDeltaTime;
        private float _currentTimeScale = 1f;
        private float _targetTimeScale = 1f;
        private bool _isPaused;


        ///// Unity Methods /////
        private void Start()
        {
            _defaultFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void Update()
        {
            HandleTimeControls();
        }

        private void OnDisable()
        {
            ResetTimeSettings();
        }


        ///// Action Methods /////
        private void HandleTimeControls()
        {
            // Check for pause toggle first
            if (Input.GetKeyDown(Pause_Key))
            {
                TogglePause();
                return;
            }

            if (_isPaused) return;

            // Handle time scale changes
            if (Input.GetKey(Slowdown_Key))
            {
                SetTargetTimeScale(SlowMotionScale);
            }
            else if (Input.GetKey(Fastforward_Key))
            {
                SetTargetTimeScale(FastMotionScale);
            }
            else
            {
                SetTargetTimeScale(1f);
            }

            UpdateTimeScale();
        }

        private void UpdateTimeScale()
        {
            if (_currentTimeScale == _targetTimeScale) return;

            // Smooth transition using unscaledDeltaTime
            _currentTimeScale = Mathf.Lerp(_currentTimeScale, _targetTimeScale, TransitionSpeed * Time.unscaledDeltaTime);

            // Snap to target when very close
            if (Mathf.Abs(_currentTimeScale - _targetTimeScale) < 0.01f)
            {
                _currentTimeScale = _targetTimeScale;
            }

            ApplyTimeScale(_currentTimeScale);
        }

        private void SetTargetTimeScale(float target)
        {
            _targetTimeScale = Mathf.Clamp(target, 0.1f, FastMotionScale);
        }

        private void ApplyTimeScale(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = _defaultFixedDeltaTime * scale;
            OnTimeScaleChanged?.Invoke(scale);
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            
            if (_isPaused)
            {
                ApplyTimeScale(0f);
                OnGamePaused?.Invoke();
            }
            else
            {
                ApplyTimeScale(_targetTimeScale);
                OnGameUnpaused?.Invoke();
            }
        }

        private void ResetTimeSettings()
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = _defaultFixedDeltaTime;
            _currentTimeScale = 1f;
            _targetTimeScale = 1f;
            _isPaused = false;
        }


        ///// Getter Methods /////
        public float GetCurrentTimeScale()
        {
            return _currentTimeScale;
        }

        public bool IsPaused()
        {
            return _isPaused;
        }
    }
}

// Implementation Steps:
// 1. Create an empty GameObject named "TimeController"
// 2. Attach this AdvancedTimeController script to it
// 3. Configure in inspector:
//    - Adjust SlowMotionScale (0.1 to 0.5)
//    - Adjust FastMotionScale (1.5 to 3.0)
//    - Set TransitionSpeed (1 to 5)
//    - Add any desired time scale change listeners
// 4. Controls:
//    - Hold I: Slow motion
//    - Hold O: Speed up
//    - Press P: Toggle pause