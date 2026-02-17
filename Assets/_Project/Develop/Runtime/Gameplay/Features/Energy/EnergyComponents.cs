using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class MaxEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CurrentEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRecoveryTimePeriod : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRecoveryCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class PercentToRecovery : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CanRecoverEnergy : IEntityComponent
    {
        public ICompositeCondition Value;
    }
}
