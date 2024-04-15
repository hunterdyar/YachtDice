﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
	[CreateAssetMenu(fileName = "Score Categories", menuName = "ScoreCategoryCollection", order = 0)]
	public class ScoreCategoryCollection : ScriptableObject
	{
		public Action OnCategoriesChanged;
		public List<ScoreCategoryBase> Categories => _scoreCategories;
		[SerializeField]
		private List<ScoreCategoryBase> _scoreCategories;

		[ContextMenu("Rebuild")]
		public void Rebuild()
		{
			OnCategoriesChanged?.Invoke();
		}
	}
}