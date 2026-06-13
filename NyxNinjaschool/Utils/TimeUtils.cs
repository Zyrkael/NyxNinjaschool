using System;
using System.Runtime.CompilerServices;

namespace NyxNinjaschool.Utils;

public static class TimeUtils
{
    /// <summary>
    /// Kiểm tra xem khoảng thời gian từ startTime đến hiện tại đã vượt qua durationMs hay chưa.
    /// Thường dùng để tính toán hồi chiêu (cooldown), khoảng thời gian chờ (delay) hoặc kiểm tra hết hạn (expire).
    /// </summary>
    /// <param name="startTime">Thời điểm bắt đầu (mili-giây).</param>
    /// <param name="durationMs">Khoảng thời gian tối thiểu cần trôi qua (mili-giây).</param>
    /// <returns>True nếu đã trôi qua đủ thời gian, ngược lại False.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasElapsed(long startTime, long durationMs)
    {
        return CurrentTimeMillis() - startTime > durationMs;
    }

    /// <summary>
    /// Lấy thời gian hiện tại tính bằng mili-giây kể từ Unix Epoch.
    /// </summary>
    /// <returns>Thời gian hiện tại bằng mili-giây.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long CurrentTimeMillis()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Kiểm tra xem ngày hiện tại (hoặc ngày được truyền vào) có phải là Tết Trung Thu (15/8 Âm lịch) hay không.
    /// Thường dùng để bật/tắt sự kiện Trung Thu trong game.
    /// </summary>
    /// <param name="date">Ngày cần kiểm tra (mặc định là ngày hiện hành hệ thống).</param>
    /// <returns>True nếu là Tết Trung Thu, ngược lại False.</returns>
    public static bool IsMidAutumnFestival(DateTime? date = null)
    {
        try
        {
            var targetDate = date ?? DateTime.Now;
            var lunarCalendar = new System.Globalization.ChineseLunisolarCalendar();
            
            int year = lunarCalendar.GetYear(targetDate);
            int month = lunarCalendar.GetMonth(targetDate);
            int day = lunarCalendar.GetDayOfMonth(targetDate);
            
            int leapMonth = lunarCalendar.GetLeapMonth(year);
            
            if (leapMonth > 0)
            {
                if (month == leapMonth) return false;
                if (month > leapMonth) month -= 1;
            }
            
            return month == 8 && day == 15;
        }
        catch
        {
            return false;
        }
    }
}
