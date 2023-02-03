using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace RunningStaminaBase
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInIncompatibility("Azumatt.AllTheBases")]
    public class RunningStaminaBasePlugin : BaseUnityPlugin
    {
        internal const string ModName = "RunningStaminaBase";
        internal const string ModVersion = "1.0.2";
        internal const string Author = "Azumatt";
        private const string ModGUID = Author + "." + ModName;

        private readonly Harmony _harmony = new(ModGUID);

        public static readonly ManualLogSource RunningStaminaBaseLogger =
            BepInEx.Logging.Logger.CreateLogSource(ModName);

        public void Awake()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            _harmony.PatchAll(assembly);
        }
    }
}