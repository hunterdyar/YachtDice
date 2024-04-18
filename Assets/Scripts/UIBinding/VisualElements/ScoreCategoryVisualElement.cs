using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;


	public class ScoreCategoryVisualElement : VisualElement
	{
		private ScoreCategoryBase _category;
		private Label nameLabel;
		private Label currentScoreLabel;
		private Label lockedScoreLabel;
		private Label possibleScoreLabel;
		
		public ScoreCategoryVisualElement()
		{
			RegisterCallback<AttachToPanelEvent>(evt => Init());
			RegisterCallback<DetachFromPanelEvent>(evt => Decompose());
			RegisterCallback<PointerEnterEvent>(evt => OnMouseHover(true));
			RegisterCallback<PointerLeaveEvent>(evt => OnMouseHover(false));

		}

		private void OnMouseHover(bool p0)
		{
			if (_category != null)
			{
				foreach (var d in _category.LastUsedDicePossible)
				{
					//todo: we use lastDice to calculate teh score in multiple places.... which one happens last? LastDiceUsed isn't as sticky as I would like. Should be current-calculating and possible-calculating.
					d.SetHighlight(true);
				}
			}
		}

		private void Init()
		{
			if (nameLabel == null)
			{
				nameLabel = this.Q<Label>("Name");
			}

			if (currentScoreLabel == null)
			{
				currentScoreLabel = this.Q<Label>("Current");
			}

			if (lockedScoreLabel == null)
			{
				lockedScoreLabel = this.Q<Label>("Banked");
			}

			if (possibleScoreLabel == null)
			{
				possibleScoreLabel = this.Q<Label>("Possible");
			}
			
		}
		private void Decompose()
		{
			//this doesn't quite match init, because _category is set in Bind.
			if (_category != null)
			{
				_category.OnPossibleScoreChange -= OnPossibleCalculatedScoreChange;
				_category.OnLastCalculatedScoreChange -= OnLastCalculatedScoreChange;
			}
		}

		public new class UxmlFactory : UxmlFactory<ScoreCategoryVisualElement, UxmlTraits>
		{
			
		} 
		public void Bind(ScoreCategoryBase category)
		{
			if (_category != null)
			{
				_category.OnLastCalculatedScoreChange -= OnLastCalculatedScoreChange;


				_category = null;
			}
			
			//Init();
			
			_category = category;
			nameLabel.text = category.categoryName;
			_category.OnLastCalculatedScoreChange += OnLastCalculatedScoreChange;
			OnLastCalculatedScoreChange(_category.LastCalculatedScore);

			_category.OnPossibleScoreChange += OnPossibleCalculatedScoreChange;
			OnPossibleCalculatedScoreChange(_category.PossibleScore);
			
			//todo: same as last calculated for locked
			lockedScoreLabel.text = category.LockedScore.ToString();
			//bind data and actions.
		}

		private void OnLastCalculatedScoreChange(int s)
		{
			currentScoreLabel.text = s.ToString();
		}

		private void OnPossibleCalculatedScoreChange(int s)
		{
			possibleScoreLabel.text = s.ToString();
		}

		
	}