using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VelocityShader
{
    [RequireComponent(typeof(Collider2D))]
    public class GrassExternalVelocityTrigger : MonoBehaviour
    {
        private GrassVelocityController grassVelocityController;
        private GameObject player;
        private Material material;
        private Rigidbody2D playerRB;

        private bool easeInRunning = false;
        private bool easeOutRunning = false;

        private int externalInfluence = Shader.PropertyToID("_ExternalInfluence");

        private float startingXVel;
        private float startingYVel;
        private float lastFrameVelocity;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>().gameObject;
            GetComponent<Collider2D>().isTrigger = true; // ensure isTrigger

            if (!player) Debug.LogWarning("GrassExternalVelocityTrigger couldn't find the player!");

            if (player)
                playerRB = player.GetComponent<Rigidbody2D>();

            material = GetComponent<SpriteRenderer>().material;
            grassVelocityController = GetComponentInParent<GrassVelocityController>();
            if (!grassVelocityController) Debug.LogError("GrassVelocityController not found in parent!");
            startingXVel = material.GetFloat(externalInfluence);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                if (!easeInRunning && Mathf.Abs(playerRB.velocity.x) > Mathf.Abs(grassVelocityController.VelocityThreshold))
                    StartCoroutine(EaseIn(playerRB.velocity.x * grassVelocityController.ExternalInfluence));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                StartCoroutine(EaseOut());
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                if (Mathf.Abs(lastFrameVelocity) > Mathf.Abs(grassVelocityController.VelocityThreshold) &&
                    Mathf.Abs(playerRB.velocity.x) < Mathf.Abs(grassVelocityController.VelocityThreshold))
                {
                    StartCoroutine(EaseOut());
                }

                else if (Mathf.Abs(lastFrameVelocity) < Mathf.Abs(grassVelocityController.VelocityThreshold) &&
                    Mathf.Abs(playerRB.velocity.x) > Mathf.Abs(grassVelocityController.VelocityThreshold))
                {
                    StartCoroutine(EaseIn(playerRB.velocity.x * grassVelocityController.ExternalInfluence));
                }
                else if (!easeInRunning && !easeOutRunning &&
                    Mathf.Abs(playerRB.velocity.x) < Mathf.Abs(grassVelocityController.VelocityThreshold))
                {
                    grassVelocityController.InfluenceGrass(material, playerRB.velocity.x * grassVelocityController.ExternalInfluence);
                }

                lastFrameVelocity = playerRB.velocity.x;
            }
        }

        private IEnumerator EaseIn(float _xVelocity)
        {
            easeInRunning = true;

            float elapsed = 0f;
            while (elapsed < grassVelocityController.EaseInTime)
            {
                elapsed += Time.deltaTime;
                float lerped = Mathf.Lerp(startingXVel, _xVelocity, (elapsed / grassVelocityController.EaseInTime));
                //Debug.Log($"Ease in lerp: {lerped}, startingXVel: {startingXVel}, curr XVel: {_xVelocity}");
                grassVelocityController.InfluenceGrass(material, lerped);

                yield return null;
            }

            easeInRunning = false;
        }

        private IEnumerator EaseOut()
        {
            easeOutRunning = true;

            float currentX = material.GetFloat(externalInfluence);
            float elapsed = 0f;
            while (elapsed < grassVelocityController.EaseOutTime)
            {
                elapsed += Time.deltaTime;
                float lerped = Mathf.Lerp(currentX, startingXVel, (elapsed / grassVelocityController.EaseOutTime));
                grassVelocityController.InfluenceGrass(material, lerped);

                yield return null;
            }

            easeOutRunning = false;
        }
    }
}
