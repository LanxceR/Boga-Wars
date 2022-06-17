using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pausePanel;

    [Header("UI Elements")]
    [SerializeField] private Transform pausedText;
    private UIMoveable pausedTextMove;
    private Vector2 activePausedTextPos, inactivePausedTextPos;
    [SerializeField] private Transform pausedButtonsGroup;
    private UIMoveable pausedButtonsGroupMove;
    private Vector2 activePausedButtonGroupPos, inactivePausedButtonGroupPos;
    [SerializeField] private Image pauseBackground;
    private UIFade pauseBackgroundFade;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe methods to game manager
        GameManager.GetInstance().OnPauseAction += PauseGame;
        GameManager.GetInstance().OnResumeAction += ResumeGame;

        // Fetch pausedText elements
        pausedText.gameObject.TryGetComponent<UIMoveable>(out pausedTextMove);
        activePausedTextPos = pausedText.localPosition;
        inactivePausedTextPos = new Vector2(0, 670);
        pausedTextMove.SetPosition(inactivePausedTextPos);

        // Fetch pausedButtonsGroup elements
        pausedButtonsGroup.gameObject.TryGetComponent<UIMoveable>(out pausedButtonsGroupMove);
        activePausedButtonGroupPos = pausedButtonsGroup.localPosition;
        inactivePausedButtonGroupPos = new Vector2(0, -717);
        pausedButtonsGroupMove.SetPosition(inactivePausedButtonGroupPos);

        // Fetch pauseBackground elements
        pauseBackground.gameObject.TryGetComponent<UIFade>(out pauseBackgroundFade);
        pauseBackgroundFade.SetAlpha(0f);

        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);

        if (pausedText)
        {
            pausedTextMove.MoveTo(activePausedTextPos, 0.5f);
        }

        if (pausedButtonsGroup)
        {
            pausedButtonsGroupMove.MoveTo(activePausedButtonGroupPos, 0.5f);
        }

        if (pauseBackground)
        {
            pauseBackgroundFade.DoFadeIn(0.5f);
        }
    }
    private void ResumeGame()
    {
        if (pausedText)
        {
            pausedTextMove.MoveTo(inactivePausedTextPos, 0.5f);
        }

        if (pausedButtonsGroup)
        {
            pausedButtonsGroupMove.MoveTo(inactivePausedButtonGroupPos, 0.5f);
        }

        if (pauseBackground)
        {
            pauseBackgroundFade.DoFadeOut(0.5f);
        }
    }
    private void PauseAndResumeGame()
    {
        if (GameManager.GetInstance().IsPlaying)
        {
            // Pause the game
            PauseGame();
        }
        else
        {
            // Resume the game
            ResumeGame();
        }
    }

    public void ResumeButton()
    {
        ResumeGame();
        GameManager.GetInstance().ResumeGame();
    }
    public void RedoStageButton()
    {
        GameSceneManager.GetInstance().ReloadScene();
    }
    public void RestartButton()
    {
        GameSceneManager.GetInstance().GotoScene(SceneName.STAGE_ONE);
    }
    public void ReturnToMenuButton()
    {
        GameSceneManager.GetInstance().GotoScene(SceneName.MAIN_MENU);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
