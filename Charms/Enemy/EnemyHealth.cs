using System;
using System.Collections.Generic;
using System.Text;
using Unity;
using UnityEngine;
using Vasi;
using SFCore;

namespace CharmCrab.Enemy {
	class EnemyHealth {
		public const int BossDamage = 2;
		public const int SuperBossDamage = 3;
		public const int SuperHealthScaleFactor = 7;
		public const int HealthScaleFactor = 5;
		public const int BossHealthThreshold = 65;
		public const int SuperBossHealthThreshold = 500;

		private static int StandardHP(HealthManager hm) {
			if (hm.hp >= SuperBossHealthThreshold) {
				hm.hp *= SuperHealthScaleFactor;
				return SuperHealthScaleFactor;
			} else if (hm.hp >= BossHealthThreshold) {
				hm.hp *= HealthScaleFactor;
			} else {
				hm.hp *= HealthScaleFactor;
			}

			return HealthScaleFactor;
		}

		public static void HandleEnemy(GameObject obj) {
			if (obj.GetComponent<EnemyStats>() == null) {
				var stats = obj.AddComponent<EnemyStats>();
				var hm = obj.GetComponent<HealthManager>();
				stats.origHp = hm.hp;
				//Log("Enemy found: " + hm.name);
				if (obj.name == "Hollow Shade(Clone)") {
					// Special case to prevent the hollow shade from having thousands of health inadvertently and doing super boss damage.
					hm.hp = PlayerData.instance.maxHealth / 2 * Charms.CharmEffects.instance.ComputeDamage(DamageType.Slash);
					stats.origHp = hm.hp;
				} else if (obj.name == "Blocker") {
					hm.hp = 40;
				} else if (obj.name == "Head") {
					StandardHP(hm);
					// The case for the False/Failed Knight Head. Need to make sure the recovery HP is set to the correct health.
					var fsm = obj.LocateMyFSM("Health Check");
					var state = FsmUtil.GetState(fsm, "State 1");
					FsmUtil.GetAction<SetHP>(state).hp.Value *= HealthScaleFactor;

				} else if (obj.name.Contains("False Knight")) {
					StandardHP(hm);
					var fsm = obj.LocateMyFSM("FalseyControl");
					var v = fsm.FsmVariables.FindFsmInt("Recover HP");
					if (v == null) {
						fsm = obj.LocateMyFSM("Check Health");
						v = fsm.FsmVariables.FindFsmInt("Recover HP");

						if (obj.name.Contains("New")) {
							v.Value *= HealthScaleFactor;
						} else {
							v.Value *= SuperHealthScaleFactor;
						}
					} else {
						if (obj.name.Contains("New")) {
							v.Value *= HealthScaleFactor;
						} else {
							v.Value *= SuperHealthScaleFactor;
						}
					}

				} else if (obj.name.Contains("Grimm Boss")) {
					var fsm = obj.LocateMyFSM("Control");
					var scale = StandardHP(obj.GetComponent<HealthManager>());
					fsm.FsmVariables.FindFsmInt("Rage HP 1").Value *= scale;
					fsm.FsmVariables.FindFsmInt("Rage HP 2").Value *= scale;
					fsm.FsmVariables.FindFsmInt("Rage HP 3").Value *= scale;
				} else {
					StandardHP(hm);
				}
			}			
		}

		public static void UpdateSpikeBallDamage(GameObject obj)
		{
			if (obj.name.Contains("Spike Ball"))
			{
				PlayMakerFSM fsm = obj.GetComponent<PlayMakerFSM>();
				SFCore.Utils.FsmUtil.GetIntVariable(fsm, "damageDealt").Value *= HealthScaleFactor;
			}
		}
	}
}
