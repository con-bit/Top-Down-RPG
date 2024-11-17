using UnityEngine;
using Udar.SceneManager;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

/// <summary>
/// data for loading scene
/// </summary>
public class SceneLoader : MonoBehaviour
{
    // singleton sceneloader
    private static SceneLoader instance = null;

    /// <summary>
    /// assigns the first existing sceneloader to static sceneloader.
    /// </summary>
    public void Awake()
    {
        if (SceneLoader.instance == null)
        {
            SceneLoader.instance = this.GetComponent<SceneLoader>();

            SceneLoader.instance.SubscribeToSystemEvents();

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// subscribe to system events for loading scenes.
    /// </summary>
    private void SubscribeToSystemEvents()
    {
        EventManager.Instance.OnSceneTransitionStarted += UnloadLoadScene;
        EventManager.Instance.OnSceneTransitionFinalize += FinalizeScene;
        EventManager.Instance.OnSceneTransitionComplete += StartScene;
    }

    /// <summary>
    /// transitions for UI.
    ///     unload/load a scene with a sceneField.
    /// </summary>
    /// <param name="sceneToLoad"> scene as a SceneField. </param>
    private Task UnloadLoadScene(SceneField sceneToLoad)
    {
        // Darken Screen here.
        // loading screen here

        SceneManager.LoadScene(sceneToLoad.Name);

        // unallow play
        // Debug.Log("OnSceneTransitionStarted");
        return Task.CompletedTask;
    }

    /// <summary>
    /// any other work needed for after scene is loaded in.
    /// </summary>
    /// <param name="sceneToLoad"> scene as a SceneField. </param>
    private Task FinalizeScene()
    {
        // dunno
        // Debug.Log("OnSceneTransitionFinalize");
        return Task.CompletedTask;
    }

    /// <summary>
    /// complete scene transition start gameplay.
    /// </summary>
    private Task StartScene()
    {
        // Darken screen UI
        // unactive loadscreen UI
        // UnDarken Screen Here

        // allow play
        // Debug.Log("OnSceneTransitionComplete");
        return Task.CompletedTask;
    }
}
