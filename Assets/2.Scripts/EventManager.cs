using System;
using UnityEngine;

public static class EventManager
{
    // 스킬 해금 시 호출 (스킬 이름, 스킬 아이콘)
    public static Action<string, Sprite> OnSkillUnlocked;
}
