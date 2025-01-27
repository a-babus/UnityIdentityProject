using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NTGD124
{
    public class SceneAction : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)][SerializeField] private string _componentDescription = "Configurable action that can activate/deactivate objects by layer or direct reference, and change materials on specified renderers. Can be triggered externally via TriggerAction().";

        ///// Public Variables /////
        [Header("Layer-based Activation")]
        [Tooltip("Layers containing objects to activate")]
        public LayerMask LayersToActivate;

        [Tooltip("Layers containing objects to deactivate")]
        public LayerMask LayersToDeactivate;

        [Header("Direct Object References")]
        [Tooltip("Specific objects to activate")]
        public List<GameObject> ObjectsToActivate = new List<GameObject>();

        [Tooltip("Specific objects to deactivate")]
        public List<GameObject> ObjectsToDeactivate = new List<GameObject>();

        [System.Serializable]
        public class MaterialChange
        {
            public Renderer TargetRenderer;
            public Material NewMaterial;
            public int MaterialIndex = 0;
            public Material OriginalMaterial; // Stored for reverting
        }

        [Tooltip("List of material changes to apply")]
        public List<MaterialChange> MaterialChanges = new List<MaterialChange>();

        [Header("Action Settings")]
        [Tooltip("If true, action can only be triggered once")]
        public bool IsOneShot = false;

        [Tooltip("If true, will log debug information")]
        public bool EnableDebugLog = true;

        [Header("Events")]
        [Tooltip("Triggered when the action is executed")]
        public UnityEvent OnActionTriggered;

        ///// Private Variables /////
        private bool _hasTriggered = false;
        private Dictionary<Renderer, Material[]> _originalMaterials = new Dictionary<Renderer, Material[]>();

        ///// Unity Methods /////
        private void Start()
        {
            StoreMaterialStates();
        }

        ///// Public Methods /////

        [ContextMenu("Test")]
        public void TriggerAction()
        {
            if (IsOneShot && _hasTriggered)
            {
                DebugLog("Action already triggered (OneShot)");
                return;
            }

            HandleLayerActivation();
            HandleDirectObjectActivation();
            HandleMaterialChanges();

            _hasTriggered = true;
            OnActionTriggered?.Invoke();
            DebugLog("Action triggered successfully");
        }

        public void RevertMaterials()
        {
            foreach (var materialChange in MaterialChanges)
            {
                if (materialChange.TargetRenderer != null && materialChange.OriginalMaterial != null)
                {
                    var materials = materialChange.TargetRenderer.sharedMaterials;
                    materials[materialChange.MaterialIndex] = materialChange.OriginalMaterial;
                    materialChange.TargetRenderer.sharedMaterials = materials;
                }
            }
            DebugLog("Materials reverted to original state");
        }

        ///// Private Methods /////
        private void HandleLayerActivation()
        {
            // Handle activation
            if (LayersToActivate.value != 0)
            {
                GameObject[] objectsToActivate = FindObjectsInLayers(LayersToActivate);
                foreach (var obj in objectsToActivate)
                {
                    obj.SetActive(true);
                    DebugLog($"Activated object by layer: {obj.name}");
                }
            }

            // Handle deactivation
            if (LayersToDeactivate.value != 0)
            {
                GameObject[] objectsToDeactivate = FindObjectsInLayers(LayersToDeactivate);
                foreach (var obj in objectsToDeactivate)
                {
                    obj.SetActive(false);
                    DebugLog($"Deactivated object by layer: {obj.name}");
                }
            }
        }

        private void HandleDirectObjectActivation()
        {
            // Activate specific objects
            foreach (var obj in ObjectsToActivate)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    DebugLog($"Activated specific object: {obj.name}");
                }
            }

            // Deactivate specific objects
            foreach (var obj in ObjectsToDeactivate)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    DebugLog($"Deactivated specific object: {obj.name}");
                }
            }
        }

        private void HandleMaterialChanges()
        {
            foreach (var materialChange in MaterialChanges)
            {
                if (materialChange.TargetRenderer != null && materialChange.NewMaterial != null)
                {
                    var materials = materialChange.TargetRenderer.sharedMaterials;

                    // Store original if not already stored
                    if (materialChange.OriginalMaterial == null)
                    {
                        materialChange.OriginalMaterial = materials[materialChange.MaterialIndex];
                    }

                    // Apply new material
                    materials[materialChange.MaterialIndex] = materialChange.NewMaterial;
                    materialChange.TargetRenderer.sharedMaterials = materials;

                    DebugLog($"Changed material on {materialChange.TargetRenderer.gameObject.name}");
                }
            }
        }

        private void StoreMaterialStates()
        {
            foreach (var materialChange in MaterialChanges)
            {
                if (materialChange.TargetRenderer != null)
                {
                    materialChange.OriginalMaterial = materialChange.TargetRenderer.sharedMaterials[materialChange.MaterialIndex];
                }
            }
        }

        private GameObject[] FindObjectsInLayers(LayerMask layerMask)
        {
            List<GameObject> results = new List<GameObject>();
            Scene activeScene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = activeScene.GetRootGameObjects();

            foreach (var root in rootObjects)
            {
                // Check the root object
                if (((1 << root.layer) & layerMask) != 0)
                {
                    results.Add(root);
                }

                // Check all children recursively
                foreach (Transform child in root.GetComponentsInChildren<Transform>(true)) // true includes inactive objects
                {
                    if (((1 << child.gameObject.layer) & layerMask) != 0)
                    {
                        results.Add(child.gameObject);
                    }
                }
            }

            DebugLog($"Found {results.Count} objects in specified layers");
            return results.ToArray();
        }

        private void DebugLog(string message)
        {
            if (EnableDebugLog)
            {
                Debug.Log($"[SceneAction - {gameObject.name}] {message}");
            }
        }
    }
}

/*
Implementation Steps:
1. Add this script to an empty GameObject in your scene
2. Configure the action:
   - Set LayersToActivate/LayersToDeactivate using the layer mask dropdown
   - Add specific GameObjects to ObjectsToActivate/ObjectsToDeactivate lists
   - Configure MaterialChanges by adding entries and assigning Renderers and Materials
   - Enable IsOneShot if action should only trigger once
   
3. Trigger the action:
   - Call TriggerAction() from other scripts
   - Connect to UnityEvents (e.g., from InteractionZone)
   - Use the OnActionTriggered event for additional effects
   
Example Uses:
- Level transitions
- Revealing hidden areas
- Changing environment states
- Quest objective completion effects
- Cutscene triggers

Tips:
- Use layers for bulk changes to similar objects
- Direct object references are better for specific key objects
- Keep material changes organized by purpose
- Use debug logging during setup
- Consider adding editor buttons for testing
*/