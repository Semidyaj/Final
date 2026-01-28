using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Meta.Economy
{
    [CreateAssetMenu(menuName = "Configs/Meta/Economy/NewGameplayEconomyConfig", fileName = "GameplayEconomyConfig")]
    public class GameplayEconomyConfig : ScriptableObject
    {
        [field: SerializeField] public int StatsResetPrice { get; private set; }
        [field: SerializeField] public int VictoryReward { get; private set; }
        [field: SerializeField] public int DefeatFee { get; private set; }
    }
}
