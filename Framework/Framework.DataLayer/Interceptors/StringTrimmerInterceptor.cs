using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace Framework.DataLayer.Interceptors
{
    /// <summary>
    /// Represents string trimmer interceptor
    /// </summary>
    public class StringTrimmerInterceptor : IDbCommandTreeInterceptor
    {

        private static readonly string[] TypesToTrim = { "nvarchar", "varchar", "char", "nchar" };
        /// <summary>
        /// This method is called after a new System.Data.Entity.Core.Common.CommandTrees.DbCommandTree
        /// has been created. The tree that is used after interception can be changed by
        /// setting System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Result
        /// while intercepting.
        /// </summary>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
            {
                return;
            }

            switch (interceptionContext.Result)
            {
                case DbInsertCommandTree insertCommand:
                    interceptionContext.Result = new DbInsertCommandTree(
                        insertCommand.MetadataWorkspace,
                        insertCommand.DataSpace,
                        insertCommand.Target,
                        insertCommand.SetClauses.Select(GetTrimmedStringValueClaus).ToList().AsReadOnly(),
                        insertCommand.Returning);
                    break;
                case DbUpdateCommandTree updateCommand:
                    interceptionContext.Result = new DbUpdateCommandTree(
                        updateCommand.MetadataWorkspace,
                        updateCommand.DataSpace,
                        updateCommand.Target,
                        updateCommand.Predicate,
                        updateCommand.SetClauses.Select(GetTrimmedStringValueClaus).ToList().AsReadOnly(),
                        updateCommand.Returning);
                    break;
            }
        }

        private static DbModificationClause GetTrimmedStringValueClaus(DbModificationClause a)
        {
            if (!(a is DbSetClause dbSetClause))
            {
                return a;
            }

            if (!(dbSetClause.Property is DbPropertyExpression dbPropertyExpression))
            {
                return a;
            }

            if (!(dbPropertyExpression.Property is EdmProperty edmProperty)
                || !TypesToTrim.Contains(edmProperty.TypeName))
            {
                return a;
            }

            if (!(dbSetClause.Value is DbConstantExpression dbConstantExpression) ||
                dbConstantExpression.Value == null)
            {
                return a;
            }

            return DbExpressionBuilder.SetClause(dbPropertyExpression,
                DbExpression.FromString(dbConstantExpression.Value.ToString().Trim())
            );
        }
    }
}
