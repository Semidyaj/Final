using Assets._Project.Develop.Runtime.Gameplay.Features;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class GameModeChooseService
    {
        public event Action<GameplayTypes> GameModeChosen;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                GameModeChosen?.Invoke(GameplayTypes.Numbers);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                GameModeChosen?.Invoke(GameplayTypes.Letters);
        }
    }
}
