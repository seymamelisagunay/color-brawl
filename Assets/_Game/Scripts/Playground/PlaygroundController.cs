using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Playground
{
    public class PlaygroundController : MonoBehaviour
    {
        public int height;
        public int width;
        public bool[,] Map;
        public Unit unitPrefab;
        public Transform parent;
        public List<Unit> units;

        [ContextMenu("Create Borders")]
        public void CreateBorders()
        {
            units = new List<Unit>();
            parent.localPosition = new Vector3(-width / 2f + 0.5f, -height / 2f + 0.5f);

            var thickness = 2;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (x < thickness || (width - x) <= thickness
                                      || y < thickness || (height - y) <= thickness)
                    {
                        var unit = Instantiate(unitPrefab, parent);
                        unit.transform.localPosition = new Vector3(x, y, 0);
                    }
                }
            }

            CalculateStates();
            Debug.LogWarning($"{name} borders created");
        }

        [ContextMenu("Calculate State")]
        public void CalculateStates()
        {
            Map = new bool[width, height];
            units = gameObject.GetComponentsInChildren<Unit>().ToList();
            units.ForEach(x => x.Init(this));
            units.ForEach(x => x.SetState());
            Debug.LogWarning($"{name} state calculated");
        }

        [ContextMenu("Update Items")]
        public void UpdateItems()
        {
            Map = new bool[width, height];
            for (var i = 0; i < units.Count; i++)
            {
                var oldUnit = units[i];
                if (oldUnit == null)
                {
                    units.RemoveAt(i);
                    i--;
                    continue;
                }

                var unit = Instantiate(unitPrefab, parent);
                unit.transform.localPosition = oldUnit.transform.localPosition;
                unit.Disable = oldUnit.Disable;
                unit.Init(this);
                units[i] = unit;
                DestroyImmediate(oldUnit.gameObject);
            }

            units.ForEach(x => x.SetState());
            Debug.LogWarning($"{name} items updated");
        }

        [ContextMenu("Destroy All Units")]
        public void DestroyAllUnits()
        {
            units.ForEach(x => DestroyImmediate(x.gameObject));
            units.Clear();
            Debug.LogWarning($"{name} destroyed");
        }
    }
}