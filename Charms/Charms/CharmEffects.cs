using System;
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
using SFCore;
using System.Reflection;
using CharmCrab.Enemy;
using SFCore.Utils;

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
		private Greed greed;
		private DreamWielder dreamWielder;

		private Functions.HitDetectManager hit;



		public void Start() {
			instance = this;
			this.hit = new Functions.HitDetectManager(0.25f);


			this.stalwart = new StalwartShell();
			this.greed = new Greed();
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
			this.dreamWielder = new DreamWielder();

			StartCoroutine(this.UpdateCostsPeriodically());
		}
		public void Update() {
			this.steady.Update();
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
			this.greed.Update();
			this.dreamWielder.Update();
		}

		public static int BaseNailDamage() {
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

		private static bool IsNailDamage(DamageType d) {
			return d == DamageType.Cyclone || d == DamageType.DashSlash || d == DamageType.GreatSlash || d == DamageType.Slash;
		}

		public int ComputeDamage(DamageType n) {
			int baseDMG = BaseNailDamage();

			switch (n) {
				case DamageType.GreatSlash: baseDMG = this.infused.AddDamage(baseDMG); break;
				case DamageType.Cyclone: baseDMG = this.infused.AddDamage(baseDMG); break;
				case DamageType.DashSlash: baseDMG = this.infused.AddDamage(baseDMG); break;
				default: break;
			}

			if (IsNailDamage(n)) {
				baseDMG = this.pride.DamageBonus(baseDMG);
				baseDMG = this.greed.DamageBonus(baseDMG);
				if (CharmData.Equipped(Charm.FragileStrength)) {
					baseDMG += 3 * baseDMG / 4;
				} else if (CharmData.Equipped(Charm.UnbreakableStrength)) {
					baseDMG += 3 * baseDMG / 4;
				}
			}			

			switch (n) {
				//case DamageType.Slash: baseDMG = (int) (this.nmg.Mult(n) * baseDMG); break;
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

			baseDMG = this.fury.DamageBonus(baseDMG);
			baseDMG = this.dreamWielder.Damage(baseDMG);
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

		public int OnPlayerSetInt(string target, int orig) {
			if (target == "geo") {
				int old = PlayerData.instance.GetInt("geo");
				if (orig < old) {
					return orig;	
				} else if (!PlayerData.instance.soulLimited && (CharmData.Equipped(Charm.FragileGreed) || CharmData.Equipped(Charm.UnbreakableGreed))) {
					orig = old + this.greed.GeoBonus(orig - old);			
				}
			}

			return orig;
		}


		public void SlashHitHandler(Collider2D col, GameObject slash) {
			var hm = col.gameObject.GetComponent<HealthManager>();

			if (hm != null) {
				if (!this.hit.Hit(col.gameObject)) {
					this.pride.ApplyStack();
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
			} else {
				// This situation we've hit something else; Need to update damage values if possible.
				// Function calls will check for validity of target before updating.
				EnemyHealth.UpdateSpikeBallDamage(col.gameObject);
			}
		}

		public int TakeDamage(ref int hazard, int dmg) {
			if (dmg == 0) {
				return 0;
			}
			
			dmg = this.steady.TakeDamage(ref hazard, dmg);
			dmg = this.stalwart.TakeDamage(ref hazard, dmg);
            this.pride.TakeDamage(dmg);

            //Modding.Logger.Log("pre-dmg = " + dmg);

            if (BaldurShell.Active()) {
				dmg = 0;
			}

			//if (CharmData.Equipped(Charm.FragileStrength)) {
			//	dmg *= 2;
			//}

			//Modding.Logger.Log("dmg = " + dmg);

			return dmg;
		}

		public GameObject UpdateSpells(GameObject obj) {
			if (obj.name.Contains("Spell Fluke")) {
				var ctrl = obj.GetComponent<SpellFluke>();                

                if (ctrl != null) {
                    // Need to use reflection to update private damage variable for fluke objects;					
                    ctrl.damager.OnTriggerEntered += delegate (Collider2D collider, GameObject sender) {
                        this.flukes.Hit(collider.gameObject);
                    };

                    FieldInfo fi = ctrl.GetType().GetField("damage", BindingFlags.NonPublic | BindingFlags.Instance);
					fi.SetValue(ctrl, this.ComputeDamage(DamageType.Flukes));
				} else {
					var FSM = obj.GetComponent<PlayMakerFSM>();

					if (FSM != null) {
						var Damager = SFCore.Utils.FsmUtil.GetFsmGameObjectVariable(FSM, "Damager");

						if (Damager == null) {
                            
                        } else {
							var DamagerObject = Damager.RawValue as GameObject;

							var Collider = DamagerObject.GetComponent<Collider2D>();

							if (Collider != null) {
								
                            } else {
                            }
						}
					}
				}
			}


			return obj;
		}

		public void OnColliderCreate(GameObject obj) {
			
			if (obj.name == "Hitbox") {
				var fsm = FSMUtility.LocateFSM(obj, "Send Event");
				var state = SFCore.Utils.FsmUtil.GetState(fsm, "Send Event");
                SFCore.Utils.FsmUtil.AddMethod(state, this.dreamWielder.DreamWielderAction);
			}

			// Possible names Fireball2 Sprial(Clone) and Fireball(Clone).
			if (obj.name.Contains("Fireball")) {
				obj.AddComponent<FireballCollider>();
			}

			// Weaverling damage hitbox; Make sure it belongs to the weaverlings also.
			if (obj.name == "Enemy Damager" && obj.transform.parent && obj.transform.parent.name == "Weaverling(Clone)") {
				//Modding.Logger.Log("Hitbox found for: " + obj.transform.parent);
				obj.AddComponent<Weaverlings>();
			}

			if (obj.name == "Hit L" || obj.name == "Hit R" || obj.name == "Hit U" || obj.name == "Hit D") {
				var parent = obj.transform.parent;
				if (parent != null) {
					if (parent.name == "Scr Heads 2" || parent.name == "Scr Heads") {
						obj.AddComponent<WraithCollider>();
					}

					if (parent.name == "Q Slam 2" || parent.name == "Q Mega") {
						obj.AddComponent<QuakeCollider>();
					}
				}
			}

			if (obj.name == "Q Fall Damage") {
				obj.AddComponent<QuakeCollider>();
			}
		}

		public int OnSoulGain(int num) {
			// This just undoes what the built-in Soul gain calculations do based off of charms. This is so the soul
			// gain from Soul Catcher/Eater isn't ridiculous with their new effects. This information comes directly from
			// the default code in the game.
			if (PlayerData.instance.GetInt("MPCharge") < PlayerData.instance.GetInt("maxMP")) {
				if (PlayerData.instance.GetBool("equippedCharm_20")) {
					num -= 3;
				}
				if (PlayerData.instance.GetBool("equippedCharm_21")) {
					num -= 8;
				}
			} else {
				if (PlayerData.instance.GetBool("equippedCharm_20")) {
					num -= 2;
				}
				if (PlayerData.instance.GetBool("equippedCharm_21")) {
					num -= 6;
				}
			}

			switch (PlayerData.instance.GetInt("nailSmithUpgrades")) {
				case 0: return 4;
				case 1: return 5;
				case 2: return 6;
				case 3: return 7;
				case 4: return 10;
				default: return num/2;
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
			} else if (hitInst.Source.name.Contains("Grubberfly Beam")) {
				hitInst.DamageDealt = this.ComputeDamage(DamageType.Slash) / 2;
			} else {
				//Log("Hit source name: " + owner.GameObject.name);
			}

			return hitInst;
		}
	}
}
