using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{

	//todo: all straights should be calculated, and return the dice count value.
	[CreateAssetMenu(fileName = "Straight", menuName = "Yacht / Score /Straight", order = 0)]
	public class Straight : ScoreCategoryBase
	{
		//listUsedDice
		[SerializeField] private int _run;
		public override bool IsValidHand(DiceCollection dice)
		{
			var lowToHigh = dice.GetSortedLowToHigh();
			for (int i = 0; i < lowToHigh.Count()-(_run-1); i++)
			{
				if (IsStraight(lowToHigh.Skip(i).Take(_run)))
				{
					//lastuseddice.add
					return true;
				}
			}

			 return false;
		}

		private bool IsStraight(IEnumerable<Dice> dice)
		{
			//if there are duplicates, then the group count won't match the total count.
			//then subtract the lowest value from the highest value.
			//I think we can optimize this by taking a sorted list and not doing the min/max, but first/last.
			var unique = dice.Select(x => x.UpFace().GetValue()).Distinct().Count();
			if (unique != dice.Count())
			{
				return false;
			}

			var min = dice.First();
			var max = dice.Last();

			var f = max.UpFace().GetValue() - min.UpFace().GetValue();
			return f == _run-1;
			//	dice.GroupBy(d => d.UpFace().GetValue()).Count() == dice.Count() &&
			//	       dice.Max(d => d.UpFace().GetValue()) - dice.Min(d => d.UpFace().GetValue()) == _run;
		}
	}
}