﻿using System;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    private KeyboardManager keyboardManager;
    public Skill skillData;

    private void Awake()
    {
        keyboardManager = FindFirstObjectByType<KeyboardManager>();

        //if (keyboardManager == null)
        //{
        //    Debug.LogError("Không tìm thấy KeyboardManager trong scene!");
        //}
    }

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
                SetUpSkillToKeyboard();
                Destroy(gameObject);
            }
        }
    }

    private void SetUpSkillToKeyboard()
    {
        keyboardManager.AssignSkillToKey(skillData);
    }

    private void ActivateSkill()
    {
        if (skillData != null)
        {
            //Debug.Log("Kích hoạt skill: " + skillData.skillName);
            if (skillData.skillPrefab)
            {
                Instantiate(skillData.skillPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
