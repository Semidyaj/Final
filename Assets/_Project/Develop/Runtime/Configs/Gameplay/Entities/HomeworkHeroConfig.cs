using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/NewHomeworkHeroConfig", fileName = "HomeworkHeroConfig")]
    public class HomeworkHeroConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/HomeworkHero";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float MaxEnergy { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float EnergyRecoveryTimePeriod { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float PercentToEnergyRecoveryByPeriod { get; private set; } = 0.1f;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float TeleportationRadius { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float TeleportationEnergyCost { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float TeleportationCooldown { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float AOEDamage { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float AOERadius { get; private set; } = 5;
    }
}
