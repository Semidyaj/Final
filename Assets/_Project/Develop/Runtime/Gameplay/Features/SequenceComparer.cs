using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class SequenceComparer
    {
        public bool IsInputCompareGuessed(ReactiveVariable<string> resultString, ReactiveVariable<string> guessedString)
        {
            for (int i = 0; i < resultString.Value.Length; i++)
                if (resultString.Value[i] != guessedString.Value[i])
                    return false;

            return true;
        }
    }
}
