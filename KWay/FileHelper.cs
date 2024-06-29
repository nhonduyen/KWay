using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KWay
{
    public static class FileHelper
    {
        public static async Task GenerateInputFileAsync(string filePath, int numOfNumber)
        {
            var random = new Random();
            var nums = new List<int>();

            for (int i = 0; i < numOfNumber; i++)
            {
                var num = random.Next(int.MaxValue);
                nums.Add(num);
            }
            await File.AppendAllLinesAsync(filePath, nums.ConvertAll(x => x.ToString()));
        }
    }
}
