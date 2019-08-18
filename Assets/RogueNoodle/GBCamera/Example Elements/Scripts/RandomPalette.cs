using UnityEngine;
using System.Collections;

namespace RogueNoodle
{
namespace GBCamera
{

public class RandomPalette : MonoBehaviour {

	public Texture[] _palettes;
	public Material[] _materials;
	private int _previousPaletteIndex = 0;
	
	public void SelectRandomPalette ()
	{
		if (_palettes.Length == 0 || _materials.Length == 0)
			return;
		
		int randomPaletteIndex = _previousPaletteIndex;
		while (randomPaletteIndex == _previousPaletteIndex)
		{
			randomPaletteIndex = Random.Range (0, _palettes.Length);
		}	
		
		for (int i = 0; i < _materials.Length; i++)
		{
			_materials[i].SetTexture ("_Palette", _palettes[randomPaletteIndex]);
		}
		
		_previousPaletteIndex = randomPaletteIndex;
	}
}

} // GBCamera
} // RogueNoodle
