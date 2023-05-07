using System;
using System.Collections;
using System.Collections.Generic;
using JugglingRaccoons.Core.GameStates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JugglingRaccoons
{
    public class TitleWithRopesBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform title;
        
        [SerializeField]
        private Transform leftRope;

        [SerializeField]
        private Transform rightRope;
        
        private Dictionary<Transform, PositionAndRotation> originalTransforms = new();

        private struct PositionAndRotation
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }
        
        private void Awake()
        {
            MainMenuState.OnMainMenuStateEntered += ResetTransformPositions;
            
            // Record the original positions and rotations of all the transforms
            RecordPositionAndRotation(title);
            foreach (Transform child in leftRope)
            {
                RecordPositionAndRotation(child);
            }
            foreach (Transform child in rightRope)
            {
                RecordPositionAndRotation(child);
            }

            void RecordPositionAndRotation(Transform currentTransform)
            {
                originalTransforms.Add(currentTransform, new PositionAndRotation
                {
                    Position = currentTransform.localPosition,
                    Rotation = currentTransform.localRotation
                });
            }
        }

        [Button]
        private void ResetTransformPositions()
        {
            // Reset all the transforms that were recorded
            foreach (var keyValuePair in originalTransforms)
            {
                var currentTransform = keyValuePair.Key;
                PositionAndRotation positionAndRotation = originalTransforms[currentTransform];
                currentTransform.localPosition = positionAndRotation.Position;
                currentTransform.localRotation = positionAndRotation.Rotation;
            }
        }

        private void OnDestroy()
        {
            MainMenuState.OnMainMenuStateEntered -= ResetTransformPositions;
            originalTransforms.Clear();
        }
    }
}
