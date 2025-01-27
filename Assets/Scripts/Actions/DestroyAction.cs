using UnityEngine;
using UnityEngine.Events;

namespace NTGD124
{
    public class DestroyAction : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)][SerializeField] private string _componentDescription = "Provides configurable destroy action with optional delay, effects, and events. Can be triggered externally via DestroyObject().";

        ///// Public Variables /////
        [Header("Destroy Settings")]
        [Tooltip("Optional delay before destruction (in seconds)")]
        public float DestroyDelay = 0f;

        [Tooltip("If true, will destroy child objects separately instead of with parent")]
        public bool DetachChildrenBeforeDestroy = false;

        [Tooltip("If true, will log debug information")]
        public bool EnableDebugLog = true;

        [Header("Events")]
        [Tooltip("Triggered right before destruction begins")]
        public UnityEvent OnDestroyStart;

        [Tooltip("Triggered after destruction (useful for delayed destruction)")]
        public UnityEvent OnDestroyComplete;

        ///// Public Methods /////
        public void DestroyObject(GameObject targetObject)
        {
            if (targetObject == null)
            {
                DebugLog("Attempted to destroy null object");
                return;
            }

            DebugLog($"Initiating destruction of: {targetObject.name}");
            OnDestroyStart?.Invoke();

            if (DetachChildrenBeforeDestroy)
            {
                DetachAndDestroyChildren(targetObject);
            }

            if (DestroyDelay <= 0)
            {
                ExecuteDestroy(targetObject);
            }
            else
            {
                StartCoroutine(DelayedDestroy(targetObject));
            }
        }

        ///// Private Methods /////
        private void DetachAndDestroyChildren(GameObject parent)
        {
            // Store children in array since hierarchy will change during detachment
            Transform[] children = new Transform[parent.transform.childCount];
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                children[i] = parent.transform.GetChild(i);
            }

            // Detach and destroy each child
            foreach (Transform child in children)
            {
                child.SetParent(null);
                if (DestroyDelay <= 0)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    StartCoroutine(DelayedDestroy(child.gameObject));
                }
                DebugLog($"Detached and queued destruction of child: {child.name}");
            }
        }

        private System.Collections.IEnumerator DelayedDestroy(GameObject obj)
        {
            DebugLog($"Waiting {DestroyDelay} seconds before destroying {obj.name}");
            yield return new WaitForSeconds(DestroyDelay);
            ExecuteDestroy(obj);
        }

        private void ExecuteDestroy(GameObject obj)
        {
            DebugLog($"Destroying object: {obj.name}");
            Destroy(obj);
            OnDestroyComplete?.Invoke();
        }

        private void DebugLog(string message)
        {
            if (EnableDebugLog)
            {
                Debug.Log($"[DestroyAction - {gameObject.name}] {message}");
            }
        }
    }
}

/*
Implementation Steps:
1. Add this script to an empty GameObject in your scene
2. Configure the destroy action:
   - Set DestroyDelay if you want delayed destruction
   - Enable DetachChildrenBeforeDestroy if needed
   - Set up OnDestroyStart/OnDestroyComplete events if needed
   
3. Trigger the destruction:
   - Call DestroyObject(gameObject) from other scripts
   - Connect to UnityEvents (e.g., from InteractionZone)
   
Example Uses:
- Destroying collectibles after pickup
- Breaking destructible objects
- Cleaning up spawned effects
- Removing temporary objects
- Destroying projectiles on impact

Tips:
- Use DestroyDelay for playing destruction effects
- Enable debug logging during setup
- Use DetachChildrenBeforeDestroy for complex hierarchies
- Connect OnDestroyComplete to trigger follow-up actions
*/