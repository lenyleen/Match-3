using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalRuby.LightningBolt;
using GameObjects.Candies;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public sealed class ChocolateCandy : BonusCandy
{
    [SerializeField] private LightningBoltScript _lightningBolt;
    [SerializeField] private LayerMask layerOfSwitchedCandy;
    private List<GameObject> lightningBolts = new List<GameObject>();
    private CandyColor TypeOfSwitchedCandy { get; set; } 
    protected override void OnEnable()
    {
        base.OnEnable();
        TypeOfSwitchedCandy = CandyColor.None;
        if (lightningBolts.Count <= 0) return;
        foreach (var bolt in lightningBolts)
        {
            Destroy(bolt.gameObject);
        }
    }

    public override async Task<bool> BonusAction(Candy candy)
    {
        if (candy.TypeOfBonus == TypeOfCandyBonus.ChocolateCandy)
            return false;
        TypeOfSwitchedCandy = candy.Color;
        if (candy.TypeOfBonus == TypeOfCandyBonus.None)
        {
            await DoCellGridMemberAction();
            return true;
        }
        await SuperBonus(candy);
        return true;
    }
    public override async Task DoCellGridMemberAction()
    {
        StartCoroutine(Shake());
        var foundCandies = FindBoxCastHits();
        await Task.WhenAll(CastLightningBolts(foundCandies));
        var raycastsCandies = foundCandies.Select(s => s.transform.GetComponent<Candy>()).ToList();
        await DestroyLightningBolts(raycastsCandies);
        ParticlesPlay(); 
        ReturnCandyToPool();
        await Task.Delay(400);
    }
    private List<Candy> FindBoxCastHits()
    {
        boxCollider.enabled = false;
        var filter2D = new ContactFilter2D();
        filter2D.SetLayerMask(layerOfSwitchedCandy);
        var hits = new List<RaycastHit2D>();
        Physics2D.BoxCast(this.transform.position, new Vector3(28f,28f, 0f), 0f,Vector2.zero, filter2D, hits);
        return hits.Where(s => s.transform.GetComponent<Candy>().Color == TypeOfSwitchedCandy).Select(hit => hit.transform.GetComponent<Candy>()).ToList();
    }
    public override async Task SuperBonus(Candy switchedCandy)
    {
        var foundCandies = FindBoxCastHits();
        foreach (var candy in foundCandies)
        {
            var newBonusCandy = candy.cell.SpawnBonusCandy(switchedCandy.TypeOfBonus,switchedCandy.Color);
            await Task.Delay(100);
        }
        await DoCellGridMemberAction();
        ReturnCandyToPool();
    }

    private async Task DestroyLightningBolts(List<Candy> foundCandies)
    {
        for (var i = 0; i < foundCandies.Count; i++)
        {
            Destroy(lightningBolts[i].gameObject);
            /*foundCandies[i].GetComponent<BoxCollider2D>().enabled = false;*/
        }
        await Task.WhenAll(foundCandies.Select(candy => candy.DoCellGridMemberAction()));
    }
    private async Task CastLightningBolts(List<Candy> hits)
    {
        SoundManager.instance.PlaySound("LightningSound",true);
        foreach (var candy in hits)
        {
            var newLightning =
                Instantiate(_lightningBolt, this.transform.position, Quaternion.identity, this.transform);
            newLightning.StartObject = this.gameObject;
            newLightning.EndObject = candy.transform.gameObject;
            newLightning.ManualMode = false;
            lightningBolts.Add(newLightning.gameObject);
            await Task.Delay(200);
        }
    }
    private IEnumerator Shake()
    {
        var thisPosition = this.transform.position;
        while (true)
        {
            this.transform.position += GetRandomPosition();
            this.transform.localScale += new Vector3(0.001f, 0.001f, 0);
            yield return 0;
            this.transform.position = thisPosition;
            this.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-15, 15)); 
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-0.10f, 0.10f), Random.Range(-0.10f, 0.10f),0f);
    }
}
