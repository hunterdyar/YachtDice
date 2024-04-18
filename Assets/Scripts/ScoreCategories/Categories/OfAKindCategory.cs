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
		[SerializeField]
		private int _count;

		public override bool IsValidHand(DiceCollection dice, ref List<Dice> usedDice)
		{
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
				usedDice.AddRange(v);

				return true;
			}
		}
		

	}
}