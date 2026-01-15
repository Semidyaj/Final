using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets._Project.Develop.Utilities.ConfigsManagment
{
    public interface IConfigsLoader
    {
        IEnumerator LoadAsync(Action<Dictionary<Type, object>> onConfigsLoaded);
    }
}