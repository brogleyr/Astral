using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public void EnqueueTick(ScoreTicker ticker, int newValue, float duration) {
        EnqueueTicks(new[] { (ticker, newValue, duration)});
    }

    public void EnqueueTicks((ScoreTicker, int, float)[] tickEvents) {
        List<QueueEvent> events = new List<QueueEvent>();
        for (int i = 0; i < tickEvents.Length; i++) {
            (ScoreTicker ticker, int newValue, float duration) = tickEvents[i];
            events.Add(new TickerEvent(ticker, newValue, duration));
        }
        queue.Enqueue(events);
    }

    public void EnqueueClearTicker(ScoreTicker ticker) {
        List<QueueEvent> events = new List<QueueEvent> {new TickerEvent(ticker)};
        queue.Enqueue(events);
    }

    public void EnqueuePause(float duration) {
        List<QueueEvent> events = new List<QueueEvent> {new QueueEvent(duration)};
        queue.Enqueue(events);
    }



    public void EnqueueMovesToPoint(List<Transform> transforms, Vector3 newLocation, float duration) {

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

    internal TickerEvent(ScoreTicker ticker, int newValue, float duration): base(duration) {
        this.function = Tick;
        this.ticker = ticker;
        this.value = newValue;
        this.duration = duration;
    }

    internal TickerEvent(ScoreTicker ticker): base(0.0f) {
        this.ticker = ticker;
        this.function = Clear;
    }

    public IEnumerator Tick() {
        return ticker.TickTo(value, duration);
    }

    public IEnumerator Clear() {
        return ticker.ClearTicker();
    }
}

// internal class LineEvent : QueueEvent {

//     public Line line;

//     internal LineEvent(Line targetLine) : base() {
//         function = Animate;
//         duration = 0.5f;
//         line = targetLine;
//     }

//     public IEnumerator Animate() {
//         yield return new WaitForSeconds(2);
//         Debug.Log("Line event finished!");
//     }
// }