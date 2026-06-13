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

    /// <summary>
    /// Kiểm tra xem một phần tử có tồn tại trong danh sách hay không.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của phần tử.</typeparam>
    /// <param name="collection">Danh sách cần kiểm tra.</param>
    /// <param name="item">Phần tử cần tìm.</param>
    /// <returns>True nếu phần tử có tồn tại, ngược lại False.</returns>
    public static bool CheckExist<T>(IEnumerable<T> collection, T item)
    {
        if (collection == null) return false;
        return collection.Contains(item);
    }

    /// <summary>
    /// Kiểm tra xem một phần tử có nằm trong danh sách các tham số được truyền vào hay không.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của phần tử.</typeparam>
    /// <param name="key">Phần tử cần kiểm tra.</param>
    /// <param name="keys">Các phần tử để đối chiếu (hỗ trợ truyền nhiều tham số rời rạc).</param>
    /// <returns>True nếu phần tử có nằm trong danh sách, ngược lại False.</returns>
    public static bool InArray<T>(T key, params T[] keys)
    {
        if (keys == null || keys.Length == 0) return false;
        return keys.Contains(key);
    }

    /// <summary>
    /// Chọn ngẫu nhiên một chỉ số (index) dựa trên mảng tỷ lệ phần trăm (hoặc trọng số).
    /// Rất hữu ích cho các tính năng rớt đồ (drop rate), quay thưởng, gacha.
    /// </summary>
    /// <param name="percent">Mảng chứa các tỷ lệ (trọng số).</param>
    /// <param name="max">Giá trị ngẫu nhiên tối đa (thường là tổng các tỷ lệ, ví dụ 100 hoặc 1000).</param>
    /// <returns>Chỉ số (index) của mảng tỷ lệ trúng thưởng.</returns>
    public static int RandomWithRate(int[] percent, int max)
    {
        if (percent == null || percent.Length == 0) return 0;

        int next = NextInt(max);
        int i;
        for (i = 0; i < percent.Length; i++)
        {
            if (next < percent[i])
            {
                return i;
            }
            next -= percent[i];
        }
        return i;
    }

    /// <summary>
    /// Chọn ngẫu nhiên một chỉ số (index) dựa trên mảng tỷ lệ (trọng số).
    /// Tự động tính toán tổng của toàn bộ mảng tỷ lệ làm giá trị tối đa.
    /// </summary>
    /// <param name="percent">Mảng chứa các tỷ lệ (trọng số).</param>
    /// <returns>Chỉ số (index) của mảng tỷ lệ trúng thưởng.</returns>
    public static int RandomWithRate(int[] percent)
    {
        if (percent == null || percent.Length == 0) return 0;
        
        int sum = percent.Sum();
        return RandomWithRate(percent, sum);
    }

    /// <summary>
    /// Sinh ra một giá trị boolean ngẫu nhiên (true hoặc false) với tỷ lệ 50-50.
    /// </summary>
    /// <returns>True hoặc False ngẫu nhiên.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NextBoolean()
    {
        return NextInt(2) == 0;
    }

    /// <summary>
    /// Nhóm danh sách các phần tử theo một khóa (key) xác định.
    /// </summary>
    /// <typeparam name="TE">Kiểu dữ liệu của phần tử.</typeparam>
    /// <typeparam name="TK">Kiểu dữ liệu của khóa (key).</typeparam>
    /// <param name="list">Danh sách đầu vào.</param>
    /// <param name="keySelector">Hàm chọn khóa từ một phần tử.</param>
    /// <returns>Dictionary chứa các danh sách phần tử đã được nhóm theo khóa.</returns>
    public static Dictionary<TK, List<TE>> GroupBy<TE, TK>(IEnumerable<TE> list, Func<TE, TK> keySelector) where TK : notnull
    {
        if (list == null)
        {
            return new Dictionary<TK, List<TE>>();
        }

        return list
            .Where(t => t != null && keySelector(t) != null)
            .GroupBy(keySelector)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}
