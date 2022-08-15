﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CharmCrab.Charms {
	class Debuffs: MonoBehaviour {
		private Bleed bleed;
		private SoulEater seat;
		private SoulCatcher scatch;
		private Infested infested;


		public void Disable() {
			Charms.SpellCollider.Remove(this.gameObject);
		}

		public void Start() {

		}

		public void Update() {
			if (this.seat != null) {
				this.seat.Update();
			}

			if (this.bleed != null) {
				this.bleed.Update();
			}

			if (this.scatch != null) {
				this.scatch.Update();
			}

			if (this.infested != null) {
				this.infested.Update();
			}
		}

		public void StackBleed() {
			if (this.bleed == null) {
				this.bleed = new Bleed(this.gameObject.GetComponent<HealthManager>());
			}

			this.bleed.increment();
		}

		public void SoulEat() {
			if (this.seat == null) {
				this.seat = new SoulEater(this.gameObject.GetComponent<HealthManager>());
			} else {
				HeroController.instance.AddMPCharge(3);
			}
		}

		public void SoulCatch(int i) {
			if (this.scatch == null) {
				this.scatch = new SoulCatcher(this.gameObject.GetComponent<HealthManager>());
			}
			this.scatch.ApplyDamage(i);
		}

		public void Infest() {
			if (this.infested == null) {
				this.infested = new Infested();
			}
			this.infested.Stack();
		}

		public void InfestAttack() {
			if (this.infested != null) {
				this.infested.SoulApply();
			}
		}

		public void SoulCatchDmg() {
			if (this.scatch != null) {
				this.scatch.DealDamage();
			}
		}

		private class SoulCatcher {
			public readonly float DecayRate = 12;
			public readonly float Transferrence = 0.25f;
			private float dmgApplied = 0;
			private HealthManager hm;

			public SoulCatcher(HealthManager hm) {
				this.hm = hm;
			}

			public void ApplyDamage(int i) {
				this.dmgApplied += i;
			}

			public void Update() {
				this.dmgApplied -= (DecayRate * Time.deltaTime);
				if (dmgApplied < 0) {
					this.dmgApplied = 0;
				}
			}

			public void DealDamage() {
				Modding.Logger.Log("Applying Catcher Damage " + (int)(this.dmgApplied * Transferrence));
				this.hm.Hit(this.GenHit());
			}

			private HitInstance GenHit() {
				int dmg = (int)(this.dmgApplied * Transferrence);
				var hit = new HitInstance() {
					DamageDealt = dmg,
					AttackType = AttackTypes.Spell,
					Direction = 0,
					CircleDirection = false,
					Source = HeroController.instance.gameObject,
					Multiplier = 1.0f,
					IgnoreInvulnerable = true,
				};

				return hit;
			}
		}

		private class SoulEater {
			public readonly float TICK = 5;
			public readonly int CHARGE = 5;

			private HealthManager hm;
			private float duration;

			public SoulEater(HealthManager hm) {
				this.hm = hm;
				this.duration = 0;
			}

			public void Update() {
				if (this.duration >= TICK) {
					Modding.Logger.Log("Apply Soul eater Tick");
					var hit = this.GenHit(1);
					hm.Hit(hit);
					this.duration = 0;
					HeroController.instance.AddMPCharge(CHARGE);
				} else {
					this.duration += Time.deltaTime;
				}
			}

			private HitInstance GenHit(uint dmg) {
				var hit = new HitInstance() {
					DamageDealt = (int) dmg,
					AttackType = AttackTypes.Spell,
					Direction = 0,
					CircleDirection = false,
					Source = HeroController.instance.gameObject,
					Multiplier = 1.0f,
					IgnoreInvulnerable = true,
				};

				return hit;
			}
		}

		private class Bleed {
			public readonly float DURATION = 2;

			private uint stacks;
			private HealthManager hm;
			private float duration;

			public Bleed(HealthManager hm) {
				this.hm = hm;
			}

			public void increment() {
				this.duration = 0;
				this.stacks += 1;
			}

			public uint Stacks {
				get { return this.stacks; }
			}

			public void Update() {
				if (this.stacks == 0) {
					return;
				}

				var t = duration + Time.deltaTime;
				if (t < DURATION) {
					// Check to see if a full second has passed.
					if ((int)duration < (int)t) {
						var hit = this.GenHit(stacks);
						hm.Hit(hit);
					}
					duration = t;
				} else {
					this.duration = DURATION;
					this.stacks = 0;
				}
			}

			private HitInstance GenHit(uint stacks) {
				int dmg = ((int)stacks) * (1 + PlayerData.instance.nailSmithUpgrades);
				var hit = new HitInstance() {
					DamageDealt = dmg,
					AttackType = AttackTypes.Spell,
					Direction = 0,
					CircleDirection = false,
					Source = HeroController.instance.gameObject,
					Multiplier = 1.0f,
					IgnoreInvulnerable = true,
				};

				return hit;
			}
		}

		private class Infested {
			public readonly int SoulPerStack = 15;
			public readonly float Decay = 1;
			public readonly float StackThreshold = 10;
			public readonly float DecayDelay = 2;
			private float stacks = 0;
			private float ticks = 0;
			private float delay = 0;

			public void Update() {
				this.delay += Time.deltaTime;

				if (this.delay >= DecayDelay) {

					this.ticks -= Time.deltaTime / Decay;

					if (this.ticks < 0) {
						this.stacks -= Time.deltaTime / Decay;
					}


					this.stacks = Math.Max(0, this.stacks);
					this.ticks = Math.Max(0, this.ticks);
				}
			}

			public void Stack() {
				this.ticks += 1;
				this.delay = 0;

				if (this.ticks >= StackThreshold) {
					this.stacks += 1;
					this.ticks = 0;
				}
			}

			public void SoulApply() {
				if (this.stacks >= 1) {
					PlayerData.instance.AddMPCharge(SoulPerStack);
					this.stacks = Math.Max(0, this.stacks - 1);
				}
			}
		}
	}
}