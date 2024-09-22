using System.Collections.Generic;
using UnityEngine;

namespace GBJAM12
{
    public class MistakesUI : MonoBehaviour
    {
        public GameObject mistakeUIPrefab;

        private List<MistakeJawUI> mistakes = new List<MistakeJawUI>();

        public void SetMistakes(int total, int current)
        {
            if (mistakes.Count != total)
            {
                // create jaw mistakes
                for (var i = 0; i < total; i++)
                {
                    var mistakeUiGameObject = GameObject.Instantiate(mistakeUIPrefab, transform);
                    mistakes.Add(mistakeUiGameObject.GetComponent<MistakeJawUI>());
                }
            }

            for (var i = 0; i < mistakes.Count; i++)
            {
                mistakes[i].isActive = i < current;
            }
        }
    }
}