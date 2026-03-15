using UnityEngine;
using Actions;

/// <summary>
/// This is an ActionBehavior that manages the tutorial menu, it only has functionality for the back button,
/// which closes the menu when pressed.
/// </summary>

public class TutorialMenu : ActionStack.ActionBehavior
{
    private bool m_bIsDone = false;

    public override bool IsDone()
    {
        return m_bIsDone;
    }

    public void OnBack()
    {
        m_bIsDone = true;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        Destroy(gameObject);
    }

    public static TutorialMenu Create(GameObject tutorialMenuGO, Transform parent)
    {
        GameObject go = Instantiate(tutorialMenuGO, parent);
        return go.GetComponent<TutorialMenu>();
    }
}
