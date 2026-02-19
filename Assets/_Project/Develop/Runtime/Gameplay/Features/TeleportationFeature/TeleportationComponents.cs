using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportationFeature
{
    public class CanUseTeleport : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class TeleportationRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportationEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportationEnergyCost : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationCooldownInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportationCooldownCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class InTeleportationCooldown : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class IsTeleportationCompleted : IEntityComponent
    {
        public ReactiveEvent Value;
    }
}
