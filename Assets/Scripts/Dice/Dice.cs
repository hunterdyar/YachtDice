using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

	//these are scriptableObjects for easier serialization and saving/loading things.	
	//todo: the dice binding didn't actually work, but i still want to go back and try again at it.
	//so it can probably just be a POCO instead of an SO.
	public class Dice : ScriptableObject
	{
		//todo: make enum for different display states.
		public Action<bool> OnDisplayStateChange;
		public bool DisplayState => _displayState;
		private bool _displayState;
		
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
			//getting a stack overflow
			//OnSelected?.Invoke(this);
		}

		public void SetHighlight(bool highlight)
		{
			//find the visualElement(s) that are rendering this dice, and add or remove a class to them.
			
		}
	}