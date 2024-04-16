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
		private List<Dice> _usedDice = new List<Dice>(); 
		public override bool IsValidHand(DiceCollection dice)
		{
			var groups = dice.GetGroups();
			bool[] hands = new bool[groupingCounts.Length];
			_usedDice.Clear();
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
						foreach (var d in group) 
						{
							_usedDice.Add(d);
						}
						break;//skip ahead so it doesn't count for multiple hands of same value.
					}	
					k++;
				}
			}

			return hands.All(x => x);
		}

		public override Func<Dice, bool> GetPredicate()
		{
			return (x) => _usedDice.Contains(x);
		}
	}
}