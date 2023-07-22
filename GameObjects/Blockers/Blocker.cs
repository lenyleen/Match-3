using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Animator))]
public abstract class Blocker : MonoBehaviour, ICellGridMember, ICanBeCollected
{
        public string collectibleId => GetComponent<SpriteRenderer>().sprite.name;
        protected Animator Anim => this.GetComponent<Animator>();
        protected BoxCollider2D _boxCollider2D => this.GetComponent<BoxCollider2D>();
        private static readonly int Kill = Animator.StringToHash("Kill");
        [SerializeField] private GameObject particlesPrefab;
        protected Cell cell;
        protected Particles Particles {get; private set;}
        public CollectiblesManager CollectiblesManager { get; set; }

        public virtual void Initialize(Cell cell)
        {
                GetParticles();
                this.cell = cell;
        }
        public abstract Task DoCellGridMemberAction();
        public virtual async void ParticlesPlay()
        {
                EventBus.RaiseEvent<IScoreItemObserver>(listener => listener.ItemCollected(collectibleId));
                var bezierAnchor = CollectiblesManager.CheckItemForCollectibility(this.gameObject);
                if (bezierAnchor != null)
                {
                        var b = bezierAnchor.PrepareToCollect(this.transform);
                        await Collect(b);
                        return;
                }
                Anim.SetTrigger(Kill);
                Particles.Play();
        }

        public virtual void GetParticles()
        {
                Particles = Instantiate(particlesPrefab, this.transform.position, quaternion.identity, this.transform)
                        .GetComponent<Particles>();
                Particles.particlesAction += Disable;
        }

        protected abstract void Disable();
        public async Task Collect(Path path)
        {
                float t = 0;
                while (t < 1)
                {
                        this.GetComponent<SpriteRenderer>().sortingOrder = 10;
                        this.transform.position = Bezier.GetPoint(this.transform.position,path.Points[1],path.Points[2],path.Points[3],t);
                        t += 2.5f * Time.deltaTime;
                        await Task.Delay(1);
                }
                Disable();
        }

        public void InitializeAsCollectible(CollectiblesManager collectiblesManager)
        {
                CollectiblesManager = collectiblesManager;
        }
}


