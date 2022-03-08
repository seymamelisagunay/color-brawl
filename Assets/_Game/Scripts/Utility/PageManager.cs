using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI
{
    [DefaultExecutionOrder(-10000)]
    public class PageManager : MonoBehaviour
    {
        private Dictionary<string, IPage> _pages;
        public static PageManager Instance { get; private set; }

        protected void Awake()
        {
            Instance = this;
            CollectPages();
        }

        private void CollectPages()
        {
            _pages = new Dictionary<string, IPage>();
            var pages = GetComponentsInChildren<IPage>();
            foreach (var page in pages)
            {
                _pages.Add(page.Name, page);
                page.Hide();
            }
        }

        public T GetPage<T>(string pageName) where T : Component
        {
            return (T) GetPage(pageName);
        }

        public IPage GetPage(string pageName)
        {
            if (_pages.TryGetValue(pageName, out var page))
            {
                return page;
            }

            Debug.LogError($"Missing page!! name:{pageName}");
            return null;
        }

        public void HideAllPages()
        {
            foreach (var page in _pages)
            {
                page.Value.Hide();
            }
        }
    }
}