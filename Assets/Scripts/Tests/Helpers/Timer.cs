using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Timer
{
    private float maxTimeSeconds;
    private float currentTimeSeconds;
    private bool isRun;

    public event Action<object, EventArgs> OnTimerStart;
    public event Action<object, EventArgs> OnTimerTick;
    public event Action<object, EventArgs> OnTimerStop;
    public event Action<object, EventArgs> OnTimeout;

    public Timer(float _maxTimeSeconds)
    {
        maxTimeSeconds = _maxTimeSeconds;
        currentTimeSeconds = maxTimeSeconds;
        isRun = false;
    }

    public void StartTimer(object _context = null)
    {
        isRun = true;
        OnTimerStart?.Invoke(_context, new EventArgs());
    }

    public void StopTimer(object _context = null)
    {
        currentTimeSeconds = maxTimeSeconds;
        isRun = false;
        OnTimerStop?.Invoke(_context, new EventArgs());
    }

    public void TimerTick(float _deltaTime, object _context = null)
    {
        if (isRun)
        {
            OnTimerTick?.Invoke(_context, new EventArgs());
            if (currentTimeSeconds > 0)
                currentTimeSeconds -= _deltaTime;
            else
            {
                OnTimeout?.Invoke(_context, new EventArgs());
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