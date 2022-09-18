using GBJAM.Commons.Transitions;
using UnityEngine;

public class TestRoomTransitionController : MonoBehaviour
{
    public Transition transition;

    public Transform unitTransform;

    private void Update()
    {
        if (unitTransform.transform.position.x > 3)
        {
            transition.Open();
        }
        else
        {
            transition.Close();
        }
    }
}
