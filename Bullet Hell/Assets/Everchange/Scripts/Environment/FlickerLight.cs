using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Environment
{
    [RequireComponent(typeof(Light2D))]
    class FlickerLight: MonoBehaviour
    {
        public float flickerPerSeconds = 0f;
        private float lastFlickerTime = 0f;
        private float originalInnerRadius;
        private float originalOuterRadius;
        public float flickerAmount = 0f;
        private bool flickerToggle = false;
        public Light2D light2D;

        void Awake()
        {
            this.originalInnerRadius = this.light2D.pointLightInnerRadius;
            this.originalOuterRadius = this.light2D.pointLightOuterRadius;
        }
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
            var fi = Random.Range(
                    this.originalInnerRadius - flickerAmount,
                    this.originalInnerRadius);
            var fo = Random.Range(
                    this.originalOuterRadius - flickerAmount,
                    this.originalOuterRadius);
            this.light2D.pointLightInnerRadius = fi;
            this.light2D.pointLightOuterRadius = fo;
            flickerToggle = !flickerToggle;
        }
    }
}
