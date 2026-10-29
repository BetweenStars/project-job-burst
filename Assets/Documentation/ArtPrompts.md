# 아트 기록의 적용 범위

갱신: 2026-09-15.

- 아래 프롬프트는 과거 `Scripts/CardSurvival` / `Prototype.unity`용 생성 기록으로 원문을 보존한다. 이번 문서 갱신에서 이미지를 새로 만들거나 기존 이미지의 출처를 재검증하지 않았다.
- 현재 개발 대상은 `Scripts/Runtime` / `GameScene.unity`다. 아래의 “캐릭터 애니메이션 없이 정적인 카드 그림”은 과거 아트의 사용 방식이며 현재 전체 유닛의 표현 규칙이 아니다.
- 현재 `UnitAnimationController`는 `StartAttack`, `ApplyAttack`, `EndAttack` 이벤트에 따라 준비/타격/Idle 스프라이트를 교체한다. 병사 프리팹의 세 이벤트 연결을 확인했고, 사용자는 병사의 모션을 작업한 상태다. 모든 유닛의 모션이 완성된 것은 아니다.
- 공격 타겟이 선딜 중 사라져도 타격 스프라이트와 후딜을 유지하도록 `UnitCombat`의 null 처리 경로가 수정됐다. 실제 플레이 재검증은 별도다.
- 현재 스프라이트들의 신규 제작 프롬프트나 출처를 과거 프롬프트로 추정하여 덧붙이지 않는다. 현재 구현 상태는 [README](README.md)를 참고한다.

---

# 과거 생성 프롬프트 원문

# 아트 제작 기록

내장 imagegen 도구로 생성한 독자적인 종이 카드 게임 일러스트입니다. 원본 게임 아트 파일은 사용하지 않았습니다. 결과 파일은 `Assets/Resources/CardSurvival/Art`에 보관합니다. 캐릭터 애니메이션 없이 정적인 카드 그림으로 사용합니다.

스타일 참고: [Sokpop의 Stacklands 공식 소개](https://sokpop.itch.io/stacklands). 기존 게임의 이미지나 캐릭터 파일을 복사하지 않고 새로운 성·시설·캐릭터·마법 그림을 생성했습니다.

## 생성 프롬프트

### castle

Use case: stylized-concept. Asset type: isolated 2D Unity game sprite, original fantasy paper-cutout castle illustration, no animation. Primary request: central friendly little medieval castle for a cozy card-survival game, Stacklands-like chunky wobbly dark brown hand-ink outlines and flat watercolor gouache fill on warm paper. A compact cream stone keep, two short towers, reddish terracotta roofs, tiny teal pennants, arched wooden gate, very sparse details, charming squat proportions, slightly elevated front three-quarter view readable as an icon at 150 pixels. Full building centered and entirely visible with ample transparent margins. Genuine transparent background, no ground rectangle, no scene, no text, no UI, no frame, no drop shadow. Muted warm cream, rust red, sage green, ink brown. Single sprite only, high readability, hand-crafted indie boardgame look; original design.

### goblin

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a small mischievous green goblin with long pointed ears, short rusty dagger, little patched ochre tunic, broad round head and tiny legs.

### orc

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a squat muscular olive green orc with small tusks, leather armor, a broad battered iron axe, sturdy heavy silhouette.

### cyclops

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a big round one-eyed cyclops boss, one very large ivory eye, sandy peach skin, shaggy hair, leather loincloth and an enormous wooden club, intimidating but charming.

### soldier

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a tiny medieval infantry guard with cream steel helmet, round teal shield and short sword, red scarf, simple stout shape.

### archer

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a tiny medieval archer in sage green hood and brown boots, holding a small wooden bow, quiver, a single expressive face.

### hero

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a confident female fantasy knight hero with short auburn hair, ivory armor, a teal cape, bright golden sword, heroic but cute stout silhouette.

### ranger

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a male fantasy adventurer hero with messy brown hair, ochre cape, leather armor, crossbow, an expressive friendly face.

### fire

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: one magical red-orange fireball, a chunky round flame with curling flame tails and little gold embers, boldly inked, an isolated spell illustration.

### heal

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: one magical healing symbol made of a luminous sage green leafy sprig inside a gentle turquoise circle with tiny gold stars, an isolated spell illustration.

### barracks

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a compact medieval barracks building, squat cream timber-frame house with rusty red roof, teal shield hanging above the door and two crossed wooden spears.

### guild

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a compact cozy adventurers guild tavern building, cream half-timbered walls, ochre roof, green hanging sign with a simple sword emblem, warm lit window.

### academy

Use case: stylized-concept. Asset type: single isolated illustration for a 2D fantasy Unity game card. Style: original Stacklands-inspired cozy indie boardgame art, chunky imperfect dark-brown ink outlines, extremely simple flat muted gouache colors and subtle warm paper texture. Full subject centered, entirely visible, ample transparent margins. Slight front three-quarter view, readable at 80 pixels, clear bold silhouette. Genuine transparent background. No background, no scene, no card frame, no lettering, no watermark, no shadow rectangle, no animation. Match warm cream, rust, sage, teal, ochre palette. Subject: a compact little wizard academy tower building, lavender conical roof, cream stone walls, teal arched door, a gold crescent finial and a small telescope.
