using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
//        public Dictionary<TRow, Dictionary<TColumn, TValue>> dict;
        public TValue[][] Values;
        public OpenIndexTable<TRow, TColumn, TValue> Open { get; set; }
        public ExistedIndexTable<TRow, TColumn, TValue> Existed { get; set; }
        public List<TRow> Rows { get; set; }
        public List<TColumn> Columns { get; set; }

        public Table()
        {
            Open = new OpenIndexTable<TRow, TColumn, TValue>(this);
            Existed = new ExistedIndexTable<TRow, TColumn, TValue>(this);
            Rows = new List<TRow>();
            Columns = new List<TColumn>();
            Values = new []{new TValue[1]};
        }

        public void AddRow(TRow row)
        {
            if (Rows.Contains(row)) return;
            Rows.Add(row);
            if (Values.Length > Rows.Count) return;
            var copy = Values;
            Values = new TValue[copy.Length * 2][];
            for (var i = 0; i < copy.Length; i++)
                Values[i] = copy[i];
        }

        public void AddColumn(TColumn column)
        {
            if (Columns.Contains(column)) return;
            Columns.Add(column);

            if (Values[0].Length > Columns.Count) return;
            for (var i = 0; i < Values.Length; i++)
            {
                var copy = Values[i];
                Values[i] = new TValue[copy?.Length * 2 ?? 1];
                for (var j = 0; j < (copy?.Length ?? 0); j++)
                    Values[i][j] = copy[j];
            }
        }
    }

    public class OpenIndexTable<TRow, TColumn, TValue>
    {
        private Table<TRow, TColumn, TValue> table;

        public OpenIndexTable(Table<TRow, TColumn, TValue> table)
        {
            this.table = table;
        }

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                try
                {
                    return table.Values[table.Rows.IndexOf(row)][table.Columns.IndexOf(column)];
                }
                catch (IndexOutOfRangeException e)
                {
                    return default(TValue);
                }
            }
            set
            {
                if(!table.Rows.Contains(row))
                    table.AddRow(row);
                if(!table.Columns.Contains(column))
                    table.AddColumn(column);
                table.Values[table.Rows.Count - 1][table.Columns.Count - 1] = value;
            }
        }
    }

    public class ExistedIndexTable<TRow, TColumn, TValue>
    {
        private Table<TRow, TColumn, TValue> table;

        public ExistedIndexTable(Table<TRow, TColumn, TValue> table)
        {
            this.table = table;
        }

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (!table.Rows.Contains(row) || !table.Columns.Contains(column))
                    throw new ArgumentException();

                try
                {
                    return table.Values[table.Rows.IndexOf(row)][table.Columns.IndexOf(column)];
                }
                catch (IndexOutOfRangeException e)
                {
                    return default(TValue);
                }
            }
            set
            {
                if (!table.Rows.Contains(row) || !table.Columns.Contains(column))
                    throw new ArgumentException();
                table.Values[table.Rows.Count - 1][table.Columns.Count - 1] = value;
            }
        }
    }
}
