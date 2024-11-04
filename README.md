
# 포플 정리

## 1. 개요
- 사용언어 : C#
- 사용엔진 : Unity
- 사용 플러그인 : DoTween2Pro
- 사용 에셋 : [Penusbmic의 Dark시리즈](https://itch.io/c/1748382/all-of-the-dark-series-patreon-tier-2-series-click-here/)

## 2. 코드 목차
1. Player의 GroundCheck 코드
    - [IsStandGround](https://github.com/dlwlgn21/Unity2DPortfolio/blob/d1a1139ef97a6c9141af5f7b15228c5e2b54e6b7/Assets/Scripts/StateMachines/Player/PlayerStates.cs#L52)
2. Player의 LedgeClimbe 코드
    - [IsGrabLedge](https://github.com/dlwlgn21/Unity2DPortfolio/blob/d1a1139ef97a6c9141af5f7b15228c5e2b54e6b7/Assets/Scripts/StateMachines/Player/PlayerStates.cs#L382)
	
3. 특정 구역에서 CamLookUpDown 코드
    - [CamLookUpDownZoneDetectionController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/CamLookUpDownZoneDetectionController.cs)
    - [CamFollowObjectController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Cam/CamFollowObjectController.cs)
    
4. Monster, Player의 HitEffetAnimation 코드
    - [MonsterHitAnimController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/MonsterHitAnimController.cs)
    - [PlayerMovementAnimController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/PlayerMovementAnimController.cs)
	
5. Monster, Player의 상속구조
    - [BaseCharacterControlloer.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/BaseCharacterController.cs)
        - [PlayerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/PlayerController.cs)
        - [BaseMonsterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/BaseMonsterController.cs)
           - [NormalMonsterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/NormalMonsterController.cs)
		     - [ArcherController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/ArcherController.cs)
			 - [BlasterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/BlasterController.cs)
			 - [CagedShockerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/CagedShockerController.cs)
			 - [FlamerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/FlamerController.cs)
			 - [GunnerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/GunnerController.cs)
			 - [HSlicerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/HSlicerController.cs)
			 - [WardenController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/WardenController.cs)
	       - [ColossalBossMonsterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/Monsters/ColossalBossMonsterController.cs)
	
6. Monster Hit시에 특정 Color로 Material 교체 코드
	- [MaterialFlashController](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Flasher/MaterialFlashController.cs)
	- [MonsterFlashController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Flasher/MonsterFlashController.cs)
	
7. Data를 어떻게 로드하고 관리하는지 DataManager, Data.Contents 코드.
	- [DataManager.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Managers/Core/DataManager.cs)
	- [Data.Contents.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Data/Data.Contents.cs)
	
8. Player, Skill, Monster, Item Json 파일
	- [Data Folder](https://github.com/dlwlgn21/Unity2DPortfolio/tree/main/Resources/Data)
	
9. Player, Monster, StateMachine 코드
	- [PlayerState.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/StateMachines/Player/PlayerStates.cs)
	- [PlayerController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/PlayerController.cs)
	- [MonsterState.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/StateMachines/Monsters/MonsterStates.cs)
	- [NormalMonsterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/NormalMonsterController.cs)
	
10. Light 점진적으로 꺼지는 코드
	- [LightController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Controller/LightController.cs)
	
11. Skill 관련 코드들.
    - [SkillManager.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Managers/Contents/PlayerSkillManager.cs)
	
12. Skill 상속구조 (Object와 Controller 컴포지션 관계로 관리, )
	- [Skill_BaseObject.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Objects/Skill_BaseObject.cs)
		- [Skill_BlackFlameObject.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Objects/Skill_BlackFlameObject.cs)
		- [Skill_SwordStrikeObject.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Objects/Skill_SwordStrikeObject.cs)
		- [Skill_SpawnReaperObject.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Objects/Skill_SpawnReaperObject.cs)
	- [Skill_SpawnShooterObject.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Objects/Skill_SpawnShooterObject.cs)
	- [Skill_BaseController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Controllers/Skill_BaseController.cs)
		- [Skill_BlackFlameController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Controllers/Skill_BlackFlameController.cs)
		- [Skill_SwordStrikeController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Controllers/Skill_SwordStrikeController.cs)
		- [Skill_SpawnReaperController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Controllers/Skill_SpawnReaperController.cs)
		- [Skill_SpawnShooterController.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Contents/Player/PlayerSkill/Controllers/Skill_SpawnShooterController.cs)
	
13. Skill UI 관련 코드들
    - [UI_SkillTree.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Skill/UI_SkillTree.cs)
	- [UI_Skill_Description.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Skill/UI_Skill_Description.cs)
	- [UI_Skill_Icon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Skill/UI_Skill_Icon.cs)
	- [UI_Skill_SlotIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Skill/UI_Skill_SlotIcon.cs)
	- [UI_Skill_Slot.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Skill/UI_Skill_Slot.cs)
	
14. Inventory 관련 코드들
    - [UI_Inventory.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory.cs)
    - [UI_Inventory_ItemDesc.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_ItemDesc.cs)
	- [UI_Inventory_BaseItemIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_BaseItemIcon.cs)
		- [UI_Inventory_ItemIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_ItemIcon.cs)
		- [UI_Inventory_EquipableItemIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_EquipableItemIcon.cs)
		- [UI_Inventory_ItemDiscardIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Inventory/UI_Inventory_ItemDiscardIcon.cs)
	- [UI_Inventory_ItemSlot.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_ItemSlot.cs)
	- [UI_Inventory_EquipableItemSlot.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_Inventory_EquipableItemSlot.cs)
	- [UI_Inventory_ItemDiscardSlot.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/Inventory/UI_Inventory_ItemDiscardSlot.cs)
	
15. Consumable(물약) 관련 코드들
    - [UI_PlayerConsumableIcon.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_PlayerConsumableIcon.cs)
	- [UI_PlayerConsumableSlot.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/UI/UI_PlayerConsumableSlot.cs)
	
16. Scene 상속구조
	- [BaseScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/BaseScene.cs)
		- [PlayScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/PlayScene.cs)
			- [TutorialScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/TutorialScene.cs)
			- [AbandonRoadScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/AbandonRoadScene.cs)
			- [ColossalBossCaveScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/ColossalBossCaveScene.cs)
		- [MainMenuScene.cs](https://github.com/dlwlgn21/Unity2DPortfolio/blob/main/Assets/Scripts/Scenes/MainMenuScene.cs)