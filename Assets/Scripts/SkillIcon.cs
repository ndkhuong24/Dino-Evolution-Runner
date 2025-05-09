﻿using System;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
    private KeyboardManager keyboardManager;
    private Skill skillData;

    private void Awake()
    {
        keyboardManager = FindFirstObjectByType<KeyboardManager>();
    }

    public void SetSkillDataToSpawn(Skill skill)
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
                //Destroy(gameObject);
                ObjectPool.Instance.ReturnObject(gameObject);
            }
        }
    }

    private void SetUpSkillToKeyboard()
    {
        keyboardManager.AssignSkillToKey(skillData);
    }
}