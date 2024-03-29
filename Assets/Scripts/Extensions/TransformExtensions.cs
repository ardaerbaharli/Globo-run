using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 SetPosX(this Transform self, float x)
        {
            Vector3 pos = self.position;
            pos.x = x;
            self.position = pos;
            return pos;
        }

        public static Vector3 SetPosY(this Transform self, float y)
        {
            Vector3 pos = self.position;
            pos.y = y;
            self.position = pos;
            return pos;
        }

        public static Vector3 SetPosZ(this Transform self, float z)
        {
            Vector3 pos = self.position;
            pos.z = z;
            self.position = pos;
            return pos;
        }

        public static Vector3 SetPosXY(this Transform self, float x, float y)
        {
            Vector3 pos = self.position;
            pos.x = x;
            pos.y = y;
            self.position = pos;
            return pos;
        }

        public static Vector3 SetPosXYZ(this Transform self, float x, float y, float z)
        {
            Vector3 pos = self.position;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            self.position = pos;
            return pos;
        }

        public static Vector3 SetScaleX(this Transform self, float x)
        {
            Vector3 scale = self.localScale;
            scale.x = x;
            self.localScale = scale;
            return scale;
        }

        public static Vector3 SetScaleY(this Transform self, float y)
        {
            Vector3 scale = self.localScale;
            scale.y = y;
            self.localScale = scale;
            return scale;
        }

        public static Vector3 SetScaleZ(this Transform self, float z)
        {
            Vector3 scale = self.localScale;
            scale.z = z;
            self.localScale = scale;
            return scale;
        }

        public static Vector3 SetScaleXY(this Transform self, float x, float y)
        {
            Vector3 scale = self.localScale;
            scale.x = x;
            scale.y = y;
            self.localScale = scale;
            return scale;
        }

        public static Vector3 SetScaleXYZ(this Transform self, float x, float y, float z)
        {
            Vector3 scale = self.localScale;
            scale.x = x;
            scale.y = y;
            scale.z = z;
            self.localScale = scale;
            return scale;
        }

        public static void ResetTransformation(this Transform self)
        {
            self.position = Vector3.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = new Vector3(1, 1, 1);
        }

        public static List<GameObject> GetChildObjectsWithTag(this Transform parent, string tag)
        {
            var childObjects = new List<GameObject>();
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    childObjects.Add(child.gameObject);
                }

                if (child.childCount > 0)
                {
                    childObjects.AddRange(GetChildObjectsWithTag(child, tag));
                }
            }

            return childObjects;
        }

        public static GameObject GetChildObjectWithTag(this Transform parent, string tag)
        {
            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.CompareTag(tag))
                {
                    return child.gameObject;
                }

                if (child.childCount > 0)
                {
                    return GetChildObjectWithTag(child, tag);
                }
            }

            return null;
        }
    }
}