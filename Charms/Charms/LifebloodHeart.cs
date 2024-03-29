﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Vasi;

namespace CharmCrab.Charms {	
	class LifebloodHeart {
		public readonly uint MAX_HEALTH = 4;
		public readonly uint THRESHOLD = 10;
		private int hits = 0;
		//private Functions.HitDetectManager hit = new Functions.HitDetectManager(0.5f);

		public void Update() {
		//	this.hit.Update();
		}

		private void Heal() {
			GameManager.UnloadLevel doHeal = null;
			doHeal = delegate ()
			{
				EventRegister.SendEvent("ADD BLUE HEALTH");
				GameManager.instance.UnloadingLevel -= doHeal;
				doHeal = null;
			};
			GameManager.instance.UnloadingLevel += doHeal;

			if (doHeal != null) {
				doHeal();
			}
		}

		public void SlashHitHandler(Collider2D col, GameObject slash) {
			if (!CharmData.Equipped(Charm.LifebloodHeart) && !CharmData.Equipped(Charm.LifebloodCore)) { return; }
			if (PlayerData.instance.healthBlue >= MAX_HEALTH) { return; }
			if (col.gameObject.GetComponent<HealthManager>()) {
				this.hits += 1;
				if (CharmData.Equipped(Charm.LifebloodCore) && this.hits >= THRESHOLD) {
					this.Heal();
					this.hits = 0;
				} else if (CharmData.Equipped(Charm.LifebloodHeart) && this.hits >= 2*THRESHOLD) {
					this.Heal();
					this.hits = 0;
				}
			}
		}
	}
}
