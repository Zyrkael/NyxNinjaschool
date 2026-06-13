
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace NyxNinjaschool.Utils;

public static partial class StringUtils
{
    [GeneratedRegex(@"\$\{([a-zA-Z0-9.]+)\}")]
    private static partial Regex FormatRegex();

    private static readonly ConcurrentDictionary<(Type, string), MemberInfo[]?> _pathCache = new();

    // 1. Hàm format chuỗi theo thuộc tính của Object
    public static string Format(string format, object? obj)
    {
        if (string.IsNullOrEmpty(format) || obj is null) return format;

        return FormatRegex().Replace(format, match =>
        {
            string key = match.Groups[1].Value;
            object? value = GetField(key, obj);
            return value?.ToString() ?? string.Empty;
        });
    }

    // 2. Hàm bổ trợ lấy thuộc tính (Reflection) hỗ trợ dấu chấm lồng nhau - Đã tối ưu bằng Cache
    private static object? GetField(string name, object? obj)
    {
        if (obj == null) return null;

        var members = _pathCache.GetOrAdd((obj.GetType(), name), key =>
        {
            var (type, path) = key;
            var parts = path.Split('.');
            var result = new MemberInfo[parts.Length];
            var currentType = type;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var prop = currentType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    result[i] = prop;
                    currentType = prop.PropertyType;
                }
                else
                {
                    var field = currentType.GetField(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (field != null)
                    {
                        result[i] = field;
                        currentType = field.FieldType;
                    }
                    else
                    {
                        return null; // Không tìm thấy
                    }
                }
            }
            return result;
        });

        if (members == null) return null;

        object? currentObj = obj;
        foreach (var member in members)
        {
            if (currentObj == null) return null;

            if (member is PropertyInfo prop)
            {
                currentObj = prop.GetValue(currentObj);
            }
            else if (member is FieldInfo field)
            {
                currentObj = field.GetValue(currentObj);
            }
        }

        return currentObj;
    }

    // 3. Hàm lặp ký tự (C# đã hỗ trợ sẵn trong Constructor của String)
    public static string Repeat(char c, int count)
    {
        if (count <= 0) return string.Empty;
        return new string(c, count);
    }

    // 4. Hàm xóa dấu tiếng Việt - Đã tối ưu bằng Span
    public static string RemoveAccent(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        string normalizedString = str.Normalize(NormalizationForm.FormD);
        Span<char> span = normalizedString.Length <= 512 
            ? stackalloc char[normalizedString.Length] 
            : new char[normalizedString.Length];
        
        int len = 0;
        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                if (c == 'Đ') span[len++] = 'D';
                else if (c == 'đ') span[len++] = 'd';
                else span[len++] = c;
            }
        }

        return new string(span[..len]).Normalize(NormalizationForm.FormC);
    }

    // 5. Hàm kiểm tra mật khẩu BCrypt
    public static bool CheckPassword(string hashed, string plaintext)
    {
        if (string.IsNullOrEmpty(hashed) || string.IsNullOrEmpty(plaintext))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(plaintext, hashed);
        }
        catch
        {
            return false; // Trả về false nếu hash không đúng định dạng BCrypt
        }
    }
}