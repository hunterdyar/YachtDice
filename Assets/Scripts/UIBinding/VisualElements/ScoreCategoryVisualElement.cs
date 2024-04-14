using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.UIElements;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;


	public class ScoreCategoryVisualElement : VisualElement
	{
		private ScoreCategoryBase _category;
		private Label nameLabel;
		private Label currentScoreLabel;
		private Label lockedScoreLable;
		
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
			
			
			Init();
			_category = category;
			nameLabel.text = category.categoryName;
			_category.OnLastCalculatedScoreChange += OnLastCalculatedScoreChange;
			OnLastCalculatedScoreChange(_category.LastCalculatedScore);
			
			//todo: same as last calculated for locked
			lockedScoreLable.text = category.LockedScore.ToString();
			//bind data and actions.
		}

		private void OnLastCalculatedScoreChange(int s)
		{
			currentScoreLabel.text = s.ToString();
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

			if (lockedScoreLable == null)
			{
				lockedScoreLable = this.Q<Label>("Banked");
			}
		}
	}