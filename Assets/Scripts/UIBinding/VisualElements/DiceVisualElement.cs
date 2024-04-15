using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

	public class DiceVisualElement : VisualElement
	{
		public Button diceButton; 
		public Dice Dice { get; set; }

		public DiceVisualElement()
		{
			this.RegisterCallback<AttachToPanelEvent>(evt => Init());
		}
		public void Init()
        {
        	diceButton = this.Q<Button>(); 
	        diceButton.clicked += DiceButtonOnClicked;
        }
		
		public new class UxmlFactory : UxmlFactory<DiceVisualElement, UxmlTraits>
		{
		}


		private void DiceButtonOnClicked()
		{
			if (Dice != null)
			{
				Dice.Selected();
			}
		}

		public void SetDice(Dice dice)
		{
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