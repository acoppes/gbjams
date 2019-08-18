using UnityEngine;
using System.Collections;

public class FadeMaterials : MonoBehaviour {

	public Material[] _materials;
	public float _fadeAmountPerSecond;
	
	
	public IEnumerator FadeIn ()
	{
		bool complete = false;
		
		while (!complete)
		{
			complete = true;
			
			for (int i = 0; i < _materials.Length; i++)
			{
				float fade = _materials[i].GetFloat ("_Fade");
				
				fade = Mathf.Min (1f, fade + (_fadeAmountPerSecond * Time.deltaTime));
				if (fade < 1f)
				{
					complete = false;
				}
				
				_materials[i].SetFloat ("_Fade", fade);
			}
			
			yield return null;
		}
		
		yield break;
		
	}
	
	public IEnumerator FadeOut ()
	{
		bool complete = false;
		
		while (!complete)
		{
			complete = true;
			
			for (int i = 0; i < _materials.Length; i++)
			{
				float fade = _materials[i].GetFloat ("_Fade");
				
				fade = Mathf.Max (0f, fade - (_fadeAmountPerSecond * Time.deltaTime));
				if (fade > 0)
				{
					complete = false;
				}
				
				_materials[i].SetFloat ("_Fade", fade);
			}
			
			yield return null;
		}
		
		yield break;
		
	}
	
}
