using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VelocityShader
{

    public class GrassVelocityController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float externalInfluenceStrength = 0.25f;
        [SerializeField] float easeInTime = 0.15f;
        [SerializeField] float easeOutTime = 0.15f;
        [SerializeField] float velocityThreshold = 5f;

        private int externalInfluence = Shader.PropertyToID("_ExternalInfluence");

        public float EaseInTime
        {
            get { return easeInTime; }
        }

        public float EaseOutTime
        {
            get { return easeOutTime; }
        }

        public float ExternalInfluence
        {
            get { return externalInfluenceStrength; }
        }

        public float VelocityThreshold
        {
            get { return velocityThreshold; }
        }

        public void InfluenceGrass(Material _mat, float _xVelocity)
        {
            _mat.SetFloat(externalInfluence, _xVelocity);
        }
    }
}
