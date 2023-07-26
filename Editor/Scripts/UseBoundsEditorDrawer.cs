using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NeatWolf.Attributes
{
    [CustomPropertyDrawer(typeof(UseBoundsEditor))]
    public class UseBoundsEditorDrawer : PropertyDrawer
    {
        private Transform cachedTransform;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Bounds)
                throw new Exception("[UseBoundsEditor] can only be used with Bounds");

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(property.serializedObject.targetObject, "Modified Bounds");
            Bounds bounds = EditorGUI.BoundsField(position, label, property.boundsValue);

            if (EditorGUI.EndChangeCheck())
            {
                property.boundsValue = bounds;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }

            UseBoundsEditor attributeBoundsEditor = (UseBoundsEditor)this.attribute;
            Transform transform = GetTransform(property.serializedObject.targetObject);
            DrawHandles(transform, ref bounds, attributeBoundsEditor.HandleColor, attributeBoundsEditor.SnapValue);
            DrawWireCubeGizmo(bounds, transform, attributeBoundsEditor.GizmoColor);
        }

        private void DrawHandles(Transform transform, ref Bounds bounds, Color handleColor, Vector3 snapValue)
        {
            Vector3[] directions = { Vector3.right, Vector3.up, Vector3.forward, Vector3.left, Vector3.down, Vector3.back };

            Handles.color = handleColor;
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 direction = directions[i];
                Vector3 handlePos = transform.position + transform.rotation * (bounds.center + Vector3.Scale(direction, bounds.size) / 2);

                Vector3 newHandlePos = Handles.FreeMoveHandle(handlePos, HandleUtility.GetHandleSize(handlePos) * 0.1f, snapValue, Handles.DotHandleCap);

                Vector3 diff = (newHandlePos - handlePos) * 0.5f;
                Vector3 localDiff = Quaternion.Inverse(transform.rotation) * diff;

                if (i < 3)
                {
                    bounds.size += direction * (2 * Vector3.Dot(localDiff, direction));
                    bounds.size = Vector3.Max(Vector3.zero, bounds.size);
                }
                else
                {
                    bounds.size -= direction * (2 * Vector3.Dot(localDiff, direction));
                    bounds.size = Vector3.Max(Vector3.zero, bounds.size);
                }

                bounds.center += Vector3.Dot(localDiff, direction) * direction;
            }
        }

        private static void DrawWireCubeGizmo(Bounds bounds, Transform transform, Color gizmoColor)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position + bounds.center, transform.rotation, transform.localScale);
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(Vector3.zero, bounds.size);
        }

        private Transform GetTransform(Object targetObject)
        {
            if (cachedTransform == null)
            {
                cachedTransform = (targetObject as MonoBehaviour)?.transform;
                var serializedObject = new SerializedObject(targetObject);
                while (cachedTransform == null && serializedObject.targetObject != null)
                {
                    cachedTransform = (serializedObject.targetObject as MonoBehaviour)?.transform;
                    serializedObject = new SerializedObject(serializedObject.targetObject);
                }
            }

            return cachedTransform;
        }
    }
}
