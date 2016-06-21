using Zephyr.MonoBehaviours;

namespace Zephyr.EventSystem.Core
{
    public class EventManager : AdvancedMonoBehaviour
    {
        public bool LimitQueueProcesing = false;
        public float QueueProcessTime = 0.0f;

        private static EventManagerModel _instance;


        public static EventManagerModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventManagerModel();

                return _instance;
            }
        }

        public void OnApplicationQuit()
        {
            if (_instance == null) return;

            _instance.ClearAll();
            _instance = null;
        }

        private void Update() { Instance.ProcessEvents(); }
    }
}