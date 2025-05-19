using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CharmCrab {
	class Functions {

		public static T AddIfNeeded<T>(GameObject obj) where T: Component {
			var t = obj.GetComponent<T>();
			if (t == null) {
				return obj.AddComponent<T>();
			} else {
				return t;
			}
		}

		public class HitDetectManager {
			private Dictionary<GameObject, float> hit = new Dictionary<GameObject, float>();
			private float cooldown;
			public HitDetectManager(float cd) {
				this.cooldown = cd;
			}

			public void Update() {
				var a = new List<GameObject>();
                Dictionary<GameObject, float> new_hit = new Dictionary<GameObject, float>();

                foreach (var k in hit.Keys) {
					var t = Time.deltaTime + hit[k];

					if (t >= this.cooldown) {
						a.Add(k);
					} else {
						new_hit.Add(k, t);
					}
				}

				foreach (var k in a) {
					hit.Remove(k);
				}

				foreach (var value in new_hit) {
					hit[value.Key] = value.Value;
				}
			}

			public void Register(GameObject obj) {
				if (!this.hit.ContainsKey(obj)) {
					this.hit.Add(obj, 0);
				}
			}

			public bool Hit(GameObject obj) {
				return this.hit.ContainsKey(obj);
			}
		}
	}
}
