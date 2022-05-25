using System;
using System.Linq.Expressions;

namespace Framework.Domain.Specifications
{
    /// <summary>
    /// Describes specification
    /// </summary>
    public interface ISpecification
    {
    }

    /// <summary>
    /// Describes custom specification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICustomSpecification<TEntity>: ISpecification<TEntity> where TEntity : class
    {
        /// <summary>
        /// Sets expression values for specification
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="values">Values for specification</param>
        void SetValue(string fieldName, object[] values);
    }

    /// <summary>
    /// Describes ordinal specification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity> : ISpecification where TEntity : class
    {
        /// <summary>
        /// Returns expression of specification
        /// </summary>
        Expression<Func<TEntity,bool>> GetExpression();
        /// <summary>
        /// Combines specifications as And
        /// </summary>
        /// <param name="specification">Right part of combination</param>
        ISpecification<TEntity> And(ISpecification<TEntity> specification);
        /// <summary>
        /// Combines specifications as Or
        /// </summary>
        /// <param name="specification">Right part of combination</param>
        ISpecification<TEntity> Or(ISpecification<TEntity> specification);
        /// <summary>
        /// Combines specifications as Not
        /// </summary>
        /// <param name="specification">Right part of combination</param>
        ISpecification<TEntity> Not(ISpecification<TEntity> specification);
    }
}
