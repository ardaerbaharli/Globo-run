using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static void DeleteChildrenOnly(this GameObject go)
        {
            foreach (var child in GetChildrenOnly(go))
                UnityEngine.Object.Destroy(child);
        }

        public static IEnumerable<GameObject> GetChildrenOnly(this GameObject go)
        {
            if (go != null)
            {
                foreach (Transform tr in go.transform)
                    yield return tr.gameObject;
            }
        }

        public static GameObject AddChild(this GameObject parent, GameObject prefab)
        {
            GameObject go = GameObject.Instantiate<GameObject>(prefab);
            go.transform.SetParent(parent.transform);
            return go;
        }

        public static T AddChild<T>(this GameObject parent, T prefab) where T : Component
        {
            T go = GameObject.Instantiate<T>(prefab);
            go.transform.SetParent(parent.transform);
            return go;
        }

        public static void SetParent(this GameObject go, GameObject parent)
        {
            if (go != null)
            {
                Transform t = go.transform;
                if (parent != null)
                {
                    t.SetParent(parent.transform);
                    go.layer = parent.layer;
                    ChangeChildsLayer(go.transform);
                }

                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
            }
        }

        private static void ChangeChildsLayer(Transform parent)
        {
            foreach (Transform tr in parent)
            {
                tr.gameObject.layer = parent.gameObject.layer;
                ChangeChildsLayer(tr);
            }
        }

        public static List<GameObject> GetChildObjectsWithTag(this GameObject g, string tag)
        {
            var childObjects = new List<GameObject>();
            var parent = g.transform;
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    childObjects.Add(child.gameObject);
                }

                if (child.childCount > 0)
                {
                    return GetChildObjectsWithTag(child.gameObject, tag);
                }
            }

            return childObjects;
        }

        public static GameObject GetChildObjectWithTag(this GameObject g, string tag)
        {
            var parent = g.transform;
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }

                if (child.childCount > 0)
                {
                    return GetChildObjectWithTag(child.gameObject, tag);
                }
            }

            return null;
        }
    }
}