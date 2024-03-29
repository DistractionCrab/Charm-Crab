﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CharmCrab.Charms {
	public class CloudTicker : MonoBehaviour {
		public List<GameObject> enemyList = new List<GameObject>();
		private static readonly float DMG_INTERVAL = 0.5f;
		private int damageAmt = 1;

		private Cooldown cd = new Cooldown(DMG_INTERVAL);

		private Action<GameObject> onDamage = (a) => { };

		public int DamageAmt {
			get {
				return this.damageAmt;
			}

			set {
				this.damageAmt = value;
			}
		}

		public Action<GameObject> OnDamage {
			get {
				return this.onDamage;
			}

			set {
				this.onDamage = value;
			}
		}

		private void OnEnable() {
			this.enemyList.Clear();
		}

		private void Update() {
			this.cd.Update();
			if (this.cd.Available) {
				//Modding.Logger.Log("Waffles");

				foreach (var enemy in this.enemyList) {
					enemy.GetComponent<HealthManager>().ApplyExtraDamage(this.damageAmt);
					this.onDamage(enemy);
				}
				this.cd.Reset();
			}
		}

		private void OnTriggerEnter2D(Collider2D otherCollider) {
			if (otherCollider.gameObject.GetComponent<HealthManager>() != null) {
				this.enemyList.Add(otherCollider.gameObject);
			}
			
		}

		private void OnTriggerExit2D(Collider2D otherCollider) {
			this.enemyList.Remove(otherCollider.gameObject);
		}

		public void EmptyDamageList() {
			this.enemyList.Clear();
		}

		public void SetDamageInterval(float newInterval) {
			this.cd = new Cooldown(newInterval);
		}


	}
}
