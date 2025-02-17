using UnityEngine;
using System.Collections.Generic;

public class SkillObjectController : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>(); // Danh s�ch skill trong object

    void Start()
    {
        foreach (var skill in skills)
        {
            Debug.Log("Skill c� trong Object: " + skill.skillName);
        }
    }
}
