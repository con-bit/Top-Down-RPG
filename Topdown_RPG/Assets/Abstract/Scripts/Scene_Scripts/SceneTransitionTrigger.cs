using Udar.SceneManager;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("NEED SCENE IN BUILD SETTINGS")]
    [SerializeField] private SceneField NextScene;

    /// <summary>
    /// transitions to next scene when player steps into the polygon collider.
    /// <paramref name="collision"/> any collision specifically focuses on gameobjects with Player tag. <para>
    /// </summary>
    private void TransitionToSceneUponPlayerCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.Instance.TransitionToScene(NextScene);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(NextScene.Name);
        TransitionToSceneUponPlayerCollision(collision);
    }
}
