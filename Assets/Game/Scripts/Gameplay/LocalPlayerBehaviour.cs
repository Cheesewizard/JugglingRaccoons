using JugglingRaccoons.Gameplay.Aiming;
using JugglingRaccoons.Gameplay.Balancing;
using JugglingRaccoons.Gameplay.Juggling;
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

        [field: SerializeField, Required]
        public JugglingBehaviour JugglingBehaviour { get; private set; }

        [field: SerializeField, Required]
        public ShootingBehaviour ShootingBehaviour { get; private set; }

        [field: SerializeField, Required]
        public BalancingBehaviour BalancingBehaviour { get; private set; }

        [SerializeField]
        private bool disableInAwake = true;

        private void Awake()
        {
            if (!disableInAwake) return;

            raccoonObject.SetActive(false);
        }
    }
}