# 포플 영상보고 들어오신 분을 위한  github 파일 검색 팁.

### Player의 GroundCheck 코드
	PlayerState.cs 파일의 BasePlayerState class의 IsStandGround() 함수

### LedgeClimbe 관련 코드
	PlayerState.cs 파일의 BaseFall class의 IsGrabLedge() 함수

### 특정한 구역에서 CamLookUpOrDown 구현 코드
	CamFollowObjectController.cs
	CamLookUpDownZoneDetectionController.cs

### Player의 Movement Animation 재생 관련 코드
	PlayerMovementAnimController.cs
	PlayerController.cs

### 몬스터 HitEffetAnimation 코드
	MonsterHitAnimController.cs

### 몬스터의 Controller들 관련 코드
	ArcherController.cs
	BlasterController.cs
	CagedShockerController.cs
	FlamerController.cs
	GunnerController.cs
	HSlicerController.cs
	WardenController.cs
	ColossalBossMonsterController.cs

### 몬스터 Hit시에 특정 색깔로 번뜩하는 부분 코드
	MonsterFlashController.cs 파일 확인.

### 플레이어, 스킬, 몬스터 정보 Json파일 로드하는 부분
	DataManager.cs
	Data.Contents.cs

### 플레이어, 스킬, 몬스터 정보 Json 파일
	Resources/Data 안에서 Json 파일들 확인.

### 플레이어, 몬스터 상태머신패턴
	NormalMonsterController.cs
	PlayerController.cs
	PlayerState.cs
	MonsterState.cs

### 라이트 점진적으로 꺼지는 코드
	LightController.cs

### 스킬관련 코드들
	접두어 Skill_
	접두어 UI_Skill 검색하여 확인.

### UI, 인벤토리 관련 코드들
	UIManager.cs
	접두어 UI_Inventory 검색하여 확인.

### Scene 관련 코드들
	Scene 단어로 검색.
