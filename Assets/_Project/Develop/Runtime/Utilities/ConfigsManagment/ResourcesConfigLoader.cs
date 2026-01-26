using Assets._Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.ConfigsManagment
{
    public class ResourcesConfigLoader : IConfigsLoader
    {
        private readonly ResourcesAssetsLoader _resources;

        private readonly Dictionary<Type, string> _configResourcesPaths = new()
        {
            {typeof(NumbersGameplayConfig), "Configs/Gameplay/NumbersGameplayConfig" },
            {typeof(LettersGameplayConfig), "Configs/Gameplay/LettersGameplayConfig" },
            {typeof(StartWalletConfig), "Configs/Meta/Wallet/StartWalletConfig" },
        };

        public ResourcesConfigLoader(ResourcesAssetsLoader resources)
        {
            _resources = resources;
        }

        public IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded)
        {
            Dictionary<Type, object> loadedConfigs = new();

            foreach (KeyValuePair<Type, string> configResourcesPath in _configResourcesPaths)
            {
                ScriptableObject config = _resources.Load<ScriptableObject>(configResourcesPath.Value);
                loadedConfigs.Add(configResourcesPath.Key, config);
                yield return null;
            }

            onConfigsLoaded?.Invoke(loadedConfigs);
        }
    }
}