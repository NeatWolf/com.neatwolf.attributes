using UnityEngine;

namespace NeatWolf.Attributes
{
    /// <summary>
    /// A custom attribute to specify a collection of ScriptableObject assets from a certain path.
    /// This attribute can be used in conjunction with a custom PropertyDrawer to show a dropdown
    /// of ScriptableObjects in the Unity editor, loaded from the specified path.
    /// </summary>
    public class ScriptableObjectCollectionAttribute : PropertyAttribute
    {
        /// <summary>
        /// Gets the path in the Unity project where the ScriptableObject assets are located.
        /// This path is used by the custom PropertyDrawer to load the available assets.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Construct a new ScriptableObjectCollectionAttribute.
        /// </summary>
        /// <param name="path">The path in the Unity project where the ScriptableObject assets are located.</param>
        public ScriptableObjectCollectionAttribute(string path)
        {
            //Assign the provided path to the Path property.
            this.Path = path;
        }
    }
}