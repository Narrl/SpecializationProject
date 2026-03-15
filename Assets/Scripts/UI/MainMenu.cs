using Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is an ActionBehavior that manages the main menu UI, handling button interactions for starting the game, 
/// opening the tutorial menu, and exiting the application.
/// </summary>

public class MainMenu : ActionStack.ActionBehavior
{
    [SerializeField] private GameObject m_mainMenuUI;
    [SerializeField] private GameObject m_tutorialUI;

    private void Start()
    {
        ActionStack.Main.PushAction(this);
    }

    public override void OnBegin(bool bFirstTime)
    {
        base.OnBegin(bFirstTime);
        m_mainMenuUI.SetActive(true);
    }

    public void OnStart()
    {
        SceneManager.LoadScene("LevelScene");
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnTutorial()
    {
        m_mainMenuUI.SetActive(false);
        Canvas canvas = m_mainMenuUI.GetComponentInParent<Canvas>();
        TutorialMenu tutorialMenu = TutorialMenu.Create(m_tutorialUI, canvas.transform);
        ActionStack.Main.PushAction(tutorialMenu);
    }

    public override bool IsDone()
    {
        return false;
    }
}
