using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features
{
    public class InputHandler
    {
        public string GetInputKeyboardText()
        {
            string resultForFrame = "";

            foreach (char enteredSymbol in Input.inputString)
                resultForFrame += enteredSymbol;

            return resultForFrame;
        }
    }
}
