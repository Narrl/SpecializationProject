using UnityEngine;
using Actions;
using Unity.VisualScripting;

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
