using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO sceneLoadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("Teleporting player to another point!");
        sceneLoadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }
}
