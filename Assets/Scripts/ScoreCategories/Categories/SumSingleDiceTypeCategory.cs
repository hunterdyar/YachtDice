using System.Collections;
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
		//public DiceFace ComparisonFace;
		public override int Calculate(IEnumerable<Dice> dice)
		{
			return dice.Where(x => x.UpFace().GetValue() == value).Sum(x=>x.UpFace().GetValue());
		}
	}
}