using System.Collections.Generic;
using System.IO;
using Living.Player;
using Newtonsoft.Json;
using UnityEngine;

namespace Buildings.Church
{
	public class Church : BuildingAbstract
	{
		[SerializeField] private ChurchPopUp popUp;
		private List<string> jpgFiles;
		private ChurchGod[] churchGods;
		private int tmp = 0;

		private void Awake()
		{
			string json = File.ReadAllText("Assets/Code/Buildings/Church/ChurchGodsProperties.json");
			churchGods = JsonConvert.DeserializeObject<List<ChurchGod>>(json).ToArray();
		}

		public override void Use(PlayerBase pBase)
		{
			base.Use(pBase);
			playerBase.canMove = false;
			popUp.OpenPopUp(this, churchGods[tmp], churchGods[tmp + 1]);
			tmp += 2;
		}

		public void OnPopUpClosed()
		{
			playerBase.canMove = true;
		}
	}
}