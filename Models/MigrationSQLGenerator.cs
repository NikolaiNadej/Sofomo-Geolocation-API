using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using MySql.Data.Entity;

namespace GeolocationAPI.Models
{
    public class MigrationSqlGenerator : MySqlMigrationSqlGenerator
    {
        private string TrimSchemaPrefix(string table)
        {
            if (table.StartsWith("dbo."))
                return table.Replace("dbo.", "");
            return table;
        }

        protected override MigrationStatement Generate(CreateIndexOperation op)
        {
            var u = new MigrationStatement();
            string unique = op.IsUnique ? "UNIQUE" : "", columns = "";
            foreach (var col in op.Columns)
                columns += $"`{col}` DESC{(op.Columns.IndexOf(col) < op.Columns.Count - 1 ? ", " : "")}";
            u.Sql = $"CREATE {unique} INDEX `{op.Name}` ON `{TrimSchemaPrefix(op.Table)}` ({columns}) USING BTREE";
            return u;
        }
    }
}