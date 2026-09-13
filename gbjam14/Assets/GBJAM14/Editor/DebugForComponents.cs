using GBJAM14.Components;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;

namespace Editor
{
    public static class DebugForComponents
    {
        sealed class InventoryComponentInspector : EcsComponentInspectorTyped<InventoryComponent>
        {
            public override bool OnGuiTyped(string label, ref InventoryComponent inventory,
                EcsEntityDebugView entityView)
            {
                if (entityView)
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                }
                
                EditorGUI.indentLevel++;

                foreach (var itemId in inventory.items)
                {
                    EditorGUILayout.LabelField(itemId);
                }

                EditorGUI.indentLevel--;

                return true;
            }
        }
    }
}