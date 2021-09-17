using GBJAM7.Scripts;
using UnityEngine;

namespace Scenes.AttackScene
{
    public class AttackSceneController : MonoBehaviour
    {
        public AttackSequence attackSequence;

        public AttackSequenceData[] testAttackDatas;

        // Update is called once per frame
        public void Update()
        {
            var codes = new []
            {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Alpha5,
                KeyCode.Alpha6,
                KeyCode.Alpha7,
                KeyCode.Alpha8,
                KeyCode.Alpha9
            };

            for (var i = 0; i < testAttackDatas.Length; i++)
            {
                if (Input.GetKeyUp(codes[i]))
                {
                    attackSequence.Show(testAttackDatas[i]);
                }
            }
        }
    }
}
