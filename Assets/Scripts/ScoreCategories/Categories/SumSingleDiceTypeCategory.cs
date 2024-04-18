using System;
using System.Collections.Generic;
using System.Linq;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "SumOf", menuName = "Yacht / Score / SumOfFaceType", order = 0)]
	public class SumSingleDiceTypeCategory : ScoreCategoryBase
	{
		public int value;

		public override bool IsValidHand(DiceCollection dice, ref List<Dice> usedDice)
		{
			usedDice = dice.Dice.Where(d => d.UpFace().GetValue() == value).ToList();
			return usedDice.Any();
		}

		public override Func<Dice, bool> GetPredicate(ScoreCalculationType scoreCalculation = ScoreCalculationType.Calculated) 
		{
			//note: doing this "again" is probably a lot faster than LINQ Contains().
			return d => d.UpFace().GetValue() == value;
		}
	}
}