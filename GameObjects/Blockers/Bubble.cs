using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bubble : Blocker, ILvlCompletedObserver, ICellBlockerBlocker
{
        private static readonly int Grow = Animator.StringToHash("Grow");
        public event Action OnDestroyAction;
        [SerializeField] private LayerMask blockersLayer;
        [SerializeField] private LayerMask defaultLayer;
        [SerializeField] private float minTimeToSpawnBubble = 10;
        [SerializeField] private float maxTimeTOSpawnBubble = 20;
        private float timeToSpawnBubble;
        private bool activeToSpawn;
        ContactFilter2D contactFilter => new ContactFilter2D();

        private void Activate()
        {
                if(!activeToSpawn) return;
                activeToSpawn = false;
                StartCoroutine(SpawnBubble());
        }

        public override void Initialize(Cell cell)
        {
                timeToSpawnBubble = Random.Range(minTimeToSpawnBubble, maxTimeTOSpawnBubble);
                EventBus.Subscribe(this);
                activeToSpawn = true;
                base.Initialize(cell);
                Anim.SetTrigger(Grow);
                cell.SetBlocked(this);
        }

        public void OnGoalReached()
        {
                StopCoroutine(SpawnBubble());
                activeToSpawn = false;
        }
        private IEnumerator SpawnBubble()
        {
                activeToSpawn = false;
                yield return new WaitForSeconds(timeToSpawnBubble);
                var hits = DetectNearestObjects(defaultLayer);
                Cell hitCell = null;
                foreach (var hit in hits)
                {
                        var candy = hit.transform.GetComponent<Candy>();
                        if (candy == null || candy.cell.Blocked) continue;
                        hitCell = candy.cell;
                        break;
                }
                if (hitCell == null) yield break;
                var newBubble = hitCell.SpawnBlocker(this);
                newBubble.GoToYourCell();
                activeToSpawn = true;
        }
        private async void GoToYourCell()
        {
                var t = 0f;
                while (t < 1)
                {
                        t +=  Constants.StandardCircleCastRadius * 0.02f;
                        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, t);
                        await Task.Delay(1);
                }
        }
        public override Task DoCellGridMemberAction()
        {
                SoundManager.instance.PlaySound("Syrup");
                ParticlesPlay();
                HitNearBeforeDie();
                return Task.CompletedTask;
        }

        private void HitNearBeforeDie()
        {
                _boxCollider2D.enabled = false;
                var hits = DetectNearestObjects(blockersLayer);
                if (hits.Count <= 0) return;
                foreach (var hit2D in hits.Where(hit2D => hit2D.transform.GetComponent<Bubble>() != null))
                {
                        hit2D.transform.GetComponent<Bubble>().Activate();
                }
        }

        private List<RaycastHit2D> DetectNearestObjects(LayerMask layerMask)
        {
                var hits = new List<RaycastHit2D>();
                contactFilter.SetLayerMask(layerMask);
                Physics2D.CircleCast(this.transform.position, Constants.StandardCircleCastRadius, Vector2.zero,
                        contactFilter, hits);
                return hits;
        }

        protected override void Disable()
        {
                StopAllCoroutines();
                EventBus.Unsubscribe(this);
                Particles.particlesAction -= Disable;
                Destroy(this.gameObject);
        }

        private void OnDestroy()
        {
                OnDestroyAction?.Invoke();
        }
}


        
