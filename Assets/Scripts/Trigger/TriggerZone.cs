using UnityEngine;
using UnityEngine.Events;

namespace NTGD124
{
    // Provides configurable trigger zone with enter/exit/stay events and one-shot option
    public class TriggerZone : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)] [SerializeField] private string _componentDescription = "Configurable trigger zone that fires events when objects enter/exit/stay. Can be set to deactivate after first use or repeat indefinitely.";


        ///// Public Variables /////
        [Header("Target Settings")]
        [Tooltip("Only objects with this tag will trigger events")]
        public string TargetTag = "Player";
        
        [Header("Event Type")]
        [Tooltip("If true, trigger will fire once and deactivate")]
        public bool IsOneShot = false;

        [Header("Stay Event Settings")]
        [Tooltip("Enable events while object is inside")]
        public bool UseStayEvent = false;

        [Tooltip("How often the stay event should fire (in seconds)")]
        public float StayEventInterval = 0.1f;

        [Header("Debug Settings")]
        [Tooltip("Show trigger zone visualization in scene view")]
        public bool ShowDebugVisuals = true;

        [Tooltip("Color for trigger zone when inactive")]
        public Color InactiveZoneColor = new Color(0, 1, 0, 0.3f);

        [Tooltip("Color for trigger zone when object is inside")]
        public Color ActiveZoneColor = new Color(1, 0, 0, 0.3f);

        [Header("Events")]
        [Tooltip("Triggered when valid object enters the zone")]
        public UnityEvent OnObjectEnter;

        [Tooltip("Triggered when valid object exits the zone")]
        public UnityEvent OnObjectExit;

        [Tooltip("Triggered periodically while object is in zone")]
        public UnityEvent OnObjectStay;


        ///// Private Variables /////
        private BoxCollider _triggerCollider;
        private bool _isObjectInside;
        private float _nextStayEventTime;


        ///// Unity Methods /////
        private void Start()
        {
            InitializeComponents();
        }

        private void Update()
        { 
            if (UseStayEvent)
            {
                HandleStayEvent();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TargetTag))
            {
                TriggerEnterResponse();
                
                if (IsOneShot)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TargetTag))
            {
                TriggerExitResponse();
            }
        }

        private void OnDrawGizmos()
        {
            if (ShowDebugVisuals)
            {
                DrawDebugVisuals();
            }
        }


        ///// Trigger Methods /////
        private void HandleStayEvent()
        {
            if (_isObjectInside && Time.time >= _nextStayEventTime)
            {
                OnObjectStay?.Invoke();
                _nextStayEventTime = Time.time + StayEventInterval;
            }
        }


        ///// Action Methods /////
        private void InitializeComponents()
        {
            // Ensure we have a trigger collider
            _triggerCollider = GetComponent<BoxCollider>();
            if (_triggerCollider == null)
            {
                _triggerCollider = gameObject.AddComponent<BoxCollider>();
                _triggerCollider.isTrigger = true;
                Debug.Log("Added trigger collider to area trigger");
            }
            else if (!_triggerCollider.isTrigger)
            {
                _triggerCollider.isTrigger = true;
                Debug.Log("Set existing collider to trigger mode");
            }
        }

        private void TriggerEnterResponse()
        {
            _isObjectInside = true;
            _nextStayEventTime = Time.time;
            OnObjectEnter?.Invoke();
            Debug.Log($"Object entered trigger: {gameObject.name}");
        }

        private void TriggerExitResponse()
        {
            _isObjectInside = false;
            OnObjectExit?.Invoke();
            Debug.Log($"Object exited trigger: {gameObject.name}");
        }

        private void DrawDebugVisuals()
        {
            if (_triggerCollider == null) return;

            Gizmos.color = _isObjectInside ? ActiveZoneColor : InactiveZoneColor;
            
            // Draw trigger bounds
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(_triggerCollider.center, _triggerCollider.size);
            
            // Draw slightly smaller wireframe for better visibility
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(
                _triggerCollider.center,
                Vector3.Scale(_triggerCollider.size, new Vector3(0.99f, 0.99f, 0.99f))
            );
        }
    }
}

/*
Implementation Steps:
1. Add this script to an empty GameObject in your scene
2. Configure the trigger zone:
   - Adjust BoxCollider size to define the trigger area
   - Set TargetTag to match your target objects
   - Choose IsOneShot if trigger should only fire once
   - Enable ShowDebugVisuals during setup
   
3. Set up events in the Unity Inspector:
   - OnObjectEnter: Triggered when object enters
   - OnObjectExit: Triggered when object leaves
   - OnObjectStay: Triggered while object is inside (if enabled)
   
4. Optional stay event:
   - Enable UseStayEvent for periodic triggers
   - Adjust StayEventInterval for timing
   
Example Uses:
- Collectibles (IsOneShot = true)
- Damage zones (UseStayEvent = true)
- Checkpoint areas (IsOneShot = false)
- Story triggers (IsOneShot = true)

Tips:
- Use descriptive names for trigger objects
- Adjust collider size using scene view handles
- Watch the console for trigger debugging
- Use stay events for continuous effects
*/