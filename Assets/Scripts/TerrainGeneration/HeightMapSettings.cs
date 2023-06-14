using UnityEngine;
using System.Collections;


namespace Terrain_Generation
{
    [CreateAssetMenu()]
    public class HeightMapSettings : UpdatableData
    {

        public NoiseSettings noise_settings;

        public bool use_falloff;

        public float height_multiplier;
        public AnimationCurve height_curve;

        public float minHeight
        {
            get
            {
                return height_multiplier * height_curve.Evaluate(0);
            }
        }

        public float maxHeight
        {
            get
            {
                return height_multiplier * height_curve.Evaluate(1);
            }
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            noise_settings.ValidateValues();
            base.OnValidate();
        }
#endif

    }
}

