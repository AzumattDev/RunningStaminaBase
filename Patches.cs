using HarmonyLib;

namespace RunningStaminaBase;

[HarmonyPatch(typeof(Player), nameof(Player.Awake))]
public static class PlayerAwakePatch
{
    public static bool AlteredSuccessfully = false;

    public static void Postfix(ref Player __instance)
    {
        UpdateBases(ref __instance);
    }

    internal static void UpdateBases(ref Player player)
    {
        if (Player.m_localPlayer == null)
            return;
        player.m_baseStamina = 50f + (1 + player.GetSkillFactor(Skills.SkillType.Run));
        if (!(player.m_stamina < player.m_baseStamina)) return;
        player.m_stamina = player.m_baseStamina;
        AlteredSuccessfully = true;
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.GetBaseFoodHP))]
static class PlayerGetBaseFoodHpPatch
{
    static void Prefix(Player __instance)
    {
        var currentResult = __instance.m_baseStamina;
        __instance.m_baseStamina += (__instance.GetSkillLevel(Skills.SkillType.Run) * 2) - currentResult + 50f;
        if (PlayerAwakePatch.AlteredSuccessfully) return;
        __instance.m_stamina = __instance.m_baseStamina;
        PlayerAwakePatch.AlteredSuccessfully = true;
    }
}

[HarmonyPatch(typeof(Player), nameof(Player.OnSkillLevelup))]
static class PlayerOnSkillLevelupPatch
{
    static void Prefix(Player __instance, Skills.SkillType skill, float level)
    {
        if (skill is Skills.SkillType.Run)
        {
            PlayerAwakePatch.UpdateBases(ref __instance);
        }
    }
}