using UnityEditor;
using UnityEngine;

namespace SkibidiRunner.Editor
{
    [CustomEditor(typeof(TileSpawner)), CanEditMultipleObjects]
    public class TileSpawnerEditor : UnityEditor.Editor
    {
        private GUIStyle _style;

        public override void OnInspectorGUI()
        {
            _style ??= GUI.skin.GetStyle("label");
            _style.fontStyle = FontStyle.Bold;
            _style.normal.textColor = Color.white;
            GUILayout.Label("Use context menu for generating map items", _style);
            base.OnInspectorGUI();
        }
    }
}