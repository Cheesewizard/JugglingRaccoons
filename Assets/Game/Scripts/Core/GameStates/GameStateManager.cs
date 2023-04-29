using System.Collections.Generic;
using UnityEngine;

namespace JugglingRaccoons.Core.GameStates
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        [SerializeField]
        private AbstractGameState startingAbstractGameState;

        private AbstractGameState _currentAbstractGameState;
        private List<AbstractGameState> gameStates = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(this);

            // Get all the states and disable them by default
            for (int i = 0; i < transform.childCount; i++)
            {
                var childObject = transform.GetChild(i).gameObject;
                var childGameState = childObject.GetComponent<AbstractGameState>();
                if (childGameState != null)
                {
                    gameStates.Add(childGameState);
                    childObject.SetActive(false);
                }
            }
            _currentAbstractGameState = startingAbstractGameState;
        }

        private void Start()
        {
            // Activate the currentState
            _currentAbstractGameState.gameObject.SetActive(true);
        }

        public void ChangeGameState(AbstractGameState state)
        {
            _currentAbstractGameState.gameObject.SetActive(false);
            state.gameObject.SetActive(true);
            _currentAbstractGameState = state;
        }
    }
}