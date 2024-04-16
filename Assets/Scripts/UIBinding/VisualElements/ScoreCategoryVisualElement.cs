using DefaultNamespace;
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
	}