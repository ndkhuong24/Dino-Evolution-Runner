using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    public Skill skillData;

    public void SetSkillData(Skill skill)
    {
        skillData = skill;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (skillData != null)
            {
                Debug.Log("Player chạm vào skill: " + skillData.skillName);
                ActivateSkill();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("SkillData bị thiếu trên skill object!");
            }
        }
    }

    private void ActivateSkill()
    {
        if (skillData != null)
        {
            Debug.Log("Kích hoạt skill: " + skillData.skillName);
            if (skillData.skillPrefab)
            {
                Instantiate(skillData.skillPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
