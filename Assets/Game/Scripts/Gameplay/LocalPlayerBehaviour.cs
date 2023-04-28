using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons.Gameplay
{
    public class LocalPlayerBehaviour : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject raccoonObject;

        [field: SerializeField, Required]
        public Transform BallAimerPivotTransform;

        private void Awake()
        {
            // Disable the raccoon at the start of the game as we should be in the main menu
            raccoonObject.SetActive(false);
        }
    }
}