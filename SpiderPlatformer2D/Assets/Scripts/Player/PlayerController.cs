﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Config
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed ;
    [SerializeField] float chargeSpeed;
    [SerializeField] float timeBetweenCharge;
    [SerializeField] float timeBetweenStep;
    [SerializeField] float pullingForceMultiplier;
    [SerializeField] float grappleForceMultiplier;
    [SerializeField] float maxGrappleForce;
    [SerializeField] float grappleRadious;
    [SerializeField] float playerHealth;
    [SerializeField] float groundCheckRadius;
    [SerializeField] float bossGrappleForceMultiplier;
    [SerializeField] string[] stepSounds;
    [SerializeField] Boss boss;
    public Color poisonColor;
    public Color waterColor;
    public delegate void DestroySomeStuff();
    public static event DestroySomeStuff DestroyBoxesInPlayerController;
    public delegate void DestroyWeb();
    public static event DestroyWeb DestroyWebs;

    [HideInInspector] public float gravityDefaultValue;
    //Component Referances
    Rigidbody2D rigidBody;
    Grapple grapple;
    public Animator animator;
    public Slider HealthBar;
    public Image HealthBarImage;
    public GameObject DeadMenu;
    [SerializeField] GameObject webSnapParticle;
    [SerializeField] GameObject jumpParticle;
    [SerializeField] GameObject runParticle;
    [SerializeField] Transform groundCheck;

    [Header ("UI")]
    public GameObject RestoreParticleUI;
    public GameObject damageParticleUI;
    public GameObject poisonDamageParticleUI;
    public Canvas canvas;
    public TextMeshProUGUI achievementText;
    public TextMeshProUGUI tutorialText;


    //State
    [SerializeField] public bool isAlive = true;
    bool isJumping;
    bool isFacingRight;
    bool isGrounded;
    bool isPoisoned;
    bool canCharge=true;
    public bool isTouchingPlatforms = false;

    [Header("Wall Jump")]
    public LayerMask groundMask;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    public float jumpDelay = 2f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;
    float mx = 0;
    float runSpeedValue;
    float jumpSpeedValue;
    float timeBetweenStepValue;
    float timeBetweenChargeValueHolder;
    float poisonTime;
    float poisonParticleHitCount;
    float antParticleHitCount;
    [SerializeField] int poisonDamage;
    [SerializeField] float poisonRate;
    [Header("AntParticle")]
    public float antParticleDamage = 3f;
    [SerializeField] GameObject antSwarmObject;
    [SerializeField] GameObject wormling1;
    [SerializeField] GameObject wormling2;
    //RopeBridgeProcess
    [Header("RopeBridge")]
    [SerializeField] RopeBridge ropeBridge;

    //Melee Process
    [Header("Melee")]
    [SerializeField] float attackDamage = 10f;
    public Vector2 attackOffset;
    public float attackRange = 1f;
    public LayerMask attackMask;
    public Transform attackPos;
    [SerializeField] Vector2[] attackPositions;

    [Header("Dash Process")]
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    public bool hasTouchedArrow = false;
    public bool canDash = true;
    private float dashTimer = 0;
    public float dashLimit = 1f;

    
    
    void Start()
    {
        if(Debug.isDebugBuild)
        {
            Grapple.canGrapple = true;
        }
        Time.timeScale = 1f;
        Cursor.visible = true;
        grapple = GetComponentInChildren<Grapple>();
        rigidBody = GetComponent<Rigidbody2D>();
        gravityDefaultValue = rigidBody.gravityScale;
        jumpSpeedValue = jumpSpeed;
        runSpeedValue = runSpeed;
        timeBetweenStepValue = timeBetweenStep;
        //Dash Process
        dashTime = startDashTime;
        PlayerPrefsProcess();
        if (Web_Projectile.canWeb)
        {
            if (wormling1 != null)
            {
                Destroy(wormling1);
                Destroy(wormling2);
            }
        }
    }

    void PlayerPrefsProcess()
    {
        transform.position = PlayerPrefsController.Instance.LastCheckPoint();
    }

    void Update()
    {
        if (isAlive)
        {
            getIfGrounded();
            InteractionWithBoss();
            Run();
            DashProcess();
            Attack();
            Jump();
            FlipSprite();
            WallJump();
            ManageJumpingAndFallingAnim();
            ChargeForce();
            if (grapple.GetIsGrapple())
            {
                if (grapple.GetTarget() != null)
                {
                    float distanceBetweenObjectAndPlayer = Vector3.Distance(grapple.GetTargetPos(), transform.position);
                    //Debug.Log(distanceBetweenObjectAndPlayer); //Gives a float value between 3 and 12
                    if (distanceBetweenObjectAndPlayer > 0 && distanceBetweenObjectAndPlayer < 3)
                    {
                        ropeBridge.ropeSegLen = 0.02f;
                    }
                    else if (distanceBetweenObjectAndPlayer > 3 && distanceBetweenObjectAndPlayer < 6)
                    {
                        ropeBridge.ropeSegLen = 0.1f;
                    }
                    else if (distanceBetweenObjectAndPlayer > 6 && distanceBetweenObjectAndPlayer < 10)
                    {
                        ropeBridge.ropeSegLen = 0.2f;
                    }
                    else
                    {
                        ropeBridge.ropeSegLen = 0.25f;
                    }

                    GameObject targetInstance = grapple.GetTarget();
                    if (distanceBetweenObjectAndPlayer >= 2f)
                    {
                        Vector3 direction = targetInstance.transform.position - transform.position;
                        direction.x = Mathf.Clamp(direction.x, -maxGrappleForce, maxGrappleForce);
                        direction.y = Mathf.Clamp(direction.y, -maxGrappleForce, maxGrappleForce);
                        rigidBody.AddForce(direction * grappleForceMultiplier * Time.deltaTime);

                        rigidBody.gravityScale = 0;
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle, (transform.position +
                            grapple.target.transform.position) / 2, Quaternion.identity);
                            grapple.DeActiveRope();
                            grapple.target = null;

                            if (DestroyWebs!=null)
                            {
                                DestroyWebs();
                            }
                            if(DestroyBoxesInPlayerController!=null)
                            {
                                DestroyBoxesInPlayerController();
                            }
                            Destroy(particle, 1);
                            grapple.target = null;
                            grapple.springJoint.enabled = false;
                            rigidBody.gravityScale = gravityDefaultValue;
                        }
                    }
                    else
                    {
                        rigidBody.gravityScale = gravityDefaultValue;
                        GetComponentInChildren<Grapple>().DisableSprintJoint();
                    }
                }
            }

            if (grapple.GetIsPulling())
            {
                if (grapple.GetTarget() != null)
                {
                    //RopeBridge Segment Process 
                    
                    float distanceBetweenObjectAndPlayer = Vector3.Distance(grapple.GetTargetPos(), transform.position);
                    GameObject targetInstance = grapple.GetTarget();
                    if (distanceBetweenObjectAndPlayer >= 2f)
                    {
                        Vector3 direction = targetInstance.transform.position - transform.position;
                        targetInstance.GetComponent<Rigidbody2D>().AddForce(-direction * pullingForceMultiplier * Time.deltaTime);
                        if (distanceBetweenObjectAndPlayer > grappleRadious)
                        {
                            GameObject particle = Instantiate(webSnapParticle, (transform.position + grapple.target.transform.position) / 2,
                            Quaternion.identity);
                            grapple.DeActiveRope();
                            grapple.target = null;
                            //DestroyBoxes(); // change this with event
                            if (DestroyBoxesInPlayerController!=null)
                            {
                                DestroyBoxesInPlayerController();
                            }
                            Destroy(particle, 1);
                            grapple.springJoint.enabled = false;
                        }
                    }
                    else
                    {
                        GetComponentInChildren<Grapple>().DisableSprintJoint();
                    }
                }
            }
        }

    }


    private void DashProcess()
    {
        if (hasTouchedArrow && dashTimer < dashLimit)
        {
            canDash = false;
            dashTimer += Time.deltaTime;
        }
        else
        {
            canDash = true;
        }
        if (direction == 0)
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow) &&canDash)
            {
                hasTouchedArrow = true;
                dashTimer = 0;
                direction = 1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow)&&canDash)
            {
                hasTouchedArrow = true;
                dashTimer = 0;
                direction = 2;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow)&&canDash)
            {
                hasTouchedArrow = true;
                dashTimer = 0;
                direction = 3;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)&&canDash)
            {
                hasTouchedArrow = true;
                dashTimer = 0;
                direction = 4;
            }
            
           
            
        }
        else
        {

            if (dashTime < 0)
            {
                direction = 0;
                rigidBody.velocity = Vector3.zero;
                dashTime = startDashTime;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (direction == 1)
                {
                    rigidBody.velocity = Vector2.left * Time.deltaTime * dashSpeed * 100f;
                }
                else if (direction == 2)
                {
                    rigidBody.velocity = Vector2.right * Time.deltaTime * dashSpeed * 100f;
                }
                else if (direction == 3)
                {
                    rigidBody.velocity = Vector2.up * Time.deltaTime * dashSpeed * 100f;
                }
                else if (direction == 4)
                {
                    rigidBody.velocity = Vector2.down * Time.deltaTime * dashSpeed * 100f;
                }
            }
        }

    }
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            attackPos.localPosition = GetAttackPos(direction.x, direction.y);
            animator.SetTrigger("Attack");
        }
    }

    public void AttackEvent()
    {
        Vector3 pos = attackPos.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.transform.GetComponent<IDamagable>() != null)
        {
            GameManager.Instance.ShakeEvent();
            colInfo.GetComponent<IDamagable>().GetDamage(attackDamage, transform);
        }
    }
    private Vector2 GetAttackPos(float xAxis,float yAxis)
    {
        bool isXaxis = Mathf.Abs(xAxis) > Mathf.Abs(yAxis);
       
        if (isXaxis)
        {
            if(GameObject.FindGameObjectWithTag("Player").transform.localScale.x!=-1)
            {
                if (xAxis > 0)
                {
                    return attackPositions[0];
                }
                else
                {
                    return attackPositions[1];
                }
            }
            else
            {
                if (xAxis > 0)
                {
                    return new Vector2(attackPositions[0].x*-1,attackPositions[0].y);
                }
                else
                {

                    return new Vector2(attackPositions[1].x * -1, attackPositions[0].y); 
                }
               
            }
           
        }
        else
        {
            if (yAxis > 0)
            {
                return attackPositions[2];
            }
            else
            {
                return attackPositions[3];
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Vector3 pos = attackPos.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    //private void FixedUpdate()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "WaterDroplet")
        {
            col.GetComponent<Animator>().SetTrigger("getTaken");
            StartCoroutine(EffectPlayerFor(1, waterColor, 2, 2));
            AudioManager.Instance.Play("WaterPop");
            Destroy(col.gameObject, 2f);
            RestoreParticleUI.GetComponent<ParticleSystem>().Play();
            UpdateHealth(-10);
            canvas.GetComponent<Animator>().SetTrigger("playerUIRestoreHealth");
            if (playerHealth > 100)
            {
                playerHealth = 100;
                HealthBar.value = playerHealth;
            }
            col.GetComponent<Collider2D>().enabled = false;
        }
        if(col.tag== "AntSwarmStart")
        {
            Debug.Log("swarm");
            antSwarmObject.SetActive(true);
        }
        if (col.tag == "TreeFall")
        {
            isAlive = false;
            DeadMenu.SetActive(true);
            
        }
        if (col.tag == "Grass")
        {
            if (rigidBody.velocity.x > 0.01f)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("HitToRight");
            }
            else if  (rigidBody.velocity.x <-0.01f)
            {
                col.gameObject.GetComponent<Animator>().SetTrigger("HitToLeft");
            }


        }
        if (col.tag == "WebObtain")
        {
            Web_Projectile.canWeb = true;
            Destroy(col.gameObject);
            canvas.GetComponent<Animator>().SetTrigger("AchievementTextAnimation");
            achievementText.text = "Web Shoot Skill Obtained";
            tutorialText.text = "Press Mouse L to use";
            AudioManager.Instance.Play("SkillObtain");
        }
        if (col.tag == "GrappleObtain")
        {
            Grapple.canGrapple = true;
            Destroy(col.gameObject);
            canvas.GetComponent<Animator>().SetTrigger("AchievementTextAnimation");
            achievementText.text = "Grapple Skill Obtained";
            tutorialText.text = "Press Mouse R to use";
            AudioManager.Instance.Play("SkillObtain");
        }
        if (col.tag == "GlideObtain")
        {
            Glide.canGlide = true;
            Destroy(col.gameObject);
            canvas.GetComponent<Animator>().SetTrigger("AchievementTextAnimation");
            achievementText.text = "Glide Skill Obtained";
            tutorialText.text = "Hold Space to use";
            AudioManager.Instance.Play("SkillObtain");
        }


    }

    public void GetHealed()
    {
        StartCoroutine(EffectPlayerFor(1, Color.green, 2, 2));
        RestoreParticleUI.GetComponent<ParticleSystem>().Play();
        UpdateHealth(-100);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "WaspBossBullet")
        {
            UpdateHealth(10);
            StartCoroutine(EffectPlayerFor(2f, poisonColor, -3, -5));
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SpiderBomb")
        {
            collision.gameObject.GetComponent<Explode>().ExplodeBomb();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "SpiderSmoke")
        {

            poisonParticleHitCount++;
            if (poisonParticleHitCount == 2)
            {
                UpdateHealth(0.25f);
                poisonDamageParticleUI.GetComponent<ParticleSystem>().Play();
                poisonParticleHitCount = 0;
            }
        }

        if(other.gameObject.tag=="AntParticle")
        {
            antParticleHitCount++;
            if(antParticleHitCount==4)
            {
                UpdateHealth(antParticleDamage);
                poisonDamageParticleUI.GetComponent<ParticleSystem>().Play();
                antParticleHitCount = 0;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.tag == "SpiderSmoke")
        //{
        //    Debug.Log("SA");
        //    poisonTime += Time.deltaTime;
        //    if (poisonTime >= poisonRate)
        //    {
        //        poisonTime = 0;
        //        UpdateHealth(poisonDamage);
        //    }
        //}
        
    }
    private  void InteractionWithBoss()
    {
        if (boss == null) return;
        Vector3 direction = boss.BossPos() - transform.position;
        if(BossGrappleBullet.bossHoldingPlayer)
        {
            rigidBody.AddForce(direction*bossGrappleForceMultiplier*Time.deltaTime);
        }
    }


    private void WallJump()
    {     
        mx = Input.GetAxis("Horizontal");
        if (mx < 0)
            {
                isFacingRight = false;
            }
            else
            {
                isFacingRight = true;
            }
            if (isFacingRight)
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundMask);

            }
            else
            {
                WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundMask);

            }

            if (WallCheckHit && !isGrounded && PlayerHasVelocity())
            {
                isWallSliding = true;
                jumpTime = Time.time + wallJumpTime;
            }
            else if (jumpTime < Time.time)
            {
                isWallSliding = false;
            }

            if (isWallSliding)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Clamp(rigidBody.velocity.y, wallSlideSpeed, float.MaxValue));
            }
        
       
    }

    private void Run()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(horizontal * runSpeed, rigidBody.velocity.y);
        rigidBody.velocity = playerVelocity;
        animator.SetBool("isRunning", PlayerHasVelocity());
    }
    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) ||isWallSliding &&Input.GetKeyDown(KeyCode.Space))
        {
            
            if (!isGrounded && !isWallSliding) 
            { 
                return; 
            }
            else
            {
                isJumping = true;
                Vector2 jumpForce = new Vector2(0, jumpSpeed);
                AudioManager.Instance.Play("PlayerJump");
                GameObject jumpParticleObject = Instantiate(jumpParticle, groundCheck.position, Quaternion.identity);
                jumpParticleObject.transform.parent = gameObject.transform;
                Destroy(jumpParticleObject, 2f);
                rigidBody.velocity += jumpForce;
            }
        }
    }
    public void ChargeForce()//animation event de çağırlıyor
    {
        if (timeBetweenChargeValueHolder > 0&& !canCharge)
        {

            timeBetweenChargeValueHolder -= Time.deltaTime;
        }
        else
        {
            canCharge = true;
            timeBetweenChargeValueHolder = timeBetweenCharge;
        }

        if (Input.GetMouseButtonDown(2)&& canCharge)
        {
  
            Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;
            //Vector3 dashPos;
            //dashPos = (Vector2)transform.position + direction * chargeSpeed;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, chargeSpeed, groundMask);
            //if (raycastHit2D.collider != null)
            //{
            //    dashPos = raycastHit2D.point;
            //}
            //rigidBody.MovePosition(dashPos);

            //direction.x *= 5;
            //rigidBody.AddForce(direction * 1000);
            //rigidBody.velocity = Vector2.zero;
            //rigidBody.velocity += direction.normalized* chargeSpeed;

            rigidBody.AddForce(direction *100,ForceMode2D.Force);
            canCharge = false;
        }
    }
    private void getIfGrounded() 
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);//ağla ugur
        if (isJumping&& isGrounded)
        {
            AudioManager.Instance.Play("PlayerLand");
        }
        animator.SetBool("isJumping", false);
        isJumping = false;



    }

    void ManageJumpingAndFallingAnim()
    {
        if (rigidBody.velocity.y > 0&& !isGrounded)
        {
            animator.SetBool("isJumping", true);
        }

        else
        {
            animator.SetBool("isJumping", false);
        }

        if (rigidBody.velocity.y < -.1f&&!isGrounded)
        {
            animator.SetBool("isFalling", true);
        }

        else
        {
            animator.SetBool("isFalling", false);
        }
    }
    public void UpdateHealth(float damage)
    {
        if (damage > 1)
        {
            damageParticleUI.GetComponent<ParticleSystem>().Play();
        }
        
        playerHealth -= damage;
        HealthBar.value = playerHealth;
        HealthBarImage.fillAmount = HealthBar.value / 100;
        if (HealthBar.value <= 0 && isAlive)
        {
            isAlive = false;
            animator.SetTrigger("Die");
            DeadMenu.SetActive(true);
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else if (isAlive && damage > 0)
        {
            animator.SetTrigger("isHurt");
            canvas.GetComponent<Animator>().SetTrigger("playerUIGetDamage");
        }
        if (playerHealth>=100)
        {
            playerHealth = 100;
            HealthBar.value = playerHealth;
            HealthBarImage.fillAmount = HealthBar.value / 100;
        }
    }

    public void GetPoisoned()
    {
        StartCoroutine(EffectPlayerFor(2f,poisonColor,-3,-5));
    }

    IEnumerator EffectPlayerFor(float time,Color color,float runSpeedChanger,float jumpSpeedChanger)
    {
        runSpeed = runSpeedValue +runSpeedChanger; ;
        jumpSpeed =jumpSpeedValue+jumpSpeedChanger;
        GetComponentInChildren<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(time);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        runSpeed = runSpeedValue;
        jumpSpeed = jumpSpeedValue;

    }
    private void FlipSprite()
    {
        if (PlayerHasVelocity())
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x)*Mathf.Sign(rigidBody.velocity.x), transform.localScale.y); // return 1 if velocity.x greater than 0 , return -1 if velocity is less than 0; //change back to 1
            if (isGrounded)
            {
                runParticle.SetActive(true);
                if (timeBetweenStep <= 0)
                {
                    AudioManager.Instance.Play(stepSounds[UnityEngine.Random.Range(0, 3)]);
                    timeBetweenStep = timeBetweenStepValue;
                }
                else
                {
                    timeBetweenStep -= Time.deltaTime;
                    
                }
            }
            else
            {
                runParticle.SetActive(false);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            runParticle.SetActive(false);
        }
    }
    private bool PlayerHasVelocity()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon)
        {
            return true;
        }
        return false;
    }
   
    
}
