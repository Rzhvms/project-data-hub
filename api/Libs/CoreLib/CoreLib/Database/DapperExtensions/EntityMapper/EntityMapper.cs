using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreLib.Database.DapperExtensions.EntityMapper;

/// <summary>
/// Вспомогательный компонент для получения имен таблиц и колонок на основе моделей домена.
/// Используется при генерации SQL-запросов, миграций и других сценариях.
/// </summary>
/// <example>
/// <code>
/// var table = EntityMapper.TbName&lt;User&gt;();                  // "User"
/// var quotedTable = EntityMapper.TbName&lt;User&gt;(withQuotes: false);        // User
/// var column = EntityMapper.ColName&lt;User&gt;(x => x.Id);       // "Id"
/// var quotedColumn = EntityMapper.ColName&lt;User&gt;(x => x.Id, withQuotes: false); // "Id"
/// </code>
/// </example>
public static class EntityMapper
{
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();
    private static readonly ConcurrentDictionary<MemberInfo, string> ColumnNameCache = new();

    /// <summary>
    /// Возвращает имя таблицы для сущности типа <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="withQuotes">Нужно ли оборачивать имя в двойные кавычки.</param>
    public static string TbName<TEntity>(bool withQuotes = true) => TbName(typeof(TEntity), withQuotes);

    /// <summary>
    /// Возвращает имя таблицы для указанного CLR-типа.
    /// </summary>
    /// <param name="entityType">Тип сущности.</param>
    /// <param name="withQuotes">Нужно ли оборачивать имя в двойные кавычки.</param>
    private static string TbName(Type entityType, bool withQuotes = true)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        var name = TableNameCache.GetOrAdd(entityType, static t => t.Name);
        return withQuotes ? Quote(name) : name;
    }

    /// <summary>
    /// Возвращает имя колонки, соответствующей свойству или полю сущности.
    /// </summary>
    /// <param name="expression">Выражение, указывающее на свойство или поле.</param>
    /// <param name="withQuotes">Нужно ли оборачивать имя в двойные кавычки.</param>
    public static string ColName<TEntity>(Expression<Func<TEntity, object?>> expression, bool withQuotes = true)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var member = ExtractMember(expression);
        var name = ColumnNameCache.GetOrAdd(member, static m => m.Name);

        return withQuotes ? Quote(name) : name;
    }

    /// <summary>
    /// Извлекает <see cref="MemberInfo"/> из лямбда-выражения,
    /// поддерживая вложенные свойства и приведения типов.
    /// </summary>
    private static MemberInfo ExtractMember<TEntity>(Expression<Func<TEntity, object?>> expression)
    {
        var body = expression.Body;

        // Убираем boxing / Convert для value-type (x => (object)x.Id)
        while (body is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
        {
            body = unary.Operand;
        }

        var memberExpression = ExtractDeepestMemberExpression(body);
        return memberExpression.Member;
    }

    /// <summary>
    /// Извлечение самого глубокого <see cref="MemberExpression"/> из выражения.
    /// </summary>
    private static MemberExpression ExtractDeepestMemberExpression(Expression expression)
    {
        MemberExpression? result = null;
        var current = expression;

        while (current is MemberExpression member)
        {
            result = member;
            current = member.Expression!;
        }

        if (result is null)
        {
            throw new ArgumentException(
                $"Expression '{expression}' must be a property or field access.");
        }

        return result;
    }

    /// <summary>
    /// Оборачивает параметр в двойные кавычки.
    /// </summary>
    private static string Quote(string parameter)
    {
        return $"\"{parameter}\"";
    }
}