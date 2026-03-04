using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public enum PreviewState
    {
        Valid,
        Invalid
    }

    [SerializeField] private Material m_validMat;
    [SerializeField] private Material m_invalidMat;

    public PreviewState State { get; private set; } = PreviewState.Invalid;
    public BuildingData Data { get; private set; }
    public BuildingModel Model { get; private set; }

    private List<Renderer> m_renderers = new List<Renderer>();
    private List<Collider> m_colliders = new List<Collider>();

    public void Setup(BuildingData data)
    {
        Data = data;
        Model = Instantiate(data.Model, transform.position, Quaternion.identity, transform);
        m_renderers.AddRange(Model.GetComponentsInChildren<Renderer>());
        m_colliders.AddRange(Model.GetComponentsInChildren<Collider>());
        foreach (var collider in m_colliders)
        {
            collider.enabled = false;
        }
        SetPreviewMaterial(State);
    }

    public void ChangeState(PreviewState newState)
    {
        if (State != newState)
        {
            State = newState;
            SetPreviewMaterial(State);
        }
    }

    public void Rotate(int angle)
    {
        Model.Rotate(angle);
    }

    private void SetPreviewMaterial(PreviewState state)
    {
        Material mat = state == PreviewState.Valid ? m_validMat : m_invalidMat;
        foreach (var renderer in m_renderers)
        {
            renderer.material = mat;
        }
    }
}
