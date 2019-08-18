using UnityEngine;
using System.Collections;

namespace RogueNoodle
{
namespace GBCamera
{

public class RandomPaletteButton : MonoBehaviour
{
	
	public FadeMaterials _fadeMaterials;
	public RandomPalette _randomPalette;
	private bool _switchingPalette = false;
	
	public void OnChooseRandomPalette ()
	{
		if (!_switchingPalette)
		{
			StartCoroutine (ChooseRandomPalette ());
		}
			
	}
	
	public IEnumerator ChooseRandomPalette ()
	{
		_switchingPalette = true;
		yield return _fadeMaterials.StartCoroutine (_fadeMaterials.FadeOut ());
		_randomPalette.SelectRandomPalette ();
		yield return _fadeMaterials.StartCoroutine (_fadeMaterials.FadeIn());
		_switchingPalette = false;
	}
	

}


}
}
