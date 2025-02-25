using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    public string keyName; // Key name (Q, W, E, etc.)
    private CanvasGroup keyCanvasGroup; // Controls transparency of the key
    private CanvasGroup skillCanvasGroup; // Controls transparency of the skill icon
    private Image skillIconImage; // Skill icon image 

    private Skill assignedSkill = null; // The currently assigned skill

    private void Awake()
    {
        if (keyCanvasGroup == null) keyCanvasGroup = GetComponent<CanvasGroup>();
        if (skillCanvasGroup == null) skillCanvasGroup = transform.Find("SkillIcon").GetComponent<CanvasGroup>();
        if (skillIconImage == null) skillIconImage = transform.Find("SkillIcon").GetComponent<Image>();

        //    ResetKey(); // Initialize key state
    }

    public bool HasSkill()
    {
        return assignedSkill != null;
    }

    internal void SetSkill(Skill skill)
    {
        assignedSkill = skill;
        keyCanvasGroup.alpha = 1f;
        skillCanvasGroup.alpha = 1f;
        skillIconImage.sprite = skill.icon;
    }

    //public void SetSkill(Skill skill)
    //{
    //    assignedSkill = skill;
    //    UpdateVisual();
    //}

    //public void ActivateSkill()
    //{
    //    if (assignedSkill != null)
    //    {
    //        Debug.Log($"Activating skill: {assignedSkill.skillName}");

    //        if (assignedSkill.skillPrefab)
    //        {
    //            Instantiate(assignedSkill.skillPrefab, transform.position, Quaternion.identity);
    //        }

    //        ResetKey(); // Reset key state after activation
    //    }
    //}

    //private void ResetKey()
    //{
    //    assignedSkill = null;
    //    UpdateVisual();
    //}

    //// **🟢 The missing UpdateVisual() function**
    //public void UpdateVisual()
    //{
    //    if (assignedSkill != null)
    //    {
    //        skillIconImage.sprite = assignedSkill.icon; // Show skill icon
    //        skillCanvasGroup.alpha = 1f; // Make skill icon visible
    //        keyCanvasGroup.alpha = 1f; // Make key fully visible
    //    }
    //    else
    //    {
    //        skillIconImage.sprite = null; // Remove skill icon
    //        skillCanvasGroup.alpha = 0f; // Hide skill icon
    //        keyCanvasGroup.alpha = 0.5f; // Dim the key when empty
    //    }
    //}
}
