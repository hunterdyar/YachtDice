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

		public override bool IsValidHand(DiceCollection dice)
		{
			LastUsedDice = dice.Dice.Where(d => d.UpFace().GetValue() == value).ToList();
			return LastUsedDice.Any();
		}

		public override Func<Dice, bool> GetPredicate() 
		{
			//note: doing this "again" is probably a lot faster than LINQ Contains().
			return d => d.UpFace().GetValue() == value;
		}
	}
}