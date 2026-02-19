using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.Teleportation
{
    public class CooldownState : State, IUpdatableState
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _cooldownTime;

        public void Update(float deltaTime)
        {
            
        }
    }
}
