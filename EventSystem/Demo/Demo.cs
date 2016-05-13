using UnityEngine;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Demo
{
  public class Demo : MonoBehaviour
  {
    private void Start()
    {
      EventManager.Instance.AddListener<GameEvent>(OnEvent);
      Invoke("TriggerEvent", 2.0f);
    }

    public void OnEvent(GameEvent evt)
    {
      Debug.Log("On Event!");
    }

    public void TriggerEvent()
    {
      EventManager.Instance.QueueEvent(new GameEvent());
    }
  }
}