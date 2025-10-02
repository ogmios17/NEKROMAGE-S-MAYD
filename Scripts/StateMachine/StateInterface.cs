using UnityEngine;

public interface StateInterface
{
    void OnEnter();
    void OnExit();
    void Update();
    void FixedUpdate();
}
