using System;
using System.Collections.Generic;
using System.Linq;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{
	
	[CreateAssetMenu(fileName = "Group Counts", menuName = "Yacht / Score / Group Counts Score Category", order = 0)]
	public class GroupCountsCategory : ScoreCategoryBase
	{
		//ie: {3,2} for some full house
		public int[] groupingCounts;
		public override bool IsValidHand(DiceCollection dice, ref List<Dice> usedDice)
		{
			var groups = dice.GetGroups();
			bool[] hands = new bool[groupingCounts.Length];
			for (int i = 0; i < hands.Length; i++)
			{
				int k = 0;
				foreach (var group in groups)
				{
					var c = group.Count();
					if (c == groupingCounts[i])
					{
						hands[i] = true;
						//cache
						usedDice.AddRange(group);
						break;//skip ahead so it doesn't count for multiple hands of same value.
					}	
					k++;
				}
			}

			return hands.All(x => x);
		}
	}
}