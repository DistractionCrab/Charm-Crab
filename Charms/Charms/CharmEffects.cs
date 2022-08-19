﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Vasi;
using HutongGames.PlayMaker.Actions;
using HutongGames;
using HutongGames.PlayMaker;
using System.Reflection;

namespace CharmCrab.Charms {
	class CharmEffects: MonoBehaviour {
		public static CharmEffects instance;


		private HeavyBleed bleed;
		private MarkOfPride pride;
		private Fury fury;
		private NailMaster nmg;
		private DashMaster dashmaster;
		private SteadyBody steady;
		private LifebloodHeart joni;
		private Fireball fireball;
		private Wraith wraith;
		private Quake quake;
		private BaldurShell baldurshell;
		private Kingsoul kingsoul;
		private SporeShroom sporeshroom;
		private SharpShadow sharpshadow;
		private GrimmChild grimmchild;
		private GlowingWomb womb;
		private Flukes flukes;
		private InfusedBlade infused;
		private StalwartShell stalwart;

		private Functions.HitDetectManager hit;



		public void Start() {
			instance = this;
			this.hit = new Functions.HitDetectManager(0.25f);


			this.stalwart = new StalwartShell();
			this.infused = new InfusedBlade();
			this.flukes = new Flukes();
			this.womb = new GlowingWomb();
			this.bleed = new HeavyBleed();
			this.pride = new MarkOfPride();
			this.fury = new Fury();
			this.nmg = new NailMaster();
			this.dashmaster = new DashMaster();
			this.steady = new SteadyBody();
			this.joni = new LifebloodHeart();
			this.fireball = new Fireball();
			this.baldurshell = new BaldurShell();
			this.kingsoul = new Kingsoul();
			this.sporeshroom = new SporeShroom();
			this.sharpshadow = new SharpShadow();
			this.quake = new Quake();
			this.wraith = new Wraith();
			this.grimmchild = new GrimmChild();

			StartCoroutine(this.UpdateCostsPeriodically());
		}
		public void Update() {
			this.womb.Update();
			this.bleed.Update();
			this.pride.Update();
			this.dashmaster.Update();
			this.baldurshell.Update();
			this.kingsoul.Update();
			this.sporeshroom.Update();
			this.sharpshadow.Update();
			this.grimmchild.Update();
			this.joni.Update();
			this.hit.Update();
			this.flukes.Update();
		}

		public int BaseNailDamage() {
			if (PlayerData.instance.nailSmithUpgrades == 0) {
				return 15;
			} else if (PlayerData.instance.nailSmithUpgrades == 1) {
				return 22;
			} else if (PlayerData.instance.nailSmithUpgrades == 2) {
				return 29;
			} else if (PlayerData.instance.nailSmithUpgrades == 3) {
				return 36;
			} else if (PlayerData.instance.nailSmithUpgrades == 4) {
				return 42;
			} else {
				return 0;
			}
		}

		public int ComputeDamage(DamageType n) {
			int baseDMG = this.BaseNailDamage();

			switch (n) {
				case DamageType.GreatSlash: baseDMG = this.infused.AddDamage(baseDMG); break;
				case DamageType.Cyclone: baseDMG = this.infused.AddDamage(baseDMG); break;
				case DamageType.DashSlash: baseDMG = this.infused.AddDamage(baseDMG); break;
				default: break;
			}

			baseDMG += this.pride.DmgBonus;
			

			switch (n) {
				case DamageType.Slash: baseDMG = (int) (this.nmg.Mult(n) * baseDMG); break;
				case DamageType.GreatSlash: baseDMG = (int)(this.nmg.Mult(n) * baseDMG); break;
				case DamageType.Cyclone: baseDMG = (int)(this.nmg.Mult(n) * baseDMG); break;
				case DamageType.DashSlash: baseDMG = (int)(this.nmg.Mult(n) * baseDMG); break;
				case DamageType.FireBall: baseDMG = this.fireball.Damage(); break;
				case DamageType.Dive: baseDMG = this.quake.Damage(); break;
				case DamageType.DiveExtra: baseDMG = this.quake.MiniDamage(); break;
				case DamageType.Shriek: baseDMG = this.wraith.Damage(); break;
				case DamageType.SharpShadow: baseDMG = this.sharpshadow.Damage(); break;
				case DamageType.Flukes: baseDMG = this.flukes.Damage(); break;
				default: break;
			}


			switch (n) {
				case DamageType.FireBall: baseDMG = HandleCharmedSpells(baseDMG); break;
				case DamageType.Dive: baseDMG = HandleCharmedSpells(baseDMG); break;
				case DamageType.DiveExtra: baseDMG = HandleCharmedSpells(baseDMG); break;
				case DamageType.Shriek: baseDMG = HandleCharmedSpells(baseDMG); break;
				case DamageType.SharpShadow: baseDMG = HandleCharmedSpells(baseDMG); break;
				case DamageType.Flukes: baseDMG = HandleCharmedSpells(baseDMG); break;
				default: break;
			}

			baseDMG = (int)(baseDMG * this.fury.Mult);
			return baseDMG;
		}

		IEnumerator UpdateCostsPeriodically() {
			while (true) {
				Spells.SpellUpdater.UpdateSpellCosts();
				yield return new WaitForSeconds(1);
			}
		}

		public static int HandleShamanSpells(int baseDMG) {
			if (PlayerData.instance.GetBool("equippedCharm_19")) {
				baseDMG *= 4;
			}
			return baseDMG;
		}

		public static int HandleTwisterspells(int baseDMG) {
			if (PlayerData.instance.GetBool("equippedCharm_33")) {
				baseDMG /= 2;
			}

			return baseDMG;
		}

		public static int HandleCharmedSpells(int baseDMG) {
			baseDMG = HandleShamanSpells(baseDMG);
			baseDMG = HandleTwisterspells(baseDMG);

			return baseDMG;
		}

		private void SoulCatcherDamage(GameObject obj) {
			var d = obj.GetComponent<Debuffs>();

			if (d != null) {				
				d.SoulCatchDmg();
			}
		}

		public void SlashHitHandler(Collider2D col, GameObject slash) {
			if (!this.hit.Hit(col.gameObject)) {
				this.bleed.SlashHitHandler(col, slash);
				this.joni.SlashHitHandler(col, slash);
				SpellCollider.Apply(col.gameObject);
				this.SoulCatcherDamage(col.gameObject);

				var d = col.gameObject.GetComponent<Debuffs>();
				if (d != null) {
					d.InfestAttack();
					if (CharmCrab.Settings.Equipped(NewCharms.AfflictedDevourer)) {
						d.Devourer();
					}
				}				
			}
			this.hit.Register(col.gameObject);
		}

		public int TakeDamage(ref int hazard, int dmg) {
			//dmg += 1;
			this.pride.TakeDamage(dmg);
			dmg = this.steady.TakeDamage(ref hazard, dmg);
			dmg = this.stalwart.TakeDamage(ref hazard, dmg);

			if (BaldurShell.Active()) {
				dmg = 0;
			}

			return dmg;
		}

		public GameObject UpdateSpells(GameObject obj) {
			if (obj.name.Contains("Spell Fluke")) {
				var ctrl = obj.GetComponent<SpellFluke>();
				if (ctrl != null) {
					// Need to use reflection to update private damage variable for fluke objects;
					
					ctrl.damager.OnTriggerEntered += delegate(Collider2D collider, GameObject sender) {
						this.flukes.Hit(collider.gameObject);
					};
					FieldInfo fi = ctrl.GetType().GetField("damage", BindingFlags.NonPublic | BindingFlags.Instance);
					fi.SetValue(ctrl, this.ComputeDamage(DamageType.Flukes));
				} else {
					Modding.Logger.Log("Found Non-Fluke to update with Fluke Details.");
				}
			}


			return obj;
		}

		public void OnColliderCreate(GameObject obj) {
			
			if (obj.name == "Fireball2 Spiral(Clone)") {
				obj.AddComponent<FireballCollider>();
			}

			if (obj.name == "Weaverling(Clone)") {
				obj.AddComponent<Weaverlings>();
			}

			if (obj.name == "Hit L" || obj.name == "Hit R" || obj.name == "Hit U" || obj.name == "Hit D") {
				var parent = obj.transform.parent;
				if (parent != null) {
					if (parent.name == "Scr Heads 2") {
						obj.AddComponent<WraithCollider>();
					}
					if (parent.name == "Q Slam 2" || parent.name == "Q Mega") {
						obj.AddComponent<QuakeCollider>();
					}
				}
			}
		}

		public HitInstance DamageRecalc(HutongGames.PlayMaker.Fsm owner, HitInstance hitInst) {
			if (owner.GameObject.name == "Slash" || owner.GameObject.name == "UpSlash" || owner.GameObject.name == "DownSlash" || owner.GameObject.name == "AltSlash") {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.Slash);
			} else if (owner.GameObject.name == "Great Slash") {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.GreatSlash);
			} else if ((owner.GameObject.name == "Hit L" || owner.GameObject.name == "Hit R") && hitInst.AttackType == AttackTypes.Nail) {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.Cyclone);
			} else if ((owner.GameObject.name == "Hit L" || owner.GameObject.name == "Hit R")) {
				var parent = owner.GameObject.transform.parent;
				if (parent.name == "Q Slam 2" || parent.name == "Q Mega") {
					hitInst.DamageDealt = this.ComputeDamage(DamageType.Dive);
				}
			} else if (owner.GameObject.name == "Q Fall Damage") {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.DiveExtra);
			} else if (owner.GameObject.name.Contains("Fireball")) {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.FireBall);
			} else if (owner.GameObject.name == "Q Fall Damage") {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.Dive);
			} else if (owner.GameObject.name == "Hit L" || owner.GameObject.name == "Hit R" || owner.GameObject.name == "Hit U" || owner.GameObject.name == "Hit D") {
				var parent = owner.GameObject.transform.parent;
				if (parent != null) {
					if (parent.name.Contains("Scr Heads")) {
						hitInst.DamageDealt = this.ComputeDamage(DamageType.Shriek);
					}
					if (parent.name.Contains("Q Slam") || parent.name.Contains("Q Mega")) {
						hitInst.DamageDealt = this.ComputeDamage(DamageType.Dive);
					}
				}
			} else if (owner.GameObject.name == "Sharp Shadow") {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.SharpShadow);
			} else {
				//Log("Hit source name: " + owner.GameObject.name);
			}

			return hitInst;
		}
	}
}
