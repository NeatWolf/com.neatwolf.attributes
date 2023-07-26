using System;
using UnityEngine;

namespace NeatWolf.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class UseBoundsEditor : PropertyAttribute
    {
        public Color32 GizmoColor { get; private set; }
        public Color32 HandleColor { get; private set; }
        public Vector3 SnapValue { get; private set; }

        public UseBoundsEditor(Color32 gizmoColor, Color32 handleColor, Vector3 snapValue)
        {
            GizmoColor = gizmoColor;
            HandleColor = handleColor;
            SnapValue = snapValue;
        }

        public UseBoundsEditor()
            : this(new Color32(0, 255, 255, 127), Color.white, Vector3.zero)
        {
        }
    }
}
