using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NeatWolf.Attributes
{
    /// <summary>
    /// Custom PropertyDrawer that changes the inspector view of fields marked with ScriptableObjectCollectionAttribute.
    /// Instead of a simple drop target for the asset, a dropdown menu is presented with all the assets located at the path specified by the attribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(ScriptableObjectCollectionAttribute))]
    public class ScriptableObjectCollectionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Overrides the GUI display of the property marked with ScriptableObjectCollectionAttribute.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for the property GUI.</param>
        /// <param name="property">The SerializedProperty to make the custom GUI for.</param>
        /// <param name="label">The label of this property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Cast the attribute to access its properties
            ScriptableObjectCollectionAttribute socAttribute = (ScriptableObjectCollectionAttribute) this.attribute;

            // Search for assets at the specified path
            var assetGUIDs = AssetDatabase.FindAssets($"t:{fieldInfo.FieldType.Name}", new[] { socAttribute.Path });

            // Prepare arrays to store the asset names and objects
            var assetNames = new string[assetGUIDs.Length];
            var assetObjects = new Object[assetGUIDs.Length];
        
            // Load each asset and store their name and reference
            for(int i=0; i<assetGUIDs.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
                assetObjects[i] = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                assetNames[i] = assetObjects[i].name;
            }

            // Get the index of the currently selected object in the assetObjects array
            int currentIndex = Array.IndexOf(assetObjects, property.objectReferenceValue);
        
            // Draw the dropdown menu in the inspector, get the selected index
            currentIndex = EditorGUI.Popup(position, label.text, currentIndex, assetNames);

            // Assign the selected object from the dropdown to the property
            try
            {
                property.objectReferenceValue = assetObjects[currentIndex];
            }
            finally{}
        }
    }
}
