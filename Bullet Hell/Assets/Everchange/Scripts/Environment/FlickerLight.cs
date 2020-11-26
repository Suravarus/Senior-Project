using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.Environment
{
    class FlickerLight: MonoBehaviour
    {
        public float flickerPerSeconds = 0f;
        private float lastFlickerTime = 0f;
        private float originalInnerRadius;
        public float flickerAmount = 0f;
        private bool flickerToggle = false;
        public Light2D light2D;
        void Update()
        {
            if (lastFlickerTime >= 1 / flickerPerSeconds)
            {
                lastFlickerTime = 0f;
                Flicker();
            } else
            {
                lastFlickerTime += Time.deltaTime;
            }
        }

        void Flicker()
        {
            if (!flickerToggle)
            {
                this.light2D.pointLightInnerRadius -= this.flickerAmount;
            } else
            {
                this.light2D.pointLightInnerRadius = this.originalInnerRadius;
            }
            flickerToggle = !flickerToggle;
        }
    }
}
