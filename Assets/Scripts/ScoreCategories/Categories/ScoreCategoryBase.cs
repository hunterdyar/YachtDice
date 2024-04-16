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
		
		public int RecalculateScore(DiceCollection dice)
		{
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
			//keep a list of dice that would count for this hand?
			return true;
		}
		public virtual int Calculate(IEnumerable<Dice> dice)
		{
			
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
			return new Func<Dice, bool>(x => true);
		}
	}
}