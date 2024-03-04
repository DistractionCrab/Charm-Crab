using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CharmCrab.Charms {
	class Debuffs: MonoBehaviour {
		private Bleed bleed;
		private SoulEater seat;
		private SoulCatcher scatch;
		private Infested infested;
		private Cooldown devcd = new Cooldown(10);
		private HealthManager hm;

		public void Disable() {
			Charms.SpellCollider.Remove(this.gameObject);
		}

		public void Start() {
			this.hm = this.gameObject.GetComponent<HealthManager>();
		}

		public void Update() {
			if (this.hm.hp <= 0) {
				Destroy(this);
			}

			this.devcd.Update();
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

		public void Devourer() {
			if (this.devcd.Available) {
				if (this.bleed != null) {
					this.bleed.DealDamage(3);
					this.bleed.Reset();
				}

				if (this.seat != null) {
					this.seat.DealDamage();
				}

				if (this.infested != null) {
					this.infested.Stack();
				}
				this.devcd.Reset();
			}
		}

		public void StackBleed() {
			if (this.bleed == null) {
				this.bleed = new Bleed(this.gameObject.GetComponent<HealthManager>());
			}

			this.bleed.Increment();
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
			public readonly float TwisterDecayRate = 24;
			public readonly float Transferrence = 0.25f;
			private float dmgApplied = 0;
			private readonly HealthManager hm;

			public SoulCatcher(HealthManager hm) {
				this.hm = hm;
			}

			public void ApplyDamage(int i) {
				this.dmgApplied += i;
			}

			public void Update() {
				var rate = DecayRate;

				if (CharmData.Equipped(Charm.SpellTwister) || CharmData.Equipped(Charm.ShamanStone)) {
					rate = TwisterDecayRate;
				}

				this.dmgApplied -= (rate * Time.deltaTime);

				if (dmgApplied < 0) {
					this.dmgApplied = 0;
				}
			}

			public void DealDamage() {
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
					this.DealDamage();
					this.duration = 0;
				} else {
					this.duration += Time.deltaTime;
				}
			}

			public void DealDamage() {
				hm.Hit(this.GenHit(1));				
				HeroController.instance.AddMPCharge(CHARGE);
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
			public readonly float DURATION = 3;

			private uint stacks;
			private HealthManager hm;
			private float duration;

			public Bleed(HealthManager hm) {
				this.hm = hm;
			}

			public void Increment() {
				if (!this.hm.IsInvincible) {
					this.duration = 0;
					this.stacks += 1;
				}				
			}

			public uint Stacks {
				get { return this.stacks; }
			}

			public bool DealDamage(int mult=1) {
				if (this.stacks == 0) {
					return false;
				} else if (this.hm.hp <= 0) {
					//return false;
				}

				//hm.Hit(this.GenHit(stacks));
				var dmg = this.GenHit(stacks).DamageDealt * mult;
				hm.ApplyExtraDamage(dmg);
				var flash = hm.gameObject.GetComponent<SpriteFlash>();
				if (flash != null) {
					flash.flashInfected();
				}

				return true;
			}

			public void Update() {
				if (this.stacks == 0) {
					return;
				}
				if (this.hm.IsInvincible) {
					return;
				}

				var t = duration + Time.deltaTime;
				if (t <= DURATION) {
					// Check to see if a full second has passed.
					if ((int)duration < (int)t) {
						this.DealDamage();
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
					AttackType = AttackTypes.Generic,
					Direction = 0,
					CircleDirection = false,
					Source = HeroController.instance.gameObject,
					Multiplier = 1.0f,
					IgnoreInvulnerable = true,
					IsExtraDamage = true
				};

				return hit;
			}

			public void Reset() {
				this.stacks = 0;
				this.duration = 0;
			}
		}

		private class Infested {
			public readonly int SoulPerStack = 15;
			public readonly float StackThreshold = 10;
			private float stacks = 0;
			private float ticks = 0;

			public void Update() {

			}

			public void Stack() {
				this.ticks += 1;

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
