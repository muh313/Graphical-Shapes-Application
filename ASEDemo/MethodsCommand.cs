using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ASEDemo
{
    public class MethodCommand
    {
        private Stack<int> methodCallStack = new Stack<int>();
        private Dictionary<String, int> methodList = new Dictionary<string, int>();

        /// <summary>
        /// Reset for method command.
        /// </summary>
        public void Reset()
        {
            methodCallStack.Clear();
            methodList.Clear();
        }

        /// <summary>
        /// Creates a method storing everything within.
        /// </summary>
        /// <param name="allCommands"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int RegisterMethod(List<String> allCommands, int index)
        {
            String commands = allCommands[index];
            String methodName = commands.Substring("Method ".Length - 1).Trim().ToLower();
            methodList[methodName] = index;

            for (int i = index; i < allCommands.Count; i++)
            {
                String line = allCommands[i];
                if (line.Trim().ToLower().Contains("endmethod"))
                    return i;
            }
            return allCommands.Count - 1;   //Missing EndMethod
        }

        /// <summary>
        /// Stores all commands.
        /// </summary>
        /// <param name="allCommands"></param>
        /// <returns></returns>
        public int ReturnMethod(List<String> allCommands)
        {
            return methodCallStack.Pop();
        }

        /// <summary>
        /// Calling everything the method had stored ().
        /// </summary>
        /// <param name="allCommands"></param>
        /// <param name="methodName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int CallMethod(List<String> allCommands, string methodName, int index)
        {
            methodCallStack.Push(index);

            if (methodList.ContainsKey(methodName))
                return methodList[methodName];

            return index;
        }
    }
}