using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="sceneToLoad">要加载的场景</param>
    /// <param name="positionToGo">要传送到的位置</param>
    /// <param name="fadeScreen">是否淡入淡出</param>
    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestEvent;
    public void RaiseLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 positionToGo, bool fadeScreen){
        LoadRequestEvent?.Invoke(sceneToLoad, positionToGo, fadeScreen);
    }
}
