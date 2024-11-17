using Udar.SceneManager;
using UnityEngine.Events;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
public class EventManager : MonoBehaviour
{
    private static EventManager instance = null;

    public static EventManager Instance
    {
        get
        {
            if (EventManager.instance == null)
            {
                EventManager.instance = new GameObject("EventManager").AddComponent<EventManager>();

                DontDestroyOnLoad(EventManager.instance.gameObject);
            }

            return instance;
        }
    }

    // used to house events that are async that return Task with two parameters
    public delegate Task AsyncUnityAction<T0, T1>(T0 ob1, T1 ob2);

    // used to house events that are async that return Task with one parameter
    public delegate Task AsyncUnityAction<T0>(T0 ob1);

    // used to house events that are async that return Task
    public delegate Task AsyncUnityAction();

    /// <summary>
    /// handle start of transition to scene.
    /// return type for subscribers must be => Task.
    ///     Include: using System.Threading.Tasks;
    /// </summary>
    public event AsyncUnityAction<SceneField> OnSceneTransitionStarted;

    /// <summary>
    /// handle finalizization of a transition to another scene.
    /// return type for subscribers must be => Task.
    ///     Include: using System.Threading.Tasks;
    /// </summary>
    public event AsyncUnityAction OnSceneTransitionFinalize;

    /// <summary>
    /// handle transition to scene completed.
    /// return type for subscribers must be => Task.
    ///     Include: using System.Threading.Tasks;
    /// </summary>
    public event AsyncUnityAction OnSceneTransitionComplete;

    // handle a game has been paused
    public event UnityAction<bool> OnPausedGame;

    // handle a game save trigger
    public event UnityAction OnSaveGame;

    // handle a game load has trigger
    public event UnityAction OnLoadGame;

    // handle player death trigger
    public event UnityAction OnPlayerDeath;

    // handle player locked in room trigger (may need specific room id or something later)
    public event UnityAction OnPlayerLockedIn;

    // handle room cleared trigger (may need specific room id or something later)
    public event UnityAction OnPlayerClearedRoom;

    // handle completed state trigger (may need specific state id or something later) 
    //     state meaning specific completed part of the game so they dont revert back to normal.
    public event UnityAction OnStateComplete;

    /// <summary>
    /// broadcasts to all listeners for each event asynchronously (in order)
    ///     Order: OnSceneTransitionStarted, OnSceneTransitionFinalize, OnSceneTransitionComplete
    /// </summary>
    /// <param name="sceneToLoad"> scene to load. </param>
    public async void TransitionToScene(SceneField sceneToLoad)
    {
        // asynchrously wait for each SceneTransitionStarted subscriber to be called before moving to the next event for scene transition
        if (OnSceneTransitionStarted != null)
        {
            List<Task> sceneTransitionStartedTasks = new List<Task>();

            foreach (Delegate subscriber in OnSceneTransitionStarted.GetInvocationList())
            {
                sceneTransitionStartedTasks.Add((subscriber.DynamicInvoke(new object[] { sceneToLoad }) as Task));
            }

            await Task.WhenAll(sceneTransitionStartedTasks);
        }

        // asynchrously wait for each SceneTransitionStarted subscriber to be called before moving to the next event for scene transition
        if (OnSceneTransitionFinalize != null)
        {
            List<Task> sceneTransitionFinalizeTasks = new List<Task>();

            foreach (Delegate subscriber in OnSceneTransitionFinalize.GetInvocationList())
            {
                sceneTransitionFinalizeTasks.Add((subscriber.DynamicInvoke(new object[] { }) as Task));
            }

            await Task.WhenAll(sceneTransitionFinalizeTasks);
        }

        // asynchrously wait for each SceneTransitionComplete subscriber to be called
        if (OnSceneTransitionComplete != null)
        {
            List<Task> sceneTransitionCompleteTasks = new List<Task>();

            foreach (Delegate subscriber in OnSceneTransitionComplete.GetInvocationList())
            {
                sceneTransitionCompleteTasks.Add((subscriber.DynamicInvoke(new object[] { }) as Task));
            }

            await Task.WhenAll(sceneTransitionCompleteTasks);
        }
    }

    /// <summary>
    /// broadcast to listeners that game has been paused or unpaused.
    /// </summary>
    /// <param name="isGamePaused"> true when game is paused. false when game is unpaused. </param>
    public void PauseGame(bool isGamePaused) => OnPausedGame?.Invoke(isGamePaused);

    /// <summary>
    /// broadcast to listeners save game has been triggered.
    /// </summary>
    public void SaveGame() => OnSaveGame?.Invoke();

    /// <summary>
    /// broadcast to listeners that load game has been triggerd.
    /// </summary>
    public void LoadGame() => OnLoadGame?.Invoke();

    /// <summary>
    /// broadcast to listeners that player has died.
    /// </summary>
    public void PlayerDied() => OnPlayerDeath?.Invoke();

    /// <summary>
    /// broadcast to listeners that room has been locked. (may need specific room id or something later)
    /// </summary>
    public void LockInPlayer() => OnPlayerLockedIn?.Invoke();

    /// <summary>
    /// broadcast to listeners that room has been cleared. (may need specific room id or something later)
    /// </summary>
    public void PlayerClearedRoom() => OnPlayerClearedRoom?.Invoke();

    /// <summary>
    /// broadcast to listeners that State has been complete. (may need specific state id or something later)
    /// </summary>
    public void StateCompleted() => OnStateComplete?.Invoke();
}
