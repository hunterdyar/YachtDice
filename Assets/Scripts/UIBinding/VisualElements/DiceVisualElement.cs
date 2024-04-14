using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

	public class DiceVisualElement : VisualElement
	{
		private bool _initialized;
		public Button diceButton; 
		public Dice Dice { get; set; }
		
		public new class UxmlFactory : UxmlFactory<DiceVisualElement, UxmlTraits>
		{
		}

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				var dve = ve as DiceVisualElement;
				//how should this be serialized? the allowed child element above? Thing?
			}
		}
		//manually binding.

		public void Init()
		{
			diceButton = this.Q<Button>();
			_initialized = diceButton != null;
			if (_initialized)
			{
				diceButton.clicked += DiceButtonOnClicked;
			}
		}

		private void DiceButtonOnClicked()
		{
			if (!_initialized)
			{
				Init();
			}
			
			if (Dice != null)
			{
				Dice.Selected();
			}
		}

		public void SetDice(Dice dice)
		{
			if (!_initialized)
			{
				Init();
			}
			//deregister existing if needed.
			if (Dice != null)
			{
				Dice.OnUpFaceChanged -= OnUpFaceChanged;
				Dice = null;
			}

			if (dice == null)
			{
				OnUpFaceChanged(null);
				return;
			}
			
			Dice = dice;
			dice.OnUpFaceChanged += OnUpFaceChanged;
			OnUpFaceChanged(Dice.UpFace());
		}

		public void OnUpFaceChanged(DiceFace face)
		{
			if (diceButton == null)
			{
				diceButton = this.Q<Button>();
				if (diceButton == null)
				{
					Debug.LogWarning("no button?");
					return;
				}
			}
			if (face == null)
			{
				diceButton.text = "_";
				return;
			}
			
			diceButton.text = face.GetValue().ToString();
		}
		

	}