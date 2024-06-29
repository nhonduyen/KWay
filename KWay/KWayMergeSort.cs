namespace KWay
{
    public static class KWayMergeSort
    {
        public static async Task<List<string>> SplitAndSortChunksAsync(string inputFilePath, int chunkSize)
        {
            List<string> chunkFiles = new List<string>();
            using var reader = new StreamReader(inputFilePath);

            var chunk = new List<int>();
            string line;
            int chunkCounter = 0;

            while ((line = reader.ReadLine()) != null)
            {
                chunk.Add(int.Parse(line));
                if (chunk.Count >= chunkSize)
                {
                    var chunkFileName = await SortAndSaveChunk(chunk, chunkCounter++);
                    chunkFiles.Add(chunkFileName);
                    Console.WriteLine($"Add chunk {chunkFileName}");
                    chunk.Clear();
                }
            }

            // Sort and save the last chunk
            if (chunk.Count > 0)
            {
                var chunkFileName = await SortAndSaveChunk(chunk, chunkCounter++);
                chunkFiles.Add(chunkFileName);
                Console.WriteLine($"Add chunk {chunkFileName}");
            }

            return chunkFiles;
        }

        public static async Task<string> SortAndSaveChunk(List<int> chunk, int chunkCounter)
        {
            chunk.Sort();
            string chunkFileName = $"chunk_{chunkCounter}.txt";
            await File.WriteAllLinesAsync(chunkFileName, chunk.ConvertAll(x => x.ToString()));
            return chunkFileName;
        }

        public static void MergeSortedChunks(List<string> sortedChunkFiles, string outputFilePath)
        {
            var readers = new List<StreamReader>();

            foreach (string file in sortedChunkFiles)
            {
                readers.Add(new StreamReader(file));
            }

            using var writer = new StreamWriter(outputFilePath);

            var minHeap = new PriorityQueue<Element, int>();

            // Initialize heap with the first element of each file
            for (int i = 0; i < readers.Count; i++)
            {
                if (!readers[i].EndOfStream)
                {
                    string line = readers[i].ReadLine();
                    minHeap.Enqueue(new Element { Value = int.Parse(line), ReaderIndex = i }, int.Parse(line));
                }
            }

            while (minHeap.Count > 0)
            {
                Element minElement = minHeap.Dequeue();
                writer.WriteLine(minElement.Value);

                // Read the next element from the same file and add it to the heap
                if (!readers[minElement.ReaderIndex].EndOfStream)
                {
                    string line = readers[minElement.ReaderIndex].ReadLine();
                    minHeap.Enqueue(new Element { Value = int.Parse(line), ReaderIndex = minElement.ReaderIndex }, int.Parse(line));
                }
            }


            foreach (StreamReader reader in readers)
            {
                reader.Dispose();
            }
        }
    }
}
