namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class SequenceComparer
    {
        public bool IsInputCompareGuessed(string resultString, string guessedString)
        {
            for (int i = 0; i < resultString.Length; i++)
                if (resultString[i] != guessedString[i])
                    return false;

            return true;
        }
    }
}
