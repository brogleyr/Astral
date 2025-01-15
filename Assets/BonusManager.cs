using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonusManager : MonoBehaviour
{

    public static BonusManager Instance { get; private set; }


    [field: SerializeField]
    public List<Bonus> activeBonuses { get; set; } = new List<Bonus>() {
        new Bonus(Bonus.BonusOperation.ADD, 3, new List<Star.StarColor>() { Star.StarColor.RED, Star.StarColor.WHITE }),
        new Bonus(Bonus.BonusOperation.MULT, 5, new List<Star.StarColor>() { Star.StarColor.RED, Star.StarColor.BLUE }),
        new Bonus(Bonus.BonusOperation.MULT, 2, new List<Star.StarColor>() { Star.StarColor.WILD, Star.StarColor.BLUE, Star.StarColor.RED })
    };

    void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else {
            Instance = this;
        }
    }

    public List<int> CalculateBonuses(Graph graph) {
        // Search the graph for instances of each active Bonus
        foreach (Bonus bonus in activeBonuses) {
            List<List<Line>> chains = graph.FindSequence(bonus.PropertyChain);
            if (chains == null || chains.Count == 0) continue;
            
            foreach (List<Line> chain in chains) {
                bonus.ApplyBonus(chain);
            }

        }

        return null;
    }

    [System.Serializable]
    public class Bonus {
        public enum BonusOperation {
            ADD,MULT
        };

        [SerializeField]
        public BonusOperation Operation { get; private set; }
        [SerializeField]
        public int Amount { get; private set; }

        [SerializeField]
        public List<Star.StarColor> PropertyChain { get; private set; }

        public Bonus(BonusOperation operation, int amount, List<Star.StarColor> propertyChain) {
            Operation = operation;
            Amount = amount;
            PropertyChain = propertyChain;
        }

        public void ApplyBonus(List<Line> lines) {
            for (int i = 0; i < lines.Count; i++) {
                if (Operation == BonusOperation.ADD) lines[i].Score += Amount;
                else if (Operation == BonusOperation.MULT) lines[i].Score *= Amount;
                
            }
        }
    }
}
