namespace Colony.Engine.Simulation;

public class GameTime
{
    private readonly SimulationSettings _settings;

    public long TotalTicks { get; private set; }

    public int Minute { get; private set; }
    public int Hour { get; private set; }
    public int Day { get; private set; }

    public GameTime(SimulationSettings settings)
    {
        _settings = settings;
    }

    public void Tick()
    {
        TotalTicks++;

        UpdateTime();
    }

    private void UpdateTime()
    {
        var totalMinutes =
            TotalTicks / _settings.TicksPerGameMinute;

        Minute = (int)(
            totalMinutes %
            _settings.MinutesPerGameHour);

        Hour = (int)(
            totalMinutes /
            _settings.MinutesPerGameHour %
            _settings.HoursPerGameDay);

        Day = (int)(
            totalMinutes /
            (_settings.MinutesPerGameHour *
             _settings.HoursPerGameDay));
    }
}