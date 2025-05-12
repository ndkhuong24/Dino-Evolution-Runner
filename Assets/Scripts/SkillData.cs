using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public int cost;
    public GameObject skillBehaviorPrefab; // prefab chứa script kế thừa ISkillBehavior
}