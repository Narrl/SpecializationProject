using UnityEngine;
using Actions;
using UnityEngine.SceneManagement;

/// <summary>
/// This is an ActionBehavior that manages the pause menu state, 
/// handling player input for resuming the game or going back to the main menu. 
/// It also pauses the game time while active.
/// </summary>

public class PauseMenu : ActionStack.ActionBehavior
{
    private bool m_bIsDone = false;

    public override bool IsDone()
    {
        return m_bIsDone;
    }

    public void OnResume()
    {
        m_bIsDone = true;
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        m_bIsDone = true;
    }

    public override void OnBegin(bool bFirstTime)
    {
        base.OnBegin(bFirstTime);
        Time.timeScale = 0f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
            m_bIsDone = true;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public static PauseMenu Create(GameObject pauseMenuGO, Transform parent)
    {
        GameObject go = Instantiate(pauseMenuGO, parent);
        return go.GetComponent<PauseMenu>();
    }
}
