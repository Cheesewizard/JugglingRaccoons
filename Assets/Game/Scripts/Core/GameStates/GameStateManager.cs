using System.Collections.Generic;
using UnityEngine;

namespace JugglingRaccoons.Core.GameStates
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        [SerializeField]
        private GameState startingGameState;

        private GameState currentGameState;
        private List<GameState> gameStates = new();

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
                var childGameState = childObject.GetComponent<GameState>();
                if (childGameState != null)
                {
                    gameStates.Add(childGameState);
                    childObject.SetActive(false);
                }
            }
            currentGameState = startingGameState;
        }

        private void Start()
        {
            // Activate the currentState
            currentGameState.gameObject.SetActive(true);
        }

        public void ChangeGameState(GameState state)
        {
            currentGameState.gameObject.SetActive(false);
            state.gameObject.SetActive(true);
            currentGameState = state;
        }
    }
}