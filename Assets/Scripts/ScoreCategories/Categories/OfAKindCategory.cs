using System;
using System.Collections.Generic;
using System.Linq;
using SODefinitions;
using UnityEngine;

namespace DefaultNamespace
{
	
	[CreateAssetMenu(fileName = "OfAKind", menuName = "Yacht / Score /Of a Kind Score Category", order = 0)]
	public class OfAKindCategory : ScoreCategoryBase
	{
		private List<Dice> _diceUsed = new List<Dice>();
		[SerializeField]
		private int _count;

		public override bool IsValidHand(DiceCollection dice)
		{
			_diceUsed.Clear();
			var groups = dice.GetGroups();
			var validGroups = groups.Where(x => x.Count() == _count)
				.OrderByDescending(x => x.First().UpFace().GetValue());

			var c = validGroups.Count();
			if (c == 0)
			{
				return false;
			}
			else
			{
				//automatically choose the highest number for dice here by grabbing the highest by sorting value.
				var v = validGroups.First();
				foreach (var d in v)
				{
					_diceUsed.Add(d);
				}

				return true;
			}
		}
		//todo: move usedDice to base and implement everywhere, so we can highlight the dice when you hover over the category.
		public override Func<Dice, bool> GetPredicate()
        {
        	return (x) => _diceUsed.Contains(x);
        }
	}
}