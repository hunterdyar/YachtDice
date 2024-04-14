using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "Score Categories", menuName = "ScoreCategoryCollection", order = 0)]
	public class ScoreCategoryCollection : ScriptableObject
	{
		public List<ScoreCategoryBase> Catgories => _scoreCategories;
		[SerializeField]
		private List<ScoreCategoryBase> _scoreCategories;
	}
}