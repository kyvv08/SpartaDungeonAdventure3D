# SpartaDungeonAdventure3D

✅ 1. 캐릭터 이동
WASD 키로 이동

카메라 회전과 연동된 방향 기반 이동

중력 및 점프 기능 포함

✅ 2. 벽타기(클라이밍)
앞에 감지되는 벽이 특정 조건을 만족하면 벽타기 가능

Raycast 2방향 체크로 벽 감지

감지 기준은 플레이어 기준 전방 (transform.forward)을 사용해 정확도 향상

✅ 3. 충돌 감지 및 레이캐스트
플레이어 주변에서 레이 발사 후 벽 혹은 바닥 감지

Ray 길이, 방향, 오프셋 등을 조정하여 트러블슈팅 진행

일정 주기로 체크해서 플레이어가 레이저와 충돌 시 데미지를 주도록 레이저 트랩 구현

✅ 4. 코루틴을 통한 아이템 효과 적용 수행
일정 시간 동안 효과 반영 후 돌아오는 코루틴 함수

✅ 5. Input System을 C# Event로 설정하여 플레이어가 활성화/비활성화 될 이벤트를 등록 / 해제


===== 트러블 슈팅 =====

**❌ 플레이어가 발판 위에서 자연스럽게 안 따라감**
문제
발판 위에 있을 때 플레이어가 발판을 따라 이동하지 않고 제자리에 떨어지는 문제

해결
매 프레임 발판의 velocity를 플레이어의 이동에 반영:

if (isOnMovingFlatform && curPlatform != null)
        {
            dir += curPlatform.velocity;
            if(curPlatform.IsMovingY())
            {
                dir.y = curPlatform.velocity.y;
            }
        }

**❌ 플레이어가 타고 오를 수 있는 벽을 감지하지 못함**
이슈	            원인	                    해결
벽 감지가 안 됨	  Vector3.forward 사용	    transform.forward로 변경
감지 범위 불안정	  레이 거리 짧음	            0.2f 이상으로 조정

