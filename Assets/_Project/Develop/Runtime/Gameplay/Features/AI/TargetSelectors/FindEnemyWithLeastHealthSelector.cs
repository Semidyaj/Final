using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelectors
{
    public class FindEnemyWithLeastHealthSelector : ITargetSelector
    {
        private Entity _source;
        private Transform _sourceTransform;

        public FindEnemyWithLeastHealthSelector(Entity entity)
        {
            _source = entity;
            _sourceTransform = entity.Transform;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            IEnumerable<Entity> selectedTargets = targets.Where(target =>
            {
                bool result = target.HasComponent<TakeDamageRequest>();

                if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage))
                {
                    result = result && canApplyDamage.Evaluate();
                }

                result = result && target != _source;

                result = result && GetDistanceTo(target) <= _source.TeleportationRadius.Value && target.CurrentHealth.Value > 0;

                return result;
            });

            if (selectedTargets.Any() == false)
                return null;

            Entity lowestHealthTarget = selectedTargets.First();

            foreach (Entity target in selectedTargets)
            {
                float targetHealth = target.CurrentHealth.Value;
                float lowestTargetHealth = lowestHealthTarget.CurrentHealth.Value;

                if (targetHealth < lowestTargetHealth)
                    lowestHealthTarget = target;
            }

            return lowestHealthTarget;
        }

        private float GetDistanceTo(Entity target)
            => (_sourceTransform.position - target.Transform.position).magnitude;
    }
}
