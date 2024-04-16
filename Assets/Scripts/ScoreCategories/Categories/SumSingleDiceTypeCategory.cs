using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "SumOf", menuName = "Yacht / Score / SumOfFaceType", order = 0)]
	public class SumSingleDiceTypeCategory : ScoreCategoryBase
	{
		public int value;
		
		public override Func<Dice, bool> GetPredicate()
		{
			return d => d.UpFace().GetValue() == value;
		}
	}
}