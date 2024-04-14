using System;
using SODefinitions;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
	public class TestDiceSpawner : MonoBehaviour
	{
		public DiceCollection Tray;
		public DiceCollection Keep;
		public ScoreCategoryCollection Scores;
		public void Start()
		{
			for (int i = 0; i < 5; i++)
			{
				var d = Dice.CreateNormalDice(6);
				d.Roll();
				Tray.AddDice(d);
			}
			RecalculateScore();
		}

		private void OnEnable()
		{
			Tray.OnDiceSelected += OnTrayDiceSelected;
			Keep.OnDiceSelected += OnKeepDiceSelected;
		}

		private void OnDisable()
		{
			Tray.OnDiceSelected -= OnTrayDiceSelected;
			Keep.OnDiceSelected -= OnKeepDiceSelected;
		}

		private void OnTrayDiceSelected(Dice dice)
		{
			Tray.RemoveDice(dice);
			Keep.AddDice(dice);
			RecalculateScore();
		}

		private void OnKeepDiceSelected(Dice dice)
		{
			Keep.RemoveDice(dice);
			Tray.AddDice(dice);
			RecalculateScore();
		}

		private void Update()
		{
			if (Input.GetButton("Jump"))
			{
				foreach (var dice in Tray.Dice)
				{
					dice.Roll();
					RecalculateScore();
				}
			}else if (Input.GetKeyDown(KeyCode.N))
			{
				var d = Dice.CreateNormalDice(6);
				d.Roll();
				Tray.AddDice(d);
			}else if (Input.GetKeyDown(KeyCode.M))
			{
				RecalculateScore();
			}
		}

		void RecalculateScore()
		{
			foreach (var cat in Scores.Catgories)
			{
				cat.RecalculateScore(Keep);
			}
		}
	}
}