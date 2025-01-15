using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationQueue : MonoBehaviour
{
    
    private Queue<List<QueueEvent>> queue = new Queue<List<QueueEvent>>();
    private bool queueRunning = false;
    private List<QueueEvent> activeSet;
    private float timer = 0.0f;
    private float timerDuration = float.MaxValue;

    private void Update() {
        if (!queueRunning) return;

        if (timer > timerDuration) {
            bool nextPlayed = PlayNextInQueue();
            timer = 0.0f;
            if (!nextPlayed) {
                GetComponent<GameManager>().CountFinished();
            }
        }
        timer += Time.deltaTime;
    }

    public void AnimateScore() {
        queueRunning = true;
        PlayNextInQueue();
    }

    private bool PlayNextInQueue() {
        if (queue.Count > 0) {
            activeSet = queue.Dequeue();
        } else {
            activeSet = null;
            queueRunning = false;
        }

        if (activeSet != null) {
            float maxDuration = 0.0f;
            foreach (QueueEvent queueEvent in activeSet) {
                maxDuration = Mathf.Max(maxDuration, queueEvent.duration);
                StartCoroutine(queueEvent.function());
            }
            timerDuration = maxDuration;
            return true;
        }
        return false;
    }

    public void EnqueueTally(Graph graph, ScoreTicker roundScoreTicker, int finalRoundScore, ScoreTicker totalScoreTicker, int finalTotalScore) {
        
        EnqueuePause(0.5f);

        // TODO: Fix score movement in different aspect ratios

        EnqueueLineScoreMove(graph, new Vector3(0f, 3.5f, 0f), 0.5f);
        // EnqueueHideLineScores

        EnqueueTick(roundScoreTicker, finalRoundScore, 0.5f);
        EnqueuePause(0.5f);
        (ScoreTicker, int, float) roundTickDown = (roundScoreTicker, 0, 0.5f);
        (ScoreTicker, int, float) totalTickUp = (totalScoreTicker, finalTotalScore, 0.5f);
        EnqueueTicks(new [] {roundTickDown, totalTickUp});
        EnqueueClearTicker(roundScoreTicker);
        EnqueuePause(1.0f);
    }

    public void EnqueueTicks((ScoreTicker, int, float)[] tickEvents) {
        List<QueueEvent> events = new List<QueueEvent>();
        for (int i = 0; i < tickEvents.Length; i++) {
            (ScoreTicker ticker, int total, float duration) = tickEvents[i];
            events.Add(new TickerEvent(ticker, total, duration));
        }
        queue.Enqueue(events);
    }

    public void EnqueueTick(ScoreTicker ticker, int newValue, float duration) {
        EnqueueTicks(new[] { (ticker, newValue, duration)});
    }

    public void EnqueueClearTicker(ScoreTicker ticker) {
        List<QueueEvent> events = new List<QueueEvent> {new TickerEvent(ticker)};
        queue.Enqueue(events);
    }

    public void EnqueuePause(float duration) {
        List<QueueEvent> events = new List<QueueEvent> {new QueueEvent(duration)};
        queue.Enqueue(events);
    }

    public void EnqueueLineScoreMove(Graph graph, Vector3 position, float duration) {
        List<QueueEvent> tickerMoves = new List<QueueEvent>();
        List<QueueEvent> tickerHides = new List<QueueEvent>();
        foreach (LineTicker ticker in graph.GetLineTickers()) {
            if (ticker != null) {
                tickerMoves.Add(new TickerMove(ticker, position, duration));
                tickerHides.Add(new TickerEvent(ticker));
            }
        }
        queue.Enqueue(tickerMoves);
        queue.Enqueue(tickerHides);
    }

    internal void EnqueueShowLineScores(Graph graph) {
        List<QueueEvent> tickerEvents = new List<QueueEvent>();
        foreach (ScoreTicker ticker in graph.GetLineTickers()) {
            if (ticker != null && !ticker.Revealed) {
                tickerEvents.Add(new TickerEvent(ticker));
            }
        }
        queue.Enqueue(tickerEvents);
    }
}

internal class QueueEvent {

    public delegate IEnumerator EventFunction();
    public EventFunction function;
    public float duration;

    internal QueueEvent(float duration) {
        this.duration = duration;
        this.function = Wait;
    }

    public IEnumerator Wait() {
        yield return new WaitForSeconds(duration);
    }

}

internal class TickerEvent : QueueEvent {

    public ScoreTicker ticker;
    int value;
    Vector3 newPosition;

    internal TickerEvent(ScoreTicker ticker, int newValue, float duration): base(duration) {
        this.function = Tick;
        this.ticker = ticker;
        this.value = newValue;
        this.duration = duration;
    }

    internal TickerEvent(ScoreTicker ticker): base(0.0f) {
        this.ticker = ticker;
        this.function = ToggleVisible;
    }

    public IEnumerator Tick() {
        return ticker.TickTo(value, duration);
    }

    public IEnumerator ToggleVisible() {
        return ticker.ToggleVisible();
    }
}

internal class TickerMove : QueueEvent {
    LineTicker ticker;
    Vector3 destination;
    
    internal TickerMove(LineTicker ticker, Vector3 newPosition, float duration): base(duration) {
        this.ticker = ticker;
        this.function = Move;
        this.destination = newPosition;
    }

    public IEnumerator Move() {
        return ticker.MoveToPoint(destination, duration);
    }
}

internal class LineEvent : QueueEvent {

    public Line line;

    internal LineEvent(Line targetLine) : base(0.0f) {
        function = Animate;
        duration = 0.5f;
        line = targetLine;
    }

    public IEnumerator Animate() {
        yield return new WaitForSeconds(2);
        Debug.Log("Line event finished!");
    }
}