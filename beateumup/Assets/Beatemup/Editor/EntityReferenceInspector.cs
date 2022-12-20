using Beatemup.Ecs;
using UnityEditor;

namespace Beatemup.Editor
{
    [CustomEditor(typeof(EntityReference))]
    [CanEditMultipleObjects]
    public class EntityReferenceInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var entityReference = target as EntityReference;
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("ENTITY" ,$"{entityReference.entity.entity:x8}:{entityReference.entity.generation}");
            EditorGUI.EndDisabledGroup();
        }
    }
}