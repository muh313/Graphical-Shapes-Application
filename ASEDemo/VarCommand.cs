using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEDemo
{
    public class VarCommand
    {
        private readonly Dictionary<String, int> variables = new Dictionary<String, int>();

        /// <summary>
        /// Resets for variables.
        /// </summary>
        public void Clear()
        {
            this.variables.Clear();
        }

        /// <summary>
        /// Stores name and value of variable.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(String name, int value)
        {
            this.variables.Add(name, value);
        }

        /// <summary>
        /// Name of string stored.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int Get(String name)
        {
            return this.variables[name];
        }

        /// <summary>
        /// Operators used between variables.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public int Parse(String statement)      // things you can do between variables
        {
            statement = statement.Trim();
            int value = 0;
            if (statement.Contains(" = "))
            {
                String[] tokens = statement.Split('=');

                this.variables[tokens[0].Trim().ToLower()] = Parse(tokens[1]);

                return 0;
            }
            else if (statement.Contains(" + "))
            {
                String[] tokens = statement.Split('+');

                return Parse(tokens[0]) + Parse(tokens[1]);
            }
            else if (statement.Contains(" - "))
            {
                String[] tokens = statement.Split('-');

                return Parse(tokens[0]) - Parse(tokens[1]);
            }
            else if (this.variables.ContainsKey(statement.ToLower()))
            {
                return this.variables[statement.ToLower()];
            }
            else if (int.TryParse(statement, out value))
            {
                return value;
            }
            return value;
        }
    }
}