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
    /// Lấy thời gian hiện tại tính bằng mili-giây kể từ Unix Epoch (tương đương System.currentTimeMillis() trong Java).
    /// </summary>
    /// <returns>Thời gian hiện tại bằng mili-giây.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long CurrentTimeMillis()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
