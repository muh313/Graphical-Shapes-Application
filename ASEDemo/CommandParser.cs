using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEDemo
{
    public class CommandParser  //made public for unit tests
    {
        readonly VarCommand varCommand;

        /// <summary>
        /// Uses Variable class for parsing.
        /// </summary>
        /// <param name="varCommand"></param>
        public CommandParser(VarCommand varCommand)
        {
            this.varCommand = varCommand;
        }

        /// <summary>
        /// Digests/stores command for operation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public string[] TokenizeCommand(string input, string commandText)   //parser method
        {
            var trimmed = input.Trim();
            var pastCommand = trimmed.Substring(commandText.Length);

            // Now pastCommand be something like " 100,100".
            // I can split that
            var tokens = pastCommand.Split(',');

            int i = 0;
            foreach (var token in tokens)
            {
                tokens[i++] = this.varCommand.Parse(token).ToString();
            }
            return tokens;
        }
    }
}
