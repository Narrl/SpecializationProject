using Actions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
