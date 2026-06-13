using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NyxNinjaschool.Utils;

/// <summary>
/// Lớp tiện ích cung cấp các hàm hỗ trợ chung cho hệ thống, đặc biệt là các hàm xử lý ngẫu nhiên (Random) thread-safe.
/// </summary>
public static class NinjaUtils
{
    /// <summary>
    /// Sinh một số nguyên ngẫu nhiên từ 0 đến <paramref name="maxValue"/> - 1.
    /// </summary>
    /// <param name="maxValue">Giá trị giới hạn trên (không bao gồm).</param>
    /// <returns>Số nguyên ngẫu nhiên trong khoảng [0, maxValue).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(int maxValue)
    {
        if (maxValue <= 0) return 0;
        return Random.Shared.Next(maxValue);
    }

    /// <summary>
    /// Sinh một số nguyên ngẫu nhiên nằm trong khoảng [<paramref name="minValue"/>, <paramref name="maxValue"/> - 1].
    /// </summary>
    /// <param name="minValue">Giá trị giới hạn dưới (bao gồm).</param>
    /// <param name="maxValue">Giá trị giới hạn trên (không bao gồm).</param>
    /// <returns>Số nguyên ngẫu nhiên trong khoảng [minValue, maxValue).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextInt(int minValue, int maxValue)
    {
        if (minValue >= maxValue) return minValue;
        return Random.Shared.Next(minValue, maxValue);
    }

    /// <summary>
    /// Sinh một số nguyên ngẫu nhiên nằm trong khoảng [<paramref name="minValue"/>, <paramref name="maxValue"/>] (Bao gồm cả giá trị max).
    /// </summary>
    /// <param name="minValue">Giá trị giới hạn dưới (bao gồm).</param>
    /// <param name="maxValue">Giá trị giới hạn trên (bao gồm).</param>
    /// <returns>Số nguyên ngẫu nhiên trong khoảng [minValue, maxValue].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int NextIntInclusive(int minValue, int maxValue)
    {
        if (minValue >= maxValue) return minValue;
        return Random.Shared.Next(minValue, maxValue + 1);
    }

    /// <summary>
    /// Sinh một số thực (double) ngẫu nhiên nằm trong khoảng [0.0, 1.0).
    /// </summary>
    /// <returns>Số thực ngẫu nhiên từ 0.0 đến dưới 1.0.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double NextDouble()
    {
        return Random.Shared.NextDouble();
    }

    /// <summary>
    /// Lấy ngẫu nhiên một phần tử từ một danh sách (List, Array, v.v.).
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của phần tử.</typeparam>
    /// <param name="list">Danh sách đầu vào.</param>
    /// <returns>Phần tử ngẫu nhiên, hoặc giá trị mặc định nếu danh sách rỗng.</returns>
    public static T? RandomElement<T>(IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }
        
        return list[Random.Shared.Next(list.Count)];
    }

    /// <summary>
    /// Lấy ngẫu nhiên một phần tử từ các tham số truyền vào (varargs).
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của các phần tử.</typeparam>
    /// <param name="items">Mảng các phần tử truyền vào (hỗ trợ truyền nhiều tham số rời rạc).</param>
    /// <returns>Phần tử ngẫu nhiên được chọn, hoặc giá trị mặc định nếu danh sách rỗng.</returns>
    public static T? RandomObject<T>(params T[] items)
    {
        if (items == null || items.Length == 0)
        {
            return default;
        }
        return items[Random.Shared.Next(items.Length)];
    }
}
