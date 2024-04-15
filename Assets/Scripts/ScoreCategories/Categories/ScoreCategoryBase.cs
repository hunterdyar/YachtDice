using System;
using System.Collections.Generic;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{
	public class ScoreCategoryBase : ScriptableObject
	{
		public Action<int> OnLastCalculatedScoreChange;
		public Action<int> OnLockedScoreChange;

		public string categoryName;
		//todo: make private and use getters for events.
		public int LastCalculatedScore;
		public int LockedScore;
		
		public int RecalculateScore(DiceCollection dice)
		{
			LastCalculatedScore = Calculate(dice.Dice);
			OnLastCalculatedScoreChange?.Invoke(LastCalculatedScore);
			return LastCalculatedScore;
		}

		public virtual int Calculate(IEnumerable<Dice> dice)
		{
			return 0;
		}
	}
}