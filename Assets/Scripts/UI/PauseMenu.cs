using UnityEngine;
using Actions;
using UnityEngine.SceneManagement;

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
