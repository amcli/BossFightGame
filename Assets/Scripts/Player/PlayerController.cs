using UnityEngine;
using UnityEngine.InputSystem;
using Game.Data;
using Game.Combat;

namespace Game.Player
{
    // Top-down melee player. WASD/arrows to move, mouse to aim, left-click to swing a box hitbox
    // in the facing direction. Stats come from a CharacterClassDefinition; damage from a WeaponDefinition.
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] float baseMoveSpeed = 4f; // units/sec at MoveSpeed stat 0 (+1 stat = +1%)

        [Header("Melee swing")]
        [SerializeField] float swingRange = 0.9f;                       // hitbox center offset from player
        [SerializeField] Vector2 swingBoxSize = new Vector2(1.3f, 1.3f);

        [Header("Swing visual")]
        [SerializeField, Min(0f)] float swingVisualDuration = 0.12f;
        [SerializeField] Color swingVisualColor = new Color(1f, 0.9f, 0.3f, 0.35f);

        Rigidbody2D body;
        Health health;
        SpriteRenderer swingVisual;
        float swingVisualRemaining;
        static Sprite sharedBoxSprite;

        // injected from data
        int attack, intelligence, moveSpeedStat;
        int weaponDamage = 1;
        DamageType weaponDamageType = DamageType.Physical;
        float attackCooldown = 0.5f;
        float knockback;

        float cooldownRemaining;
        Vector2 facing = Vector2.down;

        public Health Health => health;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            CreateSwingVisual();
        }

        public void Configure(CharacterClassDefinition cls, WeaponDefinition weapon)
        {
            var s = cls.BaseStats;
            attack = s.Attack;
            intelligence = s.Intelligence;
            moveSpeedStat = s.MoveSpeed;
            health.Configure(Faction.Player, s.MaxHP, s.Defense, s.Resistance);

            weaponDamage = weapon.Damage;
            weaponDamageType = weapon.DamageType;
            attackCooldown = weapon.FireRate > 0.01f ? 1f / weapon.FireRate : 0.5f;
            knockback = weapon.KnockbackForce;
        }

        void Update()
        {
            TickSwingVisual();

            if (health.IsDead)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            ReadMoveAndAim();

            if (cooldownRemaining > 0f) cooldownRemaining -= Time.deltaTime;

            var mouse = Mouse.current;
            if (mouse != null && mouse.leftButton.wasPressedThisFrame && cooldownRemaining <= 0f)
                Swing();
        }

        void ReadMoveAndAim()
        {
            Vector2 move = Vector2.zero;
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.wKey.isPressed || kb.upArrowKey.isPressed) move.y += 1f;
                if (kb.sKey.isPressed || kb.downArrowKey.isPressed) move.y -= 1f;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) move.x += 1f;
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) move.x -= 1f;
            }
            if (move.sqrMagnitude > 1f) move.Normalize();

            float speed = baseMoveSpeed * (1f + moveSpeedStat / 100f);
            body.linearVelocity = move * speed;

            var mouse = Mouse.current;
            var cam = Camera.main;
            if (mouse != null && cam != null)
            {
                Vector3 world = cam.ScreenToWorldPoint(mouse.position.ReadValue());
                Vector2 aim = (Vector2)world - (Vector2)transform.position;
                if (aim.sqrMagnitude > 0.01f) facing = aim.normalized;
            }
            else if (move.sqrMagnitude > 0.01f)
            {
                facing = move;
            }
        }

        void Swing()
        {
            cooldownRemaining = attackCooldown;
            Vector2 center = (Vector2)transform.position + facing * swingRange;
            float angle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;
            int outgoing = DamageMath.ScaleOutgoing(weaponDamage, weaponDamageType, attack, intelligence);
            var info = new DamageInfo(outgoing, weaponDamageType, facing, knockback, Faction.Player);
            Hitbox.OverlapDamage(center, swingBoxSize, angle, info);
            ShowSwingVisual(angle);
        }

        // -------- swing visual: a translucent box flashed at the hitbox, matching its size + angle --------

        void CreateSwingVisual()
        {
            var go = new GameObject("SwingVisual");
            go.transform.SetParent(transform, false);
            swingVisual = go.AddComponent<SpriteRenderer>();
            swingVisual.sprite = GetBoxSprite();
            swingVisual.color = swingVisualColor;
            swingVisual.sortingOrder = 11; // above the knight (10)
            go.SetActive(false);
        }

        void ShowSwingVisual(float angleDeg)
        {
            if (swingVisual == null) return;
            var t = swingVisual.transform;
            t.localPosition = facing * swingRange;
            t.localRotation = Quaternion.Euler(0f, 0f, angleDeg);
            t.localScale = new Vector3(swingBoxSize.x, swingBoxSize.y, 1f);
            swingVisual.gameObject.SetActive(true);
            swingVisualRemaining = swingVisualDuration;
        }

        void TickSwingVisual()
        {
            if (swingVisualRemaining <= 0f) return;
            swingVisualRemaining -= Time.deltaTime;
            if (swingVisualRemaining <= 0f && swingVisual != null)
                swingVisual.gameObject.SetActive(false);
        }

        // 1x1 white sprite at 1 px-per-unit, so a child localScale equals world size in units.
        static Sprite GetBoxSprite()
        {
            if (sharedBoxSprite == null)
            {
                var tex = new Texture2D(1, 1) { filterMode = FilterMode.Point };
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                sharedBoxSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
            }
            return sharedBoxSprite;
        }
    }
}
