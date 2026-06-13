# Coding Guidelines for AI Agents

These rules define the coding standards, formatting, and performance guidelines for the C# (.NET 8) codebase in this repository. All AI agents must strictly adhere to these rules when modifying or writing new code.

## 1. Naming Conventions
- **Classes, Structs, Records, Enums, Methods, Properties:** Use `PascalCase`.
- **Interfaces:** Use `PascalCase` with a capital `I` prefix (e.g., `IUserService`).
- **Private Instance Fields:** Use `_camelCase` with an underscore prefix (e.g., `_httpClient`).
- **Local Variables & Parameters:** Use `camelCase`.
- **Constants & Static Readonly Fields:** Use `PascalCase` (e.g., `DefaultTimeout`, `PathCache`). Do NOT use `_camelCase` for these.

## 2. Modern C# (.NET 8) Features
- **File-scoped Namespaces:** Always use file-scoped namespaces (e.g., `namespace NyxNinjaschool.Utils;`) instead of block-scoped namespaces.
- **Nullable Reference Types (NRTs):** The project has `<Nullable>enable</Nullable>`. Properly annotate nullable types with `?` (e.g., `string?`, `object?`) and handle null checks gracefully.
- **Implicit Usings:** The project uses `<ImplicitUsings>enable</ImplicitUsings>`. Do not add redundant `using` statements (like `System`, `System.Linq`, `System.Collections.Generic`) unless necessary.
- **Target-typed `new`:** Use `new()` instead of repeating the type name when the type is obvious from the context (e.g., `List<string> list = new();`).
- **Pattern Matching:** Use modern pattern matching (`is null`, `is not null`, `switch` expressions) for cleaner logic.

## 3. Performance & Optimizations
- **Zero/Low Allocation:** Prefer `Span<T>`, `ReadOnlySpan<T>`, and `Memory<T>` over allocating new arrays or substrings when working with buffers or string manipulation. Use `stackalloc` for small localized buffers.
- **String Manipulation:** Avoid `+` concatenation in loops. Use `StringBuilder`, `string.Create`, or interpolated strings.
- **Regular Expressions:** Use `[GeneratedRegex]` for compile-time generated regexes instead of initializing `Regex` instances at runtime.
- **Reflection:** Avoid using Reflection (`GetProperty`, `GetField`) in performance-critical loops. If required, cache the `MemberInfo` or `PropertyInfo` delegates (e.g., using `ConcurrentDictionary`).

## 4. Formatting and Structure
- **Braces (Allman Style):** Always place curly braces `{` and `}` on a new line.
- **Code Preservation:** Do not randomly delete existing comments, docstrings, or code unless explicitly instructed or directly related to the fix.
- **Documentation:** Do not include comments comparing C# code to Java equivalents (e.g., `Tương đương với ... trong Java`). Write documentation focused solely on the C# context.
- **Early Returns:** Keep the "happy path" unindented by returning early when encountering invalid arguments or edge cases.
- **Clean Code & IDE Warnings:**
  - Remove redundant field initializers (e.g., initializing to `0`, `false`, or default enum values).
  - Use the ternary operator (`?:`) instead of simple `if/else` blocks when returning/assigning values or calling methods with different arguments.
  - Avoid redundant conditional access (`?.`) on variables/fields guaranteed to be non-null by nullable reference type annotations (e.g., `readonly` fields initialized in constructors).

## 5. Security
- Never hardcode sensitive information (secrets, keys, passwords). Use `appsettings.json` or environment variables (via `IConfiguration`).
- Use standardized security libraries (e.g., `BCrypt.Net-Next`) for cryptography and hashing instead of rolling out custom logic.

## 6. Common Pitfalls & Ambiguities
- **Ambiguous Types:** When using types with common names like `Timer` in projects with `<ImplicitUsings>enable</ImplicitUsings>` (which includes `System.Threading`), fully qualify the type (e.g., `System.Timers.Timer`) or use a using alias (`using Timer = System.Timers.Timer;`) to prevent ambiguity errors between `System.Threading.Timer` and `System.Timers.Timer`.
