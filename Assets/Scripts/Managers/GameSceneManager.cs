using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MAIN_MENU,
    STAGE_ONE,
    STAGE_TWO,
    STAGE_THREE,
    TEST_LEVEL_D,
    TEST_LEVEL_B
}
public class GameSceneManager : MonoBehaviour
{
    // Singleton instance
    private static GameSceneManager instance;

    [SerializeField] private Animator animator;
    [SerializeField] private string defaultSceneTarget;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameSceneManager GetInstance()
    {
        return instance;
    }

    // Go to scene
    public void GotoScene(SceneName scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene, 0));
    }
    public void GotoScene(string scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene, 0));
    }
    public void GotoScene()
    {
        StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, 0));
    }

    // Go to scene with delay
    public void GotoSceneWithDelay(SceneName scene, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(scene, delay));
    }
    public void GotoSceneWithDelay(string scene, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(scene, delay));
    }
    public void GotoSceneWithDelay(float delay)
    {
        StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, delay));
    }

    // Reload scene
    public void ReloadScene()
    {
        StartCoroutine(LoadSceneCoroutine(GetCurrentScene().name, 0));
    }

    // Load scene coroutine
    private IEnumerator LoadSceneCoroutine(SceneName scene, float delay)
    {
        // Reset Timescale now (IMPORTANT: Coroutines wont run if timescale = 0)
        Time.timeScale = 1f;

        if (GameManager.GetInstance())
            GameManager.GetInstance().SceneChange(delay);

        yield return new WaitForSeconds(delay);

        // Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync((int)scene);

    }
    private IEnumerator LoadSceneCoroutine(string scene, float delay)
    {
        // Reset Timescale now (IMPORTANT: Coroutines wont run if timescale = 0)
        Time.timeScale = 1f;

        if (GameManager.GetInstance())
            GameManager.GetInstance().SceneChange(delay);

        yield return new WaitForSeconds(delay);

        // Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(scene);
    }

    // Get current scene
    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }
}
