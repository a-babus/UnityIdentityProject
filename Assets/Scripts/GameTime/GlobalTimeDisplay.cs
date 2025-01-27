using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace NTGD124
{
    // GlobalTimeDisplay: Displays game time using both text and a slider
    public class GlobalTimeDisplay : MonoBehaviour
    {
        ///// Component Description /////
        [TextArea(3, 10)]
        [SerializeField] private string _componentDescription = "Displays the current game time using both text (minutes:seconds format) and a slider. Works with any start/end time configuration.";


        ///// Public Variables/Editor Properties /////
        [Header("Timer Reference")]
        [Tooltip("Reference to the GameTime component")]
        public GameTime GameTimeComponent;

        [Header("Display Elements")]
        [Tooltip("TextMeshPro component for showing time")]
        public TextMeshProUGUI TimeText;

        [Tooltip("Slider component for visual time representation")]
        public Slider TimeSlider;


        ///// Unity Methods /////
        private void Start()
        {
            InitializeDisplay();
        }

        private void Update()
        {
            UpdateDisplay();
        }


        ///// Action Methods /////
        private void InitializeDisplay()
        {
            if (TimeSlider != null)
            {
                if (GameTimeComponent.IsCountdown())
                {
                    TimeSlider.minValue = GameTimeComponent.EndTime;
                    TimeSlider.maxValue = GameTimeComponent.StartTime;
                    TimeSlider.value = GameTimeComponent.StartTime;
                }
                else
                {
                    TimeSlider.minValue = GameTimeComponent.StartTime;
                    TimeSlider.maxValue = GameTimeComponent.EndTime;
                    TimeSlider.value = GameTimeComponent.StartTime;
                }
            }
        }

        private void UpdateDisplay()
        {
            if (!GameTimeComponent) return;

            // Update text display
            if (TimeText != null)
            {
                int minutes = Mathf.FloorToInt(GameTimeComponent.CurrentTime / 60);
                int seconds = Mathf.FloorToInt(GameTimeComponent.CurrentTime % 60);
                TimeText.text = $"{minutes:00}:{seconds:00}";
            }

            // Update slider display
            if (TimeSlider != null)
            {
                TimeSlider.value = GameTimeComponent.CurrentTime;
            }
        }
    }
}

// Implementation Steps:
// 1. Make sure you have a GameTime component set up in your scene
// 2. Create UI elements:
//    - TextMeshPro text for time display
//    - Slider for visual representation
// 3. Add this GlobalTimeDisplay component to a GameObject
// 4. Connect in inspector:
//    - Drag GameTime component reference
//    - Drag TextMeshPro reference
//    - Drag Slider reference
// 5. The slider will automatically:
//    - For countdown: start at StartTime and decrease to EndTime
//    - For normal: start at StartTime and increase to EndTime