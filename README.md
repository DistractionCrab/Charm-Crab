# Charm Crab
 Charm Rework that Includes new Spells


# Installation
Install [Scarab](https://github.com/fifty-six/Scarab) to install mods for the game. Install Vasi and
SFCore through Scarab. (For now manual installation) In Scarab click on "Open Mods" and create Folder "CharmCrab" and drop the downloaded files into this folder. Open up Hollow Knight and play

# Interactivity with other Mods.
This mod doesn't attempt to play nicely with other overhaul mods, but can work with other mods, such as randomizer, which do not make complex changes to Player FSM and various variable manipulations.

# Gameplay Guide
Since this mod changes a few things, trying to play it like the vanilla game may get frustrating if you try to bash your head into a few walls. So in this section we'll go over a few things to keep in mind as you play.

- This is not a challenge mod; Somethings are more difficult to accomodate the fact they are not required very early. If you're having difficulty choose one of the different paths to progress the game. The changes are also integrated with the new charm mechanics, and designed to work well with what is available at the time; You will never be required to do +0 bosses that take 10 minutes or more.
- False Knight is not required, and is more difficult in the beginning. Use the fact you can break the wall to progress without the need to defeat them at +0 if they are giving you trouble. 
- Dung Defender is only required for Isma's; DD is quite a bit more difficult at +0 and few upgrades, however keep in mind there are *three* ways into the City of Tears, both providing upgrades and other goodies as you explore. Alternative paths are C-Dash+Claw and going above Salubra and getting the tram pass, both of which also allow access to Resting Grounds and City of Tears.
- Make sure to use defensive charms if the increased boss damage is giving you a hard time. The increased boss damage is not meant to be a *hard mode*. There is nothing in the mod intended to be hard, but rather motivate you trying alternative routes (as there are many in this game).
- Explore a lot; The rebalancing of enemies are designed for the charms and not intended to be a *hard mode*. If you do not take advantage of the knowledge you have and explore you will be put into very challenging situations. They are doable if you know the bosses well, and don't require you to defeat any boss hitless or is unreasonable at +0, but is relatively harder than the base game.

# Overview of changes:

General changes are modified charm mechanics, rebalances to accomodate the heavy changes, and some new 
charms obtained through special ways in the game. This mod designed as a way to help promote playstyles 
that isn't simply just the caster playstyle that dominates the vanilla game. To do this, we promote higher 
damage scaling, and upgrading your nail also affects your spells. So old, somewhat unhelpful or hurtful, mechanics are made to be more valuable when making choices. This is heavily inspired by how RPG's handle orthogonality of playstyles and one should find themselves make a charm build to accomodate melee, spell, hybrid, or summoner styles. 

Some rebalances of note is enemy health, as damage is much higher than before, and thus is much higher. Base soul gain has also been nerfed, and this includes the base soul gain while wearing Soul Catcher/Eater so soul has more complex mechanics to be gathered quickly. The goal of this is accomodate builds that care about soul and builds that don't; Soul while useful for healing will be harder to gather by default but some changed charms accomodate healing so focusing isn't a primary healing methodology but is still useful in cases.

Note that rebalancing is done over casual play; Knowing which charms are the best and knowing how to get certain charm very early are not balanced around. So becoming overpowered for some early stuff by ignoring it and collecting strong charms will not be nerfed. This is due to how the game can be traversed in creative and arguably valid ways (or just in many different ways by default).

Other changes of note are as follows:

- Soul gain on the nail is nerfed, but scales as you upgrade your nail.
- Spells scale with nail upgrades
- Many bosses have increase damage, but some damage is vanilla.
- New enemy health values.

# Charm changes

- Wayward Compass: Now costs 0
- Gathering Swarm: Now costs 0
- Stalwart Shell: Reduce damage taken by 1, to a minimum of 1.
- Soul Catcher: Applies the Soul Catcher Debuff
- Soul Eater: Applies the Soul Eater Debuff
- Shaman Stone: Quadruples spell damage, but requires a full soul orb (3x cost).
- Dash Master: Shadecloak also has no cooldown
- Sprintmaster: Now Costs 0
- Grubsong:                    
- Grubberfly's elegy:          
- Fragile Heart:               
- Fragile Strength: Fragile: a 133% dmg increase, but double damage taken. Unbreakable: 75% dmg increase, but you do not take increased damage.
- Fragile Greed: Increases geo gains by a larger amount, and ever 1.5s you can spend geo to increase your nail damage by that much. This happens automatically, and will increase your nail damage by up to its base amount each time you attack something. So don't get greedy with your nail.
- Spell Twister: Spell costs no soul, but deal half damage (dmg ratio very likely to change).
- Steady Body: Has a 25% chance to reduce all damage take to 0.
- Heavy Blow: Melee attacks inflict the Bleed debuff
- Quick Slash: 
- Long Nail: 
- Mark of Pride: Increase damage as you attck; Lose it when taking damage
- Fury of the Fallen: Increase damage by 25% per missing mask, also spell damage scales with fury.
- Thorns of Agony: Not yet changed (have plans, but need thinking due to recent overhaul)
- Baldur Shell: Spells now proc the effect, and reduces all damage taken to zero while active
- Fluke Nest: Now heals after enough strikes. (should hit hard, but not interact with soul eater/catcher)
- Defender's Crest: Unchanged (other effects modified by this charm are changed).
- Glowing Womb: Summon costs happens more frequently, costs more Soul, but can have up to 15 minions.
- Quick Focus: 
- Deep Focus: 
- Lifeblood Heart: Every (currently) 15 nailstrikes generates a lifeblood
- Lifeblood Core: 
- Joni's Blessing:
- Hiveblood: 
- Spore Shroom: Spawns on healing and any spell cast, inflicts the infested debuff             
- Sharp Shadow: Changed Damage (not necessarly proportionally better), and generates soul when hitting.
- Shape of Unn: Costs 0
- NMG: Increases Art damage also
- Weaversong: Increased damage and can proc some melee effects (like bleed).
- Dream Wielder: Buffs the Knight's damage on strick (50% for 10s)
- Dreamshield: Increase Shield count for nail upgrade lvl (Not yet implemented)
- Grimm Child: Increase Damage, and revamp scaling
- Carefree Melody: Convert to support minion (Not yet implemented)
- Kingsoul: Effect persists through voidheart and is stronger.


# Debuffs

- Bleed: Lasts 2 seconds, dealing damage every second, and can stack. Duration refreshed on hit.
- Soul Eater: Deals minor damage over time, and attacks and some spells will restore soul when the afflicted enemy is hit.
- Soul Catch: Spell Damage from most spells accumulates on the target, and melee damage will receive a boost proportional to the accumulated damage. This bonus decays quickly, but the debuff lasts indefinitely.
- Infested: Infested stacks can be struck by the nail to gain a large amount of soul. Soul can only be gained when a stack is available and a stack is applied every 10 ticks of infestation damage. These stacks decay over time, so one cannot save up a lot of stacks to gain soul as desired.

# New Charms
- Void Tendrils: Converts your shriek into a summon spell for Void Tendrils. Obtained by getting the journey entry for Void Tendrils.

- Aura of Purity: Turns your shriek into a spell that summons a persistent aura that will damage nearby enemies. This spell will consume all soul, regardless of charms, and damage and duration of this aura depends upon the soul consumed. This spell only interacts with soul catcher. Obtained by conversing with Bardoon after obtaining Void Heart.

- Afflicted Devourer : Charm that causes debuffs to proc regardless of their stacks. So Bleed will cause damage, Soul Eater will deal damage/provide Soul. Does not interact with Soul Catcher or infested. Obtained by conversing with the Midwife while having weaverlings (Equipped? You need to trigger the dialogue related to the charm).

- Soul Infused Blade: Causes Nail arts to consume soul (up to a specific amount), and increase damage based off of the soul consumed. Obtained by bringing the fragile flower to Emilitia.

# Known Bugs (and planned fixes)
- Hornet: Not all damage is boss-damage (also other bosses which spawn projectiles)
- Recycled enemies don't revert the maximum health.

# Art Credits

Credits for new Charm Icons go to [Endermint](https://www.twitch.tv/endermint)