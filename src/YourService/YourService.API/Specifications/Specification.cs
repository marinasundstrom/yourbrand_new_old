﻿using System.Linq;
using System.Linq.Expressions;

using LinqKit;

namespace YourBrand.YourService.API.Domain.Specifications;

public abstract class Specification<T>
{
    public readonly static Specification<T> All = new AllSpecification<T>();

    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }

    public Specification<T> And(Specification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    public Specification<T> Or(Specification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    public Specification<T> AndNot(Specification<T> specification)
    {
        return new AndNotSpecification<T>(this, specification);
    }

    public Specification<T> OrNot(Specification<T> specification)
    {
        return new OrNotSpecification<T>(this, specification);
    }
}

class AllSpecification<T> : Specification<T>
{
    public AllSpecification()
    {

    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expr = (todo) => true;

        return expr;
    }
}

public class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        if (_left is AllSpecification<T>)
        {
            return _right.ToExpression();
        }

        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression andExpression = Expression.AndAlso(
            leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(
            andExpression, leftExpression.Parameters.Single());
    }
}

public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        if (_left is AllSpecification<T>)
        {
            return _right.ToExpression();
        }

        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression orExpression = Expression.OrElse(
            leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(
            orExpression, leftExpression.Parameters.Single());
    }
}

public class AndNotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndNotSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        if (_left is AllSpecification<T>)
        {
            return _right.ToExpression();
        }

        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        UnaryExpression negateExpression = Expression.Negate(rightExpression.Body);

        BinaryExpression andExpression = Expression.AndAlso(
            leftExpression.Body, negateExpression);

        return Expression.Lambda<Func<T, bool>>(
            andExpression, leftExpression.Parameters.Single());
    }
}

public class OrNotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrNotSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        if (_left is AllSpecification<T>)
        {
            return _right.ToExpression();
        }

        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        UnaryExpression negateExpression = Expression.Negate(rightExpression.Body);

        BinaryExpression andExpression = Expression.OrElse(
            leftExpression.Body, negateExpression);

        return Expression.Lambda<Func<T, bool>>(
            andExpression, leftExpression.Parameters.Single());
    }
}