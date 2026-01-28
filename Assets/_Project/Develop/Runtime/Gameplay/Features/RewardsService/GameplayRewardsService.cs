using Assets._Project.Develop.Runtime.Configs.Meta.Economy;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.RewardsService
{
    public class GameplayRewardsService
    {
        private readonly WalletService _wallet;
        private readonly GameplayEconomyConfig _economyConfig;

        public GameplayRewardsService(WalletService wallet, GameplayEconomyConfig economyConfig)
        {
            _wallet = wallet;
            _economyConfig = economyConfig;
        }

        public void ApplyVictoryReward() => _wallet.Add(CurrencyTypes.Gold, _economyConfig.VictoryReward);

        public void ApplyDefeatFee()
        {
            if (_wallet.Enough(CurrencyTypes.Gold, _economyConfig.DefeatFee))
                _wallet.Spend(CurrencyTypes.Gold, _economyConfig.DefeatFee);
        }
    }
}
