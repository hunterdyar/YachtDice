using System;
using System.Collections.Generic;
using DefaultNamespace;
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
		public Action<DiceDisplayState> OnDisplayStateChange;
		public DiceDisplayState DisplayState => _displayState;
		private DiceDisplayState _displayState;
		
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
			//todo: Roll animation should change to Rolling, then change to static-or-appropriate when done with rolling.
			//this is why I don't like this UI-owned events. Hard to sync with game-state.		
			_displayState = DiceDisplayState.Static;
			OnDisplayStateChange?.Invoke(_displayState);
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

		public void SetHighlight(bool highlight)
		{
			if (_displayState == DiceDisplayState.Static && highlight)
			{
				_displayState = DiceDisplayState.Highlighted;
			}else if (_displayState == DiceDisplayState.Highlighted && !highlight)
			{
				_displayState = DiceDisplayState.Static;
			}
			//else? toggling highlighting can get complex if it's around other states... 
			//find the visualElement(s) that are rendering this dice, and add or remove a class to them.
			OnDisplayStateChange?.Invoke(_displayState);
		}
	}