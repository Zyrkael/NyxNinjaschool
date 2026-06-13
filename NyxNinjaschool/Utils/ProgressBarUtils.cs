namespace NyxNinjaschool.Utils;

public class ProgressBarUtils : IDisposable
{
    private enum ProgressStatus
    {
        Running = 0,
        Success = 1,
        Error = -1
    }

    private readonly string _animationFrames = "\\-/";
    private readonly int _barWidth = 40;
    private double _currentValue;
    private readonly double _maxValue;

    private int _currentAnimationFrameIndex;
    private readonly System.Timers.Timer _timer;
    private ProgressStatus _status;
    private readonly string _taskName;
    private string? _extraMessage;
    private int _lastRenderedTextLength;
    private readonly object _lockObj = new object();

    public ProgressBarUtils(string taskName, double maxValue)
    {
        _taskName = taskName;
        _maxValue = maxValue;
        
        // Khởi tạo timer với chu kỳ 100ms tương đương bản Java
        _timer = new System.Timers.Timer(100);
        _timer.Elapsed += (_, _) =>
        {
            _currentAnimationFrameIndex++;
            Render();
        };
        _timer.Start();
    }

    public void SetExtraMessage(string? extraMessage)
    {
        _extraMessage = extraMessage?.Replace("\n", " ");
    }

    public void SetValue(double value)
    {
        _currentValue = value;
        Render();
    }

    public void Step()
    {
        _currentValue++;
        Render();
    }

    public void ReportSuccess()
    {
        _status = ProgressStatus.Success;
        Render();
        Stop();
    }

    public void ReportError()
    {
        _status = ProgressStatus.Error;
        Render();
        Stop();
    }

    public void Render()
    {
        // Sử dụng lock để tránh việc Console in bị đè/lẫn lộn do gọi từ luồng Timer và luồng chính
        lock (_lockObj)
        {
            if (_lastRenderedTextLength > 0)
            {
                string spaces = new string(' ', _lastRenderedTextLength) + "\r";
                Console.Write(spaces);
            }

            int currentBlock = (int)(_currentValue / _maxValue * _barWidth);
            char symbol = GetSymbol();
            
            string filledBlock = new string('#', currentBlock);
            string remainBlock = new string('-', Math.Max(0, _barWidth - currentBlock));

            string text = $"{_taskName} [{symbol}] [{filledBlock}{remainBlock}] {_currentValue:0}/{_maxValue:0} {(_extraMessage ?? "")}";
            Console.Write(text);

            Console.Write(_status == ProgressStatus.Running ? "\r" : "\n");

            _lastRenderedTextLength = text.Length;
        }
    }

    private char GetSymbol()
    {
        switch (_status)
        {
            case ProgressStatus.Success:
                return 'V';
            case ProgressStatus.Error:
                return 'X';
            default:
                return _animationFrames[_currentAnimationFrameIndex % _animationFrames.Length];
        }
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}
