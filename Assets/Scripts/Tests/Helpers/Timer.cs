using System;

public class Timer
{
    private float maxTimeSeconds;
    private float currentTimeSeconds;
    private bool isRun;

    public event Action<object, EventArgs> OnTimerStartEvent;
    public event Action<object, EventArgs> OnTimerTickEvent;
    public event Action<object, EventArgs> OnTimerStopEvent;
    public event Action<object, EventArgs> OnTimeoutEvent;

    public Timer(float _maxTimeSeconds)
    {
        maxTimeSeconds = _maxTimeSeconds;
        currentTimeSeconds = maxTimeSeconds;
        isRun = false;
    }

    public void StartTimer(object _context = null)
    {
        isRun = true;
        OnTimerStartEvent?.Invoke(_context, new EventArgs());
    }

    public void StopTimer(object _context = null)
    {
        currentTimeSeconds = maxTimeSeconds;
        isRun = false;
        OnTimerStopEvent?.Invoke(_context, new EventArgs());
    }

    public void TimerTick(float _deltaTime, object _context = null)
    {
        if (isRun)
        {
            OnTimerTickEvent?.Invoke(_context, new EventArgs());
            if (currentTimeSeconds > 0)
                currentTimeSeconds -= _deltaTime;
            else
            {
                OnTimeoutEvent?.Invoke(_context, new EventArgs());
            }
        }
    }

    public override string ToString()
    {
        int minutes = (int)(currentTimeSeconds / 60);
        int seconds = (int)(currentTimeSeconds % 60);

        string stringSeconds = "";
        if (seconds < 10)
            stringSeconds = $"0{seconds}";
        else
            stringSeconds = $"{seconds}";

        return $"{minutes}:{stringSeconds}"; 
    }
}