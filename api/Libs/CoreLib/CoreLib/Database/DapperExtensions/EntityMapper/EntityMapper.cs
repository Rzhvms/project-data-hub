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
/// var table = EntityMapper.TbName&lt;User&gt;(); - "User"
/// var column = EntityMapper.ColName&lt;User&gt;(x => x.Id); - "Id"
/// </code>
/// </example>
public static class EntityMapper
{
    private static readonly ConcurrentDictionary<Type, string> TableNameCache = new();
    private static readonly ConcurrentDictionary<MemberInfo, string> ColumnNameCache = new();

    /// <summary>
    /// Возвращает имя таблицы для сущности типа <typeparamref name="TEntity"/>.
    /// </summary>
    public static string TbName<TEntity>() => TbName(typeof(TEntity));

    /// <summary>
    /// Возвращает имя таблицы для указанного CLR-типа.
    /// </summary>
    private static string TbName(Type entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);
        return TableNameCache.GetOrAdd(entityType, static t => t.Name);
    }

    /// <summary>
    /// Возвращает имя колонки, соответствующей свойству или полю сущности.
    /// </summary>
    public static string ColName<TEntity>(Expression<Func<TEntity, object?>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var member = ExtractMember(expression);
        return ColumnNameCache.GetOrAdd(member, static m => m.Name);
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
}