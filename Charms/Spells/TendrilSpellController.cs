﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace CharmCrab.Spells {
	

	class TendrilSpellController: MonoBehaviour {
		// The objects we need to regularly apply damage to.
		private readonly HashSet<HealthManager> attacking = new HashSet<HealthManager>();
		private Functions.HitDetectManager hit;
		// Animator to keep track of.
		private Animator anim;

		public void Start() {
			this.anim = this.GetComponent<Animator>();
			this.hit = new Functions.HitDetectManager(0.15f);
		}
		public void Update() {		

			var v1 = GameManager.instance.gameSettings.masterVolume / 10.0f;
			var v2 = GameManager.instance.gameSettings.soundVolume / 10.0f;
			var v3 = v1 * v2;
			if (this.IsActive()) {
				if (GameManager.instance.isPaused) {
					foreach (var a in this.GetComponents<AudioSource>()) {
						a.Pause();
					}
				} else {
					foreach (var a in this.GetComponents<AudioSource>()) {
						a.UnPause();
						a.volume = v3;
					}
					this.ApplyDamage();
				}
				this.hit.Update();
			} else { 
				Destroy(this.transform.parent.gameObject);
				Destroy(this.gameObject);
			}
		}


		private bool IsActive() {
			return !(this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0));
		}

		private void ApplyDamage() {
			foreach (var hm in this.attacking) {
				if (!this.hit.Hit(hm.gameObject)) {
					var hit = this.GenHit();
					hm.Hit(hit);
					FSMUtility.SendEventToGameObject(hm.gameObject, "TAKE DAMAGE", false);

					var d = hm.gameObject.GetComponent<Charms.Debuffs>();

					if (d) {
						d.StackBleed();
						d.SoulEat();
						d.SoulCatch(hit.DamageDealt);
					}

					this.hit.Register(hm.gameObject);
				}
				
			}
		}

		public void OnTriggerEnter2D(Collider2D col) {
			var go = col.gameObject;
			var hm = go.GetComponent<HealthManager>();

			if (hm) {
				this.attacking.Add(hm);
				
			}			
		}

		public void OnTriggerExit2D(Collider2D col) {
			var go = col.gameObject;
			var hm = go.GetComponent<HealthManager>();

			if (hm) {
				this.attacking.Remove(hm);
			}			
		}

		

		private HitInstance GenHit() {
			int basedmg;
			switch (PlayerData.instance.nailSmithUpgrades) {
				case 0: basedmg = 1; break;
				case 1: basedmg = 2; break;
				case 2: basedmg = 3; break;
				case 4: basedmg = 7; break;
				default: basedmg = 0; break;
			}

			basedmg = Charms.CharmEffects.HandleCharmedSpells(basedmg);

			var hit = new HitInstance() {
				DamageDealt = basedmg,
				AttackType = AttackTypes.Spell,
				Direction = 90.0f,
				CircleDirection = false,
				Source = this.gameObject,
				Multiplier = 1.0f,
			};
			return hit;
		}
	}
}
