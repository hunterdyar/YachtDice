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
        public int PossibleScore;

		[Header("Configuration")]
		public string categoryName;

		[SerializeField] private ScoreTallyType _tallyType;
		public List<Dice> LastUsedDiceCalculated => _lastUsedDiceCalculated;
		protected  List<Dice> _lastUsedDiceCalculated;
		public List<Dice> LastUsedDicePossible => LastUsedDicePossible;
		protected List<Dice> _lastUsedDicePossible;
		public Action<int> OnPossibleScoreChange;
		
		public int CalculatePossibleScore(DiceCollection dice)
		{
			PossibleScore = Calculate(dice.Dice);
			OnPossibleScoreChange?.Invoke(PossibleScore);
			return PossibleScore;
		}
		
		public int RecalculateScore(DiceCollection dice, ScoreCalculationType calculationType = ScoreCalculationType.Calculated)
		{
			bool valid = false;
			if (calculationType == ScoreCalculationType.Calculated)
			{
				_lastUsedDiceCalculated.Clear();
				valid = IsValidHand(dice,ref _lastUsedDiceCalculated);
			}else if (calculationType == ScoreCalculationType.Possible)
			{
				_lastUsedDicePossible.Clear();
				valid = IsValidHand(dice, ref _lastUsedDicePossible);
			}

			if (calculationType == ScoreCalculationType.Calculated)
			{
				if (valid)
				{
					LastCalculatedScore = Calculate(dice.Dice, calculationType);
				}
				else
				{
					LastCalculatedScore = 0;
				}

				OnLastCalculatedScoreChange?.Invoke(LastCalculatedScore);
				return LastCalculatedScore;
			}else if (calculationType == ScoreCalculationType.Possible)
			{
				if (valid)
				{
					PossibleScore = Calculate(dice.Dice, calculationType);
				}
				else
				{
					PossibleScore = 0;
				}

				OnPossibleScoreChange?.Invoke(PossibleScore);
				return PossibleScore;
			}

			return 0;
		}

		public virtual bool IsValidHand(DiceCollection dice, ref List<Dice> usedDice)
		{
			usedDice = dice.DiceList;
			return true;
		}
		public virtual int Calculate(IEnumerable<Dice> dice, ScoreCalculationType calculationType = ScoreCalculationType.Calculated)
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
            		var f = GetPredicate(calculationType);
            		return dice.Where(f).Sum(d => d.UpFace().GetValue());
            	case ScoreTallyType.sumOfAllDice:
            		return dice.Sum(d => d.UpFace().GetValue());
            	default:
            		return 0;
            }
		}

		public virtual Func<Dice,bool> GetPredicate(ScoreCalculationType calculationType)
		{
			return x => _lastUsedDiceCalculated.Contains(x);
		}

		public List<Dice> CalculatingDiceList(ScoreCalculationType calcType)
		{
			return calcType == ScoreCalculationType.Calculated ? _lastUsedDiceCalculated : _lastUsedDicePossible;
		}
	}
}