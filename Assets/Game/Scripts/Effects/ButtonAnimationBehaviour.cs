using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace JugglingRaccoons
{
    public class ButtonAnimationBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float inflationMultiplier = 1.5f;

        [SerializeField]
        private float inflationDuration = 0.3f;

        [SerializeField]
        private AnimationCurve inflationCurve;

        private bool isSelected = false;
        private Vector3 originalScale = Vector3.one;

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        public void OnSelected()
        {
            isSelected = true;
            Inflate();
        }

        public void OnDeselected()
        {
            isSelected = false;
            Deflate();
        }
        
        private async void Inflate()
        {
            var inflatedScale = originalScale * inflationMultiplier - originalScale;
            var end = Time.time + inflationDuration;
            while (Time.time < end)
            {
                if (!isSelected) return;
                float progress = 1f - ((end - Time.time) / inflationDuration);
                var inflation = progress * inflatedScale;
                transform.localScale = originalScale + inflation;
                await UniTask.Yield();
            }
        }

        private async void Deflate()
        {
            var inflatedScale = originalScale * inflationMultiplier - originalScale;
            var end = Time.time + inflationDuration;
            while (Time.time < end)
            {
                if (isSelected) return;
                float progress = (end - Time.time) / inflationDuration;
                var deflation = progress * inflatedScale;
                transform.localScale = originalScale + deflation;
                await UniTask.Yield();
            }
        }
    }
}
