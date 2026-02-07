using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Infrastructure.Editor
{
    public sealed class StringListSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private IEnumerable<string> _listItems;
        private Action<string> _callback;
        private bool _withSplit;
        
        public void Construct(IEnumerable<string> items, Action<string> callback, bool withSplit = true)
        {
            _listItems = items;
            _callback = callback;
            _withSplit = withSplit;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var list = new List<SearchTreeEntry>() {new SearchTreeGroupEntry(new GUIContent("List"), 0)};

            List<string> sortedItems = new List<string>() {string.Empty};
            sortedItems.AddRange(_listItems.ToList());
            if (_withSplit)
            {
                SplitList(sortedItems, list);
            }
            else
            {
                for (int i = 0; i < sortedItems.Count; i++)
                {
                    SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(sortedItems[i]))
                    {
                        level = 1, userData = sortedItems[i]
                    };
                    list.Add(entry);
                }
            }
            

            return list;
        }

        private static void SplitList(List<string> sortedItems, List<SearchTreeEntry> list)
        {
            sortedItems.Sort((a, b) =>
            {
                string[] splits1 = a.Split('/');
                string[] splits2 = b.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                    {
                        return 1;
                    }

                    int comparison = string.CompareOrdinal(splits1[i], splits2[i]);
                    if (comparison != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            return splits1.Length < splits2.Length ? 1 : -1;
                        }

                        return comparison;
                    }
                }

                return 0;
            });

            List<string> groups = new List<string>();
            for (int i = 0; i < sortedItems.Count; i++)
            {
                string[] entryTitle = sortedItems[i].Split('/');
                string groupName = string.Empty;
                for (int j = 0; j < entryTitle.Length - 1; j++)
                {
                    groupName += entryTitle[j];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[j]), j + 1));
                        groups.Add(groupName);
                    }

                    groupName += "/";
                }

                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()))
                {
                    level = entryTitle.Length, userData = sortedItems[i]
                };
                list.Add(entry);
            }
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            _callback?.Invoke((string) searchTreeEntry.userData);
            return true;
        }
    }
}