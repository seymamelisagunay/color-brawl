using UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utility
{
    public interface ILineInfo
    {
        void Show(string message, float lifeTime = 1.5f);
        void OpenWithTranslate(string message, float lifeTime = 1.5f);
    }

    public class LineInfo : MonoBehaviour, ILineInfo
    {
        public static LineInfo Instance { get; private set; }
        private LineInfoController _prefab;

        private void Awake()
        {
            Instance = this;
        }

        public void Show(string message, float lifeTime = 1.5f)
        {
            if (_prefab == null)
                _prefab = Resources.Load<LineInfoController>("UI/LineInfoController");

            var lineInfo = Instantiate(_prefab, transform);
            lineInfo.Open(message, lifeTime);
        }

        public void OpenWithTranslate(string message, float lifeTime = 1.5f)
        {
            Show(message, lifeTime);
        }
    }
}