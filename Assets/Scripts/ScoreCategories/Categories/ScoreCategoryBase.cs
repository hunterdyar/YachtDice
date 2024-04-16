using System;
using System.Collections.Generic;
using System.Linq;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{
	public class ScoreCategoryBase : ScriptableObject
	{
		public Action<int> OnLastCalculatedScoreChange;
		public Action<int> OnLockedScoreChange;

		[Header("Runtime Data")]
		//todo: make private and use getters for events.
        public int LastCalculatedScore;
        public int LockedScore;
        
		[Header("Configuration")]
		public string categoryName;

		[SerializeField] private ScoreTallyType _tallyType;
		protected  List<Dice> LastUsedDice;
		
		public int RecalculateScore(DiceCollection dice)
		{
			LastUsedDice.Clear();
			var valid = IsValidHand(dice);
			if (valid)
			{
				LastCalculatedScore = Calculate(dice.Dice);
			}
			else
			{
				LastCalculatedScore = 0;
			}
			
			OnLastCalculatedScoreChange?.Invoke(LastCalculatedScore); 
			return LastCalculatedScore;
		}

		public virtual bool IsValidHand(DiceCollection dice)
		{
			LastUsedDice = dice.DiceList;
			return true;
		}
		public virtual int Calculate(IEnumerable<Dice> dice)
		{
			//lastUsedDice needs to be calculated before GetPredicate, which 
			switch (_tallyType)
            {
            	case ScoreTallyType.constant:
            		return 30;//todo: serialized field.
            	case ScoreTallyType.numberOfDice:
            		return dice.Count();//todo: times multiplier
            	case ScoreTallyType.sumOfValidDice:
            		//todo: cache once.
            		var f = GetPredicate();
            		return dice.Where(f).Sum(d => d.UpFace().GetValue());
            	case ScoreTallyType.sumOfAllDice:
            		return dice.Sum(d => d.UpFace().GetValue());
            	default:
            		return 0;
            }
		}

		public virtual Func<Dice,bool> GetPredicate()
		{
			return x => LastUsedDice.Contains(x);
		}
	}
}