using System;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    public Skill skillData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player chạm vào skill: " + skillData.skillName);
            ActivateSkill();
        }
    }

    private void ActivateSkill()
    {
        Debug.Log("Kích hoạt skill: " + skillData.skillName);

        if (skillData.skillPrefab)
        {
            Instantiate(skillData.skillPrefab, transform.position, Quaternion.identity);
        }
    }
}
