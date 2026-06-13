using System;
using Serilog;

namespace NyxNinjaschool.Utils;

/// <summary>
/// Wrapper class cho Serilog để giúp việc chuyển đổi (port) code từ Java sang C# dễ dàng hơn.
/// Các hàm được mô phỏng theo cấu trúc hàm Log.java cũ.
/// </summary>
public static class NinjaLog
{
    // Info Level Logs
    public static void Info(string message) => Log.Information(message);
    
    public static void Info(object obj) => Log.Information(obj?.ToString() ?? "null");

    // Warn Level Logs
    public static void Warn(string message) => Log.Warning(message);
    
    public static void Warn(object obj) => Log.Warning(obj?.ToString() ?? "null");

    // Error Level Logs
    public static void Error(string message) => Log.Error(message);
    
    public static void Error(object obj) => Log.Error(obj?.ToString() ?? "null");

    public static void Error(string message, Exception exception) => Log.Error(exception, message);

    // Fatal Level Logs
    public static void Fatal(string message) => Log.Fatal(message);

    // Debug Level Logs
    public static void Debug(string message) => Log.Debug(message);
    
    public static void Debug(object obj) => Log.Debug(obj?.ToString() ?? "null");
}
