using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

	//these are scriptableObjects for easier serialization and saving/loading things.	
	public class Dice : ScriptableObject
	{
		public Action<Dice> OnSelected;
		public Action<DiceFace> OnUpFaceChanged;
		private List<DiceFace> _diceFaces;
		private int _upFace = 0;
		public DiceFace UpFace() => _diceFaces[_upFace];

		public void Roll()
		{
			//animation?
			_upFace = Random.Range(0, _diceFaces.Count);
			OnUpFaceChanged?.Invoke(UpFace());
		}

		public static Dice CreateNormalDice(int sides)
		{
			Dice d = ScriptableObject.CreateInstance<Dice>();
			d._diceFaces = new List<DiceFace>();
			for (int i = 0; i < sides; i++)
			{
				d._diceFaces.Add(DiceFace.CreateNormalDiceFace(i+1));
			}

			return d;
		}

		public void Selected()
		{
			OnSelected?.Invoke(this);
		}
	}