using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using UnityEngine;
using SFCore;
using Vasi;

namespace CharmCrab.Enemy {
	class EnemyStats: MonoBehaviour {
		public int origHp = 1;

		public void OnEnable() {
			if (this.name.Contains("Fluke Fly Spawner")) {
				var fsm = FSMUtility.LocateFSM(this.gameObject, "Fluke Fly");

				if (fsm != null) {
					// Special case for recycled fluke spawners in Fluke Marm's arena.
					FsmUtil.GetAction<SetHP>(FsmUtil.GetState(fsm, "Reset")).hp = 5 * 13;
				}
			}
		}

		public void Update() {
			if (this.origHp >= Enemy.EnemyHealth.SuperBossHealthThreshold) {
				foreach (var d in this.GetComponentsInChildren<DamageHero>()) {
					d.damageDealt = d.damageDealt == 0 ? 0 : Enemy.EnemyHealth.SuperBossDamage;
				}
			} else if (this.origHp >= Enemy.EnemyHealth.BossHealthThreshold) {
				foreach (var d in this.GetComponentsInChildren<DamageHero>()) {
					d.damageDealt = d.damageDealt == 0 ? 0 : Enemy.EnemyHealth.BossDamage;
				}
			}
		}
	}
}
