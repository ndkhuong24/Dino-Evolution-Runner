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

            // Kiểm tra xem skillPrefab có bị trống không
            if (skill.skillPrefab != null)
            {
                // Instantiate skillPrefab tại vị trí của SkillObject
                Instantiate(skill.skillPrefab, transform.position, Quaternion.identity);
                Debug.Log("Spawned: " + skill.skillName);
            }
            else
            {
                Debug.LogError("SkillPrefab bị thiếu: " + skill.skillName);
            }
        }
    }
}
