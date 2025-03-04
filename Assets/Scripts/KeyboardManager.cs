using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    private List<KeyManager> keys = new List<KeyManager>(); // Danh sách các phím có thể gán skill
    private Dictionary<string, KeyManager> keyDictionary = new Dictionary<string, KeyManager>(); // Dễ dàng truy xuất phím theo tên
    private KeyManager keyManager;

    private void Awake()
    {
        keys.AddRange(GetComponentsInChildren<KeyManager>());
        keyManager = GetComponent<KeyManager>();

        foreach (var key in keys)
        {
            keyDictionary[key.keyName] = key;
        }

        //UpdateKeysVisual();
    }

    private void Update()
    {
        DetectKeyPress();
    }

    private void DetectKeyPress()
    {
        foreach (var key in keyDictionary.Keys)
        {
            if (Input.GetKeyDown(key.ToLower()))
            {
                if (keyDictionary[key].HasSkill())
                {
                    keyDictionary[key].ActivateSkill();
                }
            }
        }
    }

    public bool AssignSkillToKey(Skill skill)
    {
        List<KeyManager> emtyKeys = keys.Where(key => !key.HasSkill()).ToList();

        foreach (var key in emtyKeys)
        {
            KeyManager randomKey = emtyKeys[Random.Range(0, emtyKeys.Count)];

            randomKey.SetSkill(skill);

            return true;
        }

        return false;
    }

    //public bool HasEmptyKey()
    //{
    //    foreach (var key in keys)
    //    {
    //        if (!key.HasSkill())
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public bool AssignSkillToKey(Skill skill)
    //{
    //    foreach (var key in keys)
    //    {
    //        if (!key.HasSkill())
    //        {
    //            key.SetSkill(skill);
    //            UpdateKeysVisual();
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public void OnKeyPressed(string keyName)
    //{
    //    if (keyDictionary.TryGetValue(keyName, out KeyManager key))
    //    {
    //        key.ActivateSkill();
    //        UpdateKeysVisual();
    //    }
    //}

    //private void UpdateKeysVisual()
    //{
    //    foreach (var key in keys)
    //    {
    //        key.UpdateVisual();
    //    }
    //}
}
