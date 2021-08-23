using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASEDemo
{
    public class LoopCommand
    {
        bool loopStarted = false;

        int startLineNo = 0, loopCount = 0;

        /// <summary>
        /// Resets loop command.
        /// </summary>
        public void Reset()
        {
            loopStarted = false;
        }

        /// <summary>
        /// Start of the loop.
        /// </summary>
        /// <param name="startLineNo"></param>
        public void SetStartLineNo(int startLineNo)
        {
            this.startLineNo = startLineNo;
        }

        /// <summary>
        /// Saves it.
        /// </summary>
        /// <returns></returns>
        public int getStartLineNo()
        {
            return this.startLineNo;
        }

        /// <summary>
        /// When loop is ended.
        /// </summary>
        /// <param name="allCommands"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int EndLoop(List<String> allCommands, int index)
        {
            loopCount--;

            if (loopCount > 0)
                return index;
            for (int i = 0; i < allCommands.Count; i++)
            {
                String line = allCommands[i];
                if (line.Trim().ToLower().Contains("endloop"))
                    return i;
            }

            return allCommands.Count - 1;   //Missing EndLoop
        }

        /// <summary>
        /// Count for the loop.
        /// </summary>
        /// <param name="loopCount"></param>
        internal void SetLoopCount(int loopCount)
        {
            if (!loopStarted)
            {
                this.loopCount = loopCount + 1;
                loopStarted = true;
            }
        }
    }
}