using UnityEngine;
using System.Collections;

public class SkillSpawner : MonoBehaviour
{
    public SkillObjectController skillObjectController;
    public Transform spawnPoint;

    private void Start()
    {
        StartCoroutine(SpawnSkillRoutine());
    }

    private IEnumerator SpawnSkillRoutine()
    {
        while (true)
        {
            SpawnSkill();
            float randomTime = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    private void SpawnSkill()
    {
        if (skillObjectController.skills.Count > 0)
        {
            int randomIndex = Random.Range(0, skillObjectController.skills.Count);
            Skill skillToSpawn = skillObjectController.skills[randomIndex];

            if (skillToSpawn.skillPrefab)
            {
                GameObject spawnedSkill = Instantiate(skillToSpawn.skillPrefab, spawnPoint.position, Quaternion.identity);
                SkillIcon skillIcon = spawnedSkill.GetComponent<SkillIcon>();

                if (skillIcon != null)
                {
                    skillIcon.SetSkillData(skillToSpawn); // Truyền dữ liệu skill vào prefab
                }
                else
                {
                    Debug.LogError("SkillPrefab thiếu SkillIcon script!");
                }

                Debug.Log("Spawned skill: " + skillToSpawn.skillName);
            }
        }
    }
}
