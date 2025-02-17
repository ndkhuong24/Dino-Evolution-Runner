using UnityEngine;
using System.Collections.Generic;

public class SkillObjectController : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>(); // Danh sách skill trong object

    void Start()
    {
        foreach (var skill in skills)
        {
            Debug.Log("Skill có trong Object: " + skill.skillName);
        }
    }
}
