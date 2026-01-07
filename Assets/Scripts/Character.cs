using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    const float CharacterJumpPower = 7f;
    const int MaxJump = 2;
    int RemainJump = 0;
    GameManager GM;

    void Awake()
    {
        // Hierarchy에 있는 GameManager 오브젝트를 찾아 스크립트를 가져옵니다.
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // 마우스 좌클릭(0) 시 남은 점프 횟수가 있다면 점프 실행
        if (Input.GetMouseButtonDown(0) && RemainJump > 0)
        {
            RemainJump--; // 점프 횟수 차감
            Jump(CharacterJumpPower);
        }
    }

    // 물리적인 힘을 가해 점프시키는 함수
    void Jump(float power)
    {
        // 속도를 초기화하고 힘을 주어야 일정한 점프 높이가 유지됩니다.
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        GetComponent<Rigidbody2D>().AddForce(new Vector3(0, power, 0), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // 1. 발판(Platform)에 닿으면 점프 횟수 초기화
        if (col.gameObject.CompareTag("Platform"))
        {
            RemainJump = MaxJump;
        }
        // 2. 장애물(Obstacle)에 닿으면 게임 오버 처리
        else if (col.gameObject.CompareTag("Obstacle"))
        {
            GM.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 3. 포인트(Point) 아이템에 닿으면 점수 획득 및 아이템 삭제
        if (col.CompareTag("Point"))
        {
            GM.GetPoint(1);
            Destroy(col.gameObject);
        }
    }
}