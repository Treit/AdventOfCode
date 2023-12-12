using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    internal class SpringChecker
    {
        string _report;
        string _damagedGroups;
        List<int> _indexes;

        public SpringChecker(string input)
        {
            var tokens = input.Split(" ");
            _report = tokens[0];
            _damagedGroups = tokens[1];
            _indexes = new();

            for (int i = 0; i < _report.Length; i++)
            {
                if (_report[i] == '?')
                {
                    _indexes.Add(i);
                }
            }

            var sb = new StringBuilder();
        }

        public int DoCheck()
        {
            var answer = 0;
            var temp = new bool[_indexes.Count];
            var combos = new List<bool[]>();

            Combos(temp, 0, combos);

            var buff = new char[_report.Length];

            foreach (var combo in combos)
            {
                var cidx = 0;

                for (int i = 0; i < buff.Length; i++)
                {
                    if (_report[i] != '?')
                    {
                        buff[i] = _report[i];
                        continue;
                    }
                    else
                    {
                        buff[i] = combo[cidx++] ? '#' : '.';
                    }
                }

                var str = new string(buff);
                var gc = GroupCount(str);
                if (gc == _damagedGroups)
                {
                    answer++;
                }
            }

            return answer;
        }

        string GroupCount(string input)
        {
            var groups = input.Split('.', StringSplitOptions.RemoveEmptyEntries);
            return string.Join(',', groups.Select(x => x.Length));
        }

        static void Combos(bool[] values, int index, List<bool[]> results)
        {
            if (index == values.Length)
            {
                results.Add(values.ToArray());
            }
            else
            {
                values[index] = true;
                Combos(values, index + 1, results);

                values[index] = false;
                Combos(values, index + 1, results);
            }
        }
    }
}
