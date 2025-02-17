using UnityEngine;

public class SkillSpawner : MonoBehaviour
{
    public SkillObjectController skillObjectController; // Kéo SkillObject vào đây
    public Transform spawnPoint; // Vị trí spawn skill

    public void SpawnSkill()
    {
        if (skillObjectController.skills.Count > 0)
        {
            int randomIndex = Random.Range(0, skillObjectController.skills.Count);
            Skill skillToSpawn = skillObjectController.skills[randomIndex];

            if (skillToSpawn.skillPrefab)
            {
                Instantiate(skillToSpawn.skillPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log("Spawn skill: " + skillToSpawn.skillName);
            }
        }
    }
}
