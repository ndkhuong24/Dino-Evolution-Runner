using UnityEngine;

public interface ISkillBehavior
{
    void Activate(GameObject user);
    void Deactivate(GameObject user);
}