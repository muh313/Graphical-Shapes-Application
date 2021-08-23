using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEDemo
{
    public class IfElseCommands
    {
        VarCommand varCommand;
        bool ifStatementHit = false;

        /// <summary>
        /// Resets the If.
        /// </summary>
        public void Reset()
        {
            ifStatementHit = false;
        }

        /// <summary>
        /// Uses variables made by user.
        /// </summary>
        /// <param name="varCommand"></param>
        public IfElseCommands(VarCommand varCommand)        // when a var is given after if/else declared
        {
            this.varCommand = varCommand;
        }

        /// <summary>
        /// Creates an If statement.
        /// </summary>
        /// <param name="allCommands"></param>
        /// <returns></returns>
        public int IfCommand(List<String> allCommands)
        {
            ifStatementHit = true;

            for (int i = 0; i < allCommands.Count; i++)
            {
                String line = allCommands[i];
                if (line.Trim().ToLower().Contains("else"))
                    return i - 1;
                if (line.Trim().ToLower().Contains("elseif"))
                    return i - 1;
            }
            return allCommands.Count - 1;   //Missing EndIF
        }

    }
}
