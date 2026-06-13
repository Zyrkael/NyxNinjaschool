
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

    private static readonly ConcurrentDictionary<(Type, string), MemberInfo[]?> PathCache = new();

    /// <summary>
    /// Thay thế các chuỗi dạng ${PropertyName} hoặc ${Property.SubProperty} bằng giá trị tương ứng từ đối tượng cung cấp.
    /// </summary>
    /// <param name="format">Chuỗi định dạng (vd: "Hello ${User.Name}").</param>
    /// <param name="obj">Đối tượng chứa dữ liệu để thay thế.</param>
    /// <returns>Chuỗi đã được định dạng với các giá trị thực tế.</returns>
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

    /// <summary>
    /// Lấy giá trị của thuộc tính hoặc trường từ một đối tượng (sử dụng Reflection Cache) hỗ trợ truy cập lồng nhau bằng dấu chấm.
    /// </summary>
    /// <param name="name">Tên hoặc đường dẫn thuộc tính (vd: "Address.City").</param>
    /// <param name="obj">Đối tượng gốc.</param>
    /// <returns>Giá trị của thuộc tính tương ứng, hoặc null nếu không tìm thấy.</returns>
    private static object? GetField(string name, object? obj)
    {
        if (obj == null) return null;

        var members = PathCache.GetOrAdd((obj.GetType(), name), key =>
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

    /// <summary>
    /// Tạo một chuỗi bằng cách lặp lại ký tự đã cho với số lượng chỉ định.
    /// </summary>
    /// <param name="c">Ký tự cần lặp.</param>
    /// <param name="count">Số lượng ký tự trong chuỗi kết quả.</param>
    /// <returns>Chuỗi kết quả.</returns>
    public static string Repeat(char c, int count)
    {
        if (count <= 0) return string.Empty;
        return new string(c, count);
    }

    /// <summary>
    /// Loại bỏ toàn bộ dấu (thanh điệu) khỏi chuỗi tiếng Việt.
    /// </summary>
    /// <param name="str">Chuỗi tiếng Việt có dấu.</param>
    /// <returns>Chuỗi tiếng Việt không dấu.</returns>
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

    /// <summary>
    /// So sánh mật khẩu dạng văn bản thô với chuỗi mã hóa BCrypt xem có khớp nhau hay không.
    /// </summary>
    /// <param name="hashed">Chuỗi mã hóa BCrypt.</param>
    /// <param name="plaintext">Mật khẩu dạng văn bản thô do người dùng nhập.</param>
    /// <returns>True nếu mật khẩu chính xác, ngược lại False.</returns>
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