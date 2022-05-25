﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Utilities;
using System.Linq;

namespace FilingPortal.Parts.Common.DataLayer.Base
{
    /// <summary>
    /// Code generator for FP migrations
    /// </summary>
    public class FpMigrationCodeGenerator : CSharpMigrationCodeGenerator
    {
        protected override void WriteClassStart(string @namespace, string className, IndentedTextWriter writer, string @base, bool designer = false, IEnumerable<string> namespaces = null)
        {
            writer.WriteLine("// Generated Time: {0}", DateTime.Now);
            writer.WriteLine("// Generated By: {0}", Environment.UserName);
            writer.WriteLine();

            @base = @base == "DbMigration" ? "FpMigration" : @base;
            base.WriteClassStart(@namespace, className, writer, @base, designer, namespaces);
        }

        /// <summary>
        /// Gets the default namespaces that must be output as "using" or "Imports" directives for
        /// any code generated.
        /// </summary>
        /// <param name="designer"> A value indicating if this class is being generated for a code-behind file. </param>
        /// <returns> An ordered list of namespace names. </returns>
        protected override IEnumerable<string> GetDefaultNamespaces(bool designer = false)
        {
            return base.GetDefaultNamespaces(designer).Union(new[] { "FilingPortal.Parts.Common.DataLayer.Base" });
        }
    }
}
