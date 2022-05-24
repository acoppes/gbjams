using GBJAM9.Ecs;
using UnityEngine;

public class AbilityDefinition : MonoBehaviour
{
    public float cooldown;
    public float duration;

    public GameObject projectileDefinitionPrefab;

    public Ability.StartType startType;
}