using UnityEngine;

public interface IState
{

    void OnEnter();
    void OnExit();
    void Tick();
    Color GizmoColor();
}