using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections; 

namespace NTGD124
{
    public class InteractionZone : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)][SerializeField] private string _componentDescription = "Interaction zone that requires both player presence and key press to trigger. Fires events for interaction availability and actual interaction.";

        ///// Public Variables /////
        [Header("Target Settings")]
        [Tooltip("Only objects with this tag will trigger events")]
        public string TargetTag = "Player";

        [Header("Interaction Settings")]
        [Tooltip("Key that triggers the interaction")]
        public KeyCode InteractionKey = KeyCode.E;

        [Tooltip("If true, interaction will only work once")]
        public bool IsOneShot = false;

        [Header("UI Prompt Settings")]
        [Tooltip("Text to show when interaction is available (e.g., 'Press E to interact')")]
        public string InteractionPrompt = "Press E to interact";

        [Header("Debug Settings")]
        [Tooltip("Show interaction zone visualization in scene view")]
        public bool ShowDebugVisuals = true;

        [Tooltip("Color for zone when inactive")]
        public Color InactiveZoneColor = new Color(0, 1, 0, 0.3f);

        [Tooltip("Color for zone when interaction is available")]
        public Color AvailableZoneColor = new Color(1, 1, 0, 0.3f);

        [Tooltip("Color for zone when interaction is triggered")]
        public Color ActiveZoneColor = new Color(1, 0, 0, 0.3f);

        [Header("Events")]
        [Tooltip("Triggered when player enters zone and interaction becomes available")]
        public UnityEvent OnEventAvailable;

        [Tooltip("Triggered when player exits zone and interaction becomes unavailable")]
        public UnityEvent OnEventUnavailable;

        [Tooltip("Triggered when player performs the interaction")]
        public UnityEvent OnInteraction;

        [SerializeField] private GameObject MessageToDisplay;

        ///// Private Variables /////
        private BoxCollider _triggerCollider;
        private bool _isPlayerInside;
        private bool _hasInteracted;
        private Color _currentZoneColor;

        private static int numberOfItemsCollected = 0;
        private int totalPossibleNumberOfItemsCollected = 2;

        private bool doorsOpen = false;

        AudioManager audioManager;

        ///// Unity Methods /////
        private void Start()
        {
            InitializeComponents();
            _currentZoneColor = InactiveZoneColor;
        }

        private void Update()
        {
            if (_isPlayerInside && !_hasInteracted)
            {
                CheckForInteraction();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TargetTag) && !_hasInteracted)
            {
                _isPlayerInside = true;
                _currentZoneColor = AvailableZoneColor;
                OnEventAvailable?.Invoke();
                Debug.Log($"Interaction available: {gameObject.name}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TargetTag))
            {
                _isPlayerInside = false;
                _currentZoneColor = InactiveZoneColor;
                OnEventUnavailable?.Invoke();
                Debug.Log($"Interaction unavailable: {gameObject.name}");
            }
        }

        private void OnDrawGizmos()
        {
            if (ShowDebugVisuals)
            {
                DrawDebugVisuals();
            }
        }

        ///// Interaction Methods /////
        private void CheckForInteraction()
        {
            if (Input.GetKeyDown(InteractionKey))
            {
                HandleInteraction();
            }
        }

        private void HandleInteraction()
        {
            numberOfItemsCollected++;
            if (numberOfItemsCollected == 1)
            {
                HideDoors();
            }
            OnInteraction?.Invoke();
            _currentZoneColor = ActiveZoneColor;
            audioManager.PlaySFX(audioManager.itemCollected);

            Debug.Log($"Interaction triggered: {gameObject.name}");
            if (numberOfItemsCollected == totalPossibleNumberOfItemsCollected)
            {
                DisplayWinScreen();
            }

            if (IsOneShot)
            {
                _hasInteracted = true;
                OnEventUnavailable?.Invoke();

                // Optional: disable the game object after one-shot interaction
                //gameObject.SetActive(false);
            }

            
        }

        ///// Setup Methods /////
        private void InitializeComponents()
        {
            // Ensure we have a trigger collider
            _triggerCollider = GetComponent<BoxCollider>();
            if (_triggerCollider == null)
            {
                _triggerCollider = gameObject.AddComponent<BoxCollider>();
                _triggerCollider.isTrigger = true;
                Debug.Log("Added trigger collider to interaction zone");
            }
            else if (!_triggerCollider.isTrigger)
            {
                _triggerCollider.isTrigger = true;
                Debug.Log("Set existing collider to trigger mode");
            }
        }

        private void DrawDebugVisuals()
        {
            if (_triggerCollider == null) return;

            Gizmos.color = _currentZoneColor;

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

        private void HideDoors()
        {
            string targetTag = "OpenDoors";
            GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

            if (targetObject != null)
            {
                targetObject.SetActive(false); 
                Debug.Log($"{targetTag} object is now not visible.");
            }
            else
            {
                Debug.LogWarning($"No object found with tag: {targetTag}");
            }
        }

        public void DisplayWinScreen()
        {
            audioManager.PlaySFX(audioManager.win);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            DontDestroyOnLoad(audioManager.gameObject);
            SceneManager.LoadScene(3);
        }

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }
    }
}

/*
Implementation Steps:
1. Add this script to an empty GameObject in your scene
2. Configure the interaction zone:
   - Adjust BoxCollider size to define the interaction area
   - Set TargetTag to match your player object
   - Configure InteractionKey (default is E)
   - Set IsOneShot if interaction should only work once
   - Enable ShowDebugVisuals during setup
   
3. Set up events in the Unity Inspector:
   - OnEventAvailable: Triggered when player enters zone (good for showing UI prompts)
   - OnEventUnavailable: Triggered when player exits zone (good for hiding UI prompts)
   - OnInteraction: Triggered when player presses interaction key while in zone
   
Example Uses:
- NPC dialogue triggers
- Item pickup zones
- Door/lever interactions
- Quest objective markers
- Checkpoint activation

Tips:
- Use OnEventAvailable/Unavailable to show/hide UI prompts
- Consider adding custom inspector to preview interaction prompt
- Use descriptive names for interaction objects
- Add audio feedback for interaction availability/triggering
*/