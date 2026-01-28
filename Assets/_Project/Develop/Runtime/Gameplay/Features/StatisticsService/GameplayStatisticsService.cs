using Assets._Project.Develop.Runtime.Utilities.DataManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatisticsService
{
    public class GameplayStatisticsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private int _winsCount;
        private int _defeatsCount;

        public GameplayStatisticsService(PlayerDataProvider playerDataProvider)
        {
            playerDataProvider.RegisterWriter(this);
            playerDataProvider.RegisterReader(this);
        }

        public int Wins => _winsCount;
        public int Defeats => _defeatsCount;

        public void Apply(GameplayEndState endState)
        {
            if (endState == GameplayEndState.Victory)
                _winsCount++;
            else if (endState == GameplayEndState.Defeat)
                _defeatsCount++;
            else
                throw new InvalidOperationException("Wrong end state " + nameof(endState));
        }

        public void ReadFrom(PlayerData data)
        {
            _winsCount = data.Wins;
            _defeatsCount = data.Defeats;
        }

        public void WriteTo(PlayerData data)
        {
            data.Wins = _winsCount;
            data.Defeats = _defeatsCount;
        }
    }
}
